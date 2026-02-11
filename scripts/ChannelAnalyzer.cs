using System;
using System.Collections.Generic;

namespace BiphaseDecoder;

public enum ChannelType
{
    Data,
    Audio,
    Unknown
}

public class ChannelClassification
{
    public int ChannelIndex { get; set; }
    public ChannelType Type { get; set; }
    public double ZeroCrossingRate { get; set; }
    public double IntervalSharpness { get; set; }
    public int SyncCount { get; set; }
    public BiphaseVariant BestVariant { get; set; }
    public BitOrder BestBitOrder { get; set; }
}

public class AnalysisResult
{
    public List<ChannelClassification> Channels { get; set; } = new();
    public List<int> DataChannelIndices { get; set; } = new();
    public List<int> AudioChannelIndices { get; set; } = new();
    public BiphaseVariant DetectedVariant { get; set; }
    public BitOrder DetectedBitOrder { get; set; }
}

public static class ChannelAnalyzer
{
    private const double ExpectedMinCrossingRate = 3500.0;
    private const double ExpectedMaxCrossingRate = 10000.0;
    private const double AnalysisDuration = 5.0;

    public static AnalysisResult Analyze(AudioData audio, bool verbose = false)
    {
        var result = new AnalysisResult();

        for (int ch = 0; ch < audio.ChannelCount; ch++)
        {
            var classification = AnalyzeChannel(audio.Channels[ch], audio.SampleRate, ch, verbose);
            result.Channels.Add(classification);

            if (classification.Type == ChannelType.Data)
                result.DataChannelIndices.Add(ch);
            else if (classification.ZeroCrossingRate >= 1.0)
                result.AudioChannelIndices.Add(ch);
        }

        if (result.DataChannelIndices.Count == 0 && result.Channels.Count > 0)
        {
            int bestCh = 0;
            int bestSync = 0;
            for (int i = 0; i < result.Channels.Count; i++)
            {
                if (result.Channels[i].SyncCount > bestSync)
                {
                    bestSync = result.Channels[i].SyncCount;
                    bestCh = i;
                }
            }
            result.Channels[bestCh].Type = ChannelType.Data;
            result.DataChannelIndices.Add(bestCh);
            result.AudioChannelIndices.Remove(bestCh);
        }

        if (result.DataChannelIndices.Count > 0)
        {
            var bestData = result.Channels[result.DataChannelIndices[0]];
            result.DetectedVariant = bestData.BestVariant;
            result.DetectedBitOrder = bestData.BestBitOrder;
        }

        return result;
    }

    private static ChannelClassification AnalyzeChannel(float[] samples, int sampleRate, int channelIndex,
        bool verbose)
    {
        var classification = new ChannelClassification { ChannelIndex = channelIndex };

        int analysisLength = Math.Min(samples.Length, (int)(AnalysisDuration * sampleRate));
        var analysisSegment = new float[analysisLength];
        Array.Copy(samples, analysisSegment, analysisLength);

        var crossings = BiphaseMarkDecoder.DetectZeroCrossings(analysisSegment, sampleRate, 0.01f);
        double duration = (double)analysisLength / sampleRate;
        classification.ZeroCrossingRate = crossings.Count / duration;

        classification.IntervalSharpness = ComputeIntervalSharpness(crossings, sampleRate);

        int bestSyncCount = 0;
        BiphaseVariant bestVariant = BiphaseVariant.BMC;
        BitOrder bestBitOrder = BitOrder.MsbFirst;

        foreach (var variant in new[] { BiphaseVariant.BMC, BiphaseVariant.BSC })
        {
            var decoded = BiphaseMarkDecoder.Decode(analysisSegment, sampleRate, variant, 0.01f);

            foreach (var order in new[] { BitOrder.MsbFirst, BitOrder.LsbFirst })
            {
                int syncCount = FrameSynchronizer.CountSyncBytes(decoded.Bits, order);

                if (verbose)
                {
                    Console.WriteLine($"  Ch{channelIndex}: {variant}/{order} -> " +
                        $"{decoded.Bits.Count} bits, {syncCount} syncs, " +
                        $"{decoded.InvalidIntervals} invalid intervals");
                }

                if (syncCount > bestSyncCount)
                {
                    bestSyncCount = syncCount;
                    bestVariant = variant;
                    bestBitOrder = order;
                }
            }
        }

        classification.SyncCount = bestSyncCount;
        classification.BestVariant = bestVariant;
        classification.BestBitOrder = bestBitOrder;

        bool crossingRateOk = classification.ZeroCrossingRate >= ExpectedMinCrossingRate &&
                               classification.ZeroCrossingRate <= ExpectedMaxCrossingRate;
        bool hasSyncs = bestSyncCount >= 3;
        bool sharpIntervals = classification.IntervalSharpness > 0.3;

        classification.Type = (hasSyncs || (crossingRateOk && sharpIntervals))
            ? ChannelType.Data
            : ChannelType.Audio;

        return classification;
    }

    private static double ComputeIntervalSharpness(List<double> crossings, int sampleRate)
    {
        if (crossings.Count < 10) return 0;

        double nominalHalf = 1.0 / (4500.0 * 2);
        double nominalFull = 1.0 / 4500.0;
        double tolerance = 0.30;

        int nearPeak = 0;
        int total = 0;

        for (int i = 1; i < crossings.Count; i++)
        {
            double interval = crossings[i] - crossings[i - 1];
            total++;

            double halfDev = Math.Abs(interval - nominalHalf) / nominalHalf;
            double fullDev = Math.Abs(interval - nominalFull) / nominalFull;

            if (halfDev < tolerance || fullDev < tolerance)
                nearPeak++;
        }

        return total > 0 ? (double)nearPeak / total : 0;
    }
}
