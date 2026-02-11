using System;
using System.Collections.Generic;

namespace BiphaseDecoder;

public class FrameData
{
    public byte[] Data { get; set; } = Array.Empty<byte>();
    public int BitOffset { get; set; }
    public bool IsGapFrame { get; set; }
}

public class SyncResult
{
    public List<FrameData> Frames { get; set; } = new();
    public int SyncHits { get; set; }
    public int SyncLosses { get; set; }
    public int TotalBits { get; set; }
}

public static class FrameSynchronizer
{
    private const byte SyncByte = 0xFD;
    private const int FrameBits = 128;
    private const int DataBits = 120;
    private const int DataBytes = 15;
    private const int MaxSearchBits = 256;

    public static SyncResult ExtractFrames(List<bool> bits, BitOrder bitOrder)
    {
        var result = new SyncResult { TotalBits = bits.Count };
        if (bits.Count < FrameBits) return result;

        int pos = FindSyncByte(bits, 0, bits.Count, bitOrder);
        if (pos < 0) return result;

        while (pos + FrameBits <= bits.Count)
        {
            byte syncByte = ExtractByte(bits, pos, bitOrder);

            if (syncByte == SyncByte)
            {
                result.SyncHits++;

                int dataStart = pos + 8;
                if (dataStart + DataBits > bits.Count) break;

                var frameData = new byte[DataBytes];
                for (int i = 0; i < DataBytes; i++)
                {
                    frameData[i] = ExtractByte(bits, dataStart + i * 8, bitOrder);
                }

                result.Frames.Add(new FrameData
                {
                    Data = frameData,
                    BitOffset = pos,
                    IsGapFrame = false
                });

                pos += FrameBits;
            }
            else
            {
                result.SyncLosses++;
                int searchEnd = Math.Min(pos + MaxSearchBits, bits.Count - FrameBits);
                int newPos = FindSyncByte(bits, pos + 1, searchEnd, bitOrder);

                if (newPos < 0)
                {
                    pos += FrameBits;
                    continue;
                }

                int gapBits = newPos - pos;
                int gapFrames = gapBits / FrameBits;
                for (int g = 0; g < gapFrames; g++)
                {
                    result.Frames.Add(new FrameData
                    {
                        Data = new byte[DataBytes],
                        BitOffset = pos + g * FrameBits,
                        IsGapFrame = true
                    });
                }

                pos = newPos;
            }
        }

        return result;
    }

    public static int CountSyncBytes(List<bool> bits, BitOrder bitOrder)
    {
        if (bits.Count < FrameBits) return 0;

        int bestCount = 0;

        int searchLimit = Math.Min(FrameBits, bits.Count - 8);
        for (int startOffset = 0; startOffset < searchLimit; startOffset++)
        {
            byte b = ExtractByte(bits, startOffset, bitOrder);
            if (b != SyncByte) continue;

            int count = 0;
            int pos = startOffset;
            int misses = 0;
            while (pos + 8 <= bits.Count && misses < 3)
            {
                b = ExtractByte(bits, pos, bitOrder);
                if (b == SyncByte)
                {
                    count++;
                    misses = 0;
                }
                else
                {
                    misses++;
                }
                pos += FrameBits;
            }

            if (count > bestCount)
                bestCount = count;
        }

        return bestCount;
    }

    private static int FindSyncByte(List<bool> bits, int start, int end, BitOrder bitOrder)
    {
        for (int i = start; i + 8 <= end && i + FrameBits <= bits.Count; i++)
        {
            if (ExtractByte(bits, i, bitOrder) == SyncByte)
            {
                if (i + FrameBits + 8 <= bits.Count)
                {
                    if (ExtractByte(bits, i + FrameBits, bitOrder) == SyncByte)
                        return i;
                }
                else
                {
                    return i;
                }
            }
        }
        return -1;
    }

    private static byte ExtractByte(List<bool> bits, int offset, BitOrder bitOrder)
    {
        if (offset + 8 > bits.Count) return 0;

        byte val = 0;
        for (int i = 0; i < 8; i++)
        {
            if (bits[offset + i])
            {
                if (bitOrder == BitOrder.MsbFirst)
                    val |= (byte)(0x80 >> i);
                else
                    val |= (byte)(1 << i);
            }
        }
        return val;
    }
}
