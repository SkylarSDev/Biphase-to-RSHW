using System;
using System.IO;

namespace BiphaseDecoder;

class Program
{
    static int Main(string[] args)
    {
        if (args.Length < 1)
        {
            Console.WriteLine("Usage: BiphaseDecoder <input.wma> [output.rshw] [--verbose]");
            Console.WriteLine();
            Console.WriteLine("Decodes biphase-encoded data from WMA audio and outputs an .rshw file");
            Console.WriteLine("compatible with Rock-afire Replay.");
            return 1;
        }

        string inputPath = args[0];
        bool verbose = Array.Exists(args, a => a == "--verbose" || a == "-v");

        string outputPath;
        if (args.Length >= 2 && !args[1].StartsWith("-"))
            outputPath = args[1];
        else
            outputPath = Path.ChangeExtension(inputPath, ".rshw");

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Error: Input file not found: {inputPath}");
            return 1;
        }

        Console.WriteLine($"Input:  {inputPath}");
        Console.WriteLine($"Output: {outputPath}");
        Console.WriteLine();

        try
        {
            var result = RshwConverter.Convert(inputPath, outputPath, verbose);

            Console.WriteLine();
            Console.WriteLine("=== Conversion Summary ===");
            Console.WriteLine($"Duration:        {result.DurationSeconds:F2}s");
            Console.WriteLine($"Data channels:   {result.DataChannelCount}");
            Console.WriteLine($"Audio channels:  {result.AudioChannelCount}");
            Console.WriteLine($"Variant:         {result.Variant}");
            Console.WriteLine($"Bit order:       {result.BitOrder}");
            Console.WriteLine($"Frames decoded:  {result.FrameCount}");
            Console.WriteLine($"Sync hits:       {result.SyncHits}");
            Console.WriteLine($"Sync losses:     {result.SyncLosses}");
            Console.WriteLine($"Sync hit rate:   {result.SyncHitRate:P1}");
            Console.WriteLine($"Output size:     {result.OutputFileSize:N0} bytes");
            Console.WriteLine();
            Console.WriteLine("Done.");
            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
            if (verbose)
                Console.Error.WriteLine(ex.StackTrace);
            return 1;
        }
    }
}
