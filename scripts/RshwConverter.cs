using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BiphaseDecoder;

public class ConversionResult
{
    public int FrameCount { get; set; }
    public int DataChannelCount { get; set; }
    public int AudioChannelCount { get; set; }
    public BiphaseVariant Variant { get; set; }
    public BitOrder BitOrder { get; set; }
    public double SyncHitRate { get; set; }
    public int SyncHits { get; set; }
    public int SyncLosses { get; set; }
    public long OutputFileSize { get; set; }
    public double DurationSeconds { get; set; }
}

public static class RshwConverter
{
    private const double BiphaseFps = 4500.0 / 128.0;
    private const double OutputFps = 60.0;
    private const int BitsPerFrame = 120;
    private const int DrawerSize = 150;

    private static readonly int[] ByteDescramble = { 10, 11, 0, 1, 2, 3, 4, 12, 13, 5, 6, 7, 8, 9, 14 };

    public static ConversionResult Convert(string inputPath, string outputPath, bool verbose = false)
    {
        var result = new ConversionResult();

        if (verbose) Console.WriteLine("Reading audio file...");
        var audio = WmaReader.Read(inputPath);
        result.DurationSeconds = (double)audio.Channels[0].Length / audio.SampleRate;
        if (verbose)
        {
            Console.WriteLine($"  Sample rate: {audio.SampleRate} Hz");
            Console.WriteLine($"  Channels: {audio.ChannelCount}");
            Console.WriteLine($"  Duration: {result.DurationSeconds:F2}s");
            Console.WriteLine($"  Samples per channel: {audio.Channels[0].Length}");
        }

        if (verbose) Console.WriteLine("\nAnalyzing channels...");
        var analysis = ChannelAnalyzer.Analyze(audio, verbose);
        result.DataChannelCount = analysis.DataChannelIndices.Count;
        result.AudioChannelCount = analysis.AudioChannelIndices.Count;
        result.Variant = analysis.DetectedVariant;
        result.BitOrder = analysis.DetectedBitOrder;

        if (verbose)
        {
            Console.WriteLine($"\nDetected variant: {analysis.DetectedVariant}");
            Console.WriteLine($"Detected bit order: {analysis.DetectedBitOrder}");
            Console.WriteLine($"Data channels: [{string.Join(", ", analysis.DataChannelIndices)}]");
            Console.WriteLine($"Audio channels: [{string.Join(", ", analysis.AudioChannelIndices)}]");

            foreach (var ch in analysis.Channels)
            {
                Console.WriteLine($"  Channel {ch.ChannelIndex}: {ch.Type} " +
                    $"(ZCR={ch.ZeroCrossingRate:F0}/s, sharpness={ch.IntervalSharpness:F3}, " +
                    $"syncs={ch.SyncCount})");
            }
        }

        if (verbose) Console.WriteLine("\nDecoding data channels...");
        var allFrames = new List<List<FrameData>>();
        int totalSyncHits = 0;
        int totalSyncLosses = 0;

        foreach (int chIdx in analysis.DataChannelIndices)
        {
            if (verbose) Console.WriteLine($"  Decoding channel {chIdx}...");
            var decoded = BiphaseMarkDecoder.Decode(
                audio.Channels[chIdx], audio.SampleRate, analysis.DetectedVariant, 0.01f);

            if (verbose)
            {
                Console.WriteLine($"    Bits decoded: {decoded.Bits.Count}");
                Console.WriteLine($"    Invalid intervals: {decoded.InvalidIntervals}/{decoded.TotalIntervals}");
            }

            var syncResult = FrameSynchronizer.ExtractFrames(decoded.Bits, analysis.DetectedBitOrder);
            allFrames.Add(syncResult.Frames);
            totalSyncHits += syncResult.SyncHits;
            totalSyncLosses += syncResult.SyncLosses;

            if (verbose)
            {
                Console.WriteLine($"    Frames extracted: {syncResult.Frames.Count}");
                Console.WriteLine($"    Sync hits: {syncResult.SyncHits}, losses: {syncResult.SyncLosses}");
            }
        }

        result.SyncHits = totalSyncHits;
        result.SyncLosses = totalSyncLosses;
        result.SyncHitRate = totalSyncHits + totalSyncLosses > 0
            ? (double)totalSyncHits / (totalSyncHits + totalSyncLosses)
            : 0;

        if (verbose) Console.WriteLine("\nPacking signal data...");
        int[] signalData = PackSignalData(allFrames, result.DurationSeconds, analysis.DetectedBitOrder, verbose);
        result.FrameCount = allFrames.Count > 0 ? allFrames[0].Count : 0;

        byte[]? audioData = null;
        if (analysis.AudioChannelIndices.Count > 0)
        {
            if (verbose) Console.WriteLine("Converting audio to WAV...");
            audioData = ConvertAudioToWav(audio, analysis.AudioChannelIndices);
            if (verbose)
                Console.WriteLine($"  WAV size: {audioData.Length:N0} bytes");
        }

        if (verbose) Console.WriteLine("\nSaving RSHW file...");
        var rshw = new rshwFormat
        {
            signalData = signalData,
            audioData = audioData,
            videoData = null
        };

        rshwFormat.Save(outputPath, rshw);
        result.OutputFileSize = new FileInfo(outputPath).Length;

        return result;
    }

