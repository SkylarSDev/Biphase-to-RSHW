using System;
using System.Collections.Generic;

namespace BiphaseDecoder;

public enum BiphaseVariant
{
    BMC,
    BSC
}

public enum BitOrder
{
    MsbFirst,
    LsbFirst
}

public class DecoderResult
{
    public List<bool> Bits { get; set; } = new();
    public int InvalidIntervals { get; set; }
    public int TotalIntervals { get; set; }
}

public static class BiphaseMarkDecoder
{
    private const double NominalBitRate = 4500.0;
    private const double ToleranceFraction = 0.35;

    public static DecoderResult Decode(float[] samples, int sampleRate, BiphaseVariant variant,
        float hysteresisThreshold = 0.01f)
    {
        var crossings = DetectZeroCrossings(samples, sampleRate, hysteresisThreshold);
        return ClassifyAndExtract(crossings, sampleRate, variant);
    }

    public static List<double> DetectZeroCrossings(float[] samples, int sampleRate,
        float hysteresisThreshold = 0.01f)
    {
        var crossings = new List<double>();
        bool positive = samples.Length > 0 && samples[0] >= 0;
        bool armed = true;

        for (int i = 1; i < samples.Length; i++)
        {
            float prev = samples[i - 1];
            float curr = samples[i];

            if (positive && curr <= -hysteresisThreshold)
                armed = true;
            else if (!positive && curr >= hysteresisThreshold)
                armed = true;

            if (!armed) continue;

            bool crossed = (positive && curr < 0) || (!positive && curr >= 0);
            if (crossed)
            {
                double fraction = Math.Abs(prev) / (Math.Abs(prev) + Math.Abs(curr));
                double crossingTime = (i - 1 + fraction) / sampleRate;
                crossings.Add(crossingTime);
                positive = !positive;
                armed = false;
            }
        }

        return crossings;
    }

    private static DecoderResult ClassifyAndExtract(List<double> crossings, int sampleRate,
        BiphaseVariant variant)
    {
        var result = new DecoderResult();
        if (crossings.Count < 2) return result;

        double nominalHalf = 1.0 / (NominalBitRate * 2);
        double nominalFull = 1.0 / NominalBitRate;

        double avgHalf = nominalHalf;
        double avgFull = nominalFull;
        double alpha = 0.02;

        bool expectingSecondHalf = false;

        for (int i = 1; i < crossings.Count; i++)
        {
            double interval = crossings[i] - crossings[i - 1];
            result.TotalIntervals++;

            double halfMin = avgHalf * (1 - ToleranceFraction);
            double halfMax = avgHalf * (1 + ToleranceFraction);
            double fullMin = avgFull * (1 - ToleranceFraction);
            double fullMax = avgFull * (1 + ToleranceFraction);

            bool isHalf = interval >= halfMin && interval <= halfMax;
            bool isFull = interval >= fullMin && interval <= fullMax;

            if (isHalf)
            {
                avgHalf = avgHalf * (1 - alpha) + interval * alpha;
                avgFull = avgHalf * 2;

                if (expectingSecondHalf)
                {
                    bool bit = variant == BiphaseVariant.BMC;
                    result.Bits.Add(bit);
                    expectingSecondHalf = false;
                }
                else
                {
                    expectingSecondHalf = true;
                }
            }
            else if (isFull)
            {
                if (expectingSecondHalf)
                {
                    result.InvalidIntervals++;
                    expectingSecondHalf = false;
                }

                avgFull = avgFull * (1 - alpha) + interval * alpha;
                avgHalf = avgFull / 2;

                bool bit = variant != BiphaseVariant.BMC;
                result.Bits.Add(bit);
            }
            else
            {
                result.InvalidIntervals++;
                expectingSecondHalf = false;
            }
        }

        return result;
    }
}
