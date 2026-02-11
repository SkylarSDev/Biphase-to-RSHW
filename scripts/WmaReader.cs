using System;
using System.Collections.Generic;
using NAudio.Wave;

namespace BiphaseDecoder;

public class AudioData
{
    public int SampleRate { get; set; }
    public int ChannelCount { get; set; }
    public float[][] Channels { get; set; } = Array.Empty<float[]>();
}

public static class WmaReader
{
    public static AudioData Read(string path)
    {
        using var reader = new MediaFoundationReader(path);
        var provider = reader.ToSampleProvider();

        int sampleRate = provider.WaveFormat.SampleRate;
        int channels = provider.WaveFormat.Channels;

        var allSamples = new List<float>();
        var buffer = new float[sampleRate * channels];
        int read;
        while ((read = provider.Read(buffer, 0, buffer.Length)) > 0)
        {
            for (int i = 0; i < read; i++)
                allSamples.Add(buffer[i]);
        }

        int totalSamples = allSamples.Count;
        int samplesPerChannel = totalSamples / channels;

        var channelData = new float[channels][];
        for (int ch = 0; ch < channels; ch++)
        {
            channelData[ch] = new float[samplesPerChannel];
            for (int i = 0; i < samplesPerChannel; i++)
                channelData[ch][i] = allSamples[i * channels + ch];
        }

        return new AudioData
        {
            SampleRate = sampleRate,
            ChannelCount = channels,
            Channels = channelData
        };
    }
}