    private static int[] PackSignalData(List<List<FrameData>> allChannelFrames, double durationSeconds, BitOrder bitOrder, bool verbose)
    {
        if (allChannelFrames.Count == 0) return new int[] { 0 };

        int numChannels = allChannelFrames.Count;
        int maxInputFrames = 0;
        foreach (var frames in allChannelFrames)
            if (frames.Count > maxInputFrames) maxInputFrames = frames.Count;

        int outputFrameCount = (int)(durationSeconds * OutputFps) + 1;

        if (verbose)
        {
            Console.WriteLine($"  Input frames: {maxInputFrames} at {BiphaseFps:F2} fps");
            Console.WriteLine($"  Output frames: {outputFrameCount} at {OutputFps} fps");
            Console.WriteLine($"  Data channels: {numChannels}");
        }

        var signalData = new List<int>();

        for (int outFrame = 0; outFrame < outputFrameCount; outFrame++)
        {
            signalData.Add(0);

            double t = outFrame / OutputFps;
            int inputFrame = (int)(t * BiphaseFps);

            for (int ch = 0; ch < numChannels; ch++)
            {
                byte[] data;
                if (inputFrame < allChannelFrames[ch].Count)
                    data = allChannelFrames[ch][inputFrame].Data;
                else
                    continue;

                int drawerOffset = ch * DrawerSize;

                byte[] descrambled = new byte[16];
                for (int i = 0; i < data.Length; i++)
                    descrambled[ByteDescramble[i]] = data[i];

                for (int bit = 0; bit < BitsPerFrame; bit++)
                {
                    int byteIndex = bit / 8;
                    int bitIndex = bit % 8;
                    int mask = (bitOrder == BitOrder.MsbFirst) ? (0x80 >> bitIndex) : (1 << bitIndex);
                    bool bitOn = (descrambled[byteIndex] & mask) != 0;

                    if (bitOn)
                    {
                        signalData.Add(drawerOffset + bit + 1);
                    }
                }
            }
        }

        if (verbose)
        {
            int frameCount = 0;
            int maxBitPos = 0;
            foreach (int v in signalData)
            {
                if (v == 0) frameCount++;
                if (v > maxBitPos) maxBitPos = v;
            }
            Console.WriteLine($"  Signal data length: {signalData.Count} ints");
            Console.WriteLine($"  Frame delimiters: {frameCount}");
            Console.WriteLine($"  Max bit position: {maxBitPos}");
        }

        return signalData.ToArray();
    }

    private static byte[] ConvertAudioToWav(AudioData audio, List<int> audioChannelIndices)
    {
        int numChannels = audioChannelIndices.Count;
        int sampleRate = audio.SampleRate;
        int samplesPerChannel = audio.Channels[audioChannelIndices[0]].Length;
        int bitsPerSample = 16;
        int blockAlign = numChannels * (bitsPerSample / 8);
        int byteRate = sampleRate * blockAlign;
        int dataSize = samplesPerChannel * blockAlign;

        using var ms = new MemoryStream(44 + dataSize);
        using var writer = new BinaryWriter(ms);

        writer.Write(Encoding.ASCII.GetBytes("RIFF"));
        writer.Write(36 + dataSize);
        writer.Write(Encoding.ASCII.GetBytes("WAVE"));

        writer.Write(Encoding.ASCII.GetBytes("fmt "));
        writer.Write(16);
        writer.Write((short)1);
        writer.Write((short)numChannels);
        writer.Write(sampleRate);
        writer.Write(byteRate);
        writer.Write((short)blockAlign);
        writer.Write((short)bitsPerSample);

        writer.Write(Encoding.ASCII.GetBytes("data"));
        writer.Write(dataSize);

        for (int s = 0; s < samplesPerChannel; s++)
        {
            for (int c = 0; c < numChannels; c++)
            {
                float sample = audio.Channels[audioChannelIndices[c]][s];
                sample = Math.Max(-1f, Math.Min(1f, sample));
                short pcmSample = (short)(sample * 32767f);
                writer.Write(pcmSample);
            }
        }

        return ms.ToArray();
    }
}
