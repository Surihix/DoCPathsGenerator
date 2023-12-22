using System;
using System.IO;
using System.Linq;

namespace DoCPathsGenerator
{
    internal class Core
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Enough arguments not specified\n");
                Console.WriteLine("Example:");
                Console.WriteLine("DoCPathsGenerator.exe [unpacked filelist folder] [unpacked _KEL.DAT folder]");
                Console.ReadLine();
                Environment.Exit(0);
            }

            var unpackedFilelistDir = args[0];
            var unpackedKELdir = args[1];
            var generatedPathsDir = Path.Combine(Path.GetDirectoryName(unpackedKELdir), "#generatedPaths");

            if (!File.Exists(Path.Combine(unpackedFilelistDir, "~Counts.txt")))
            {
                ErrorExit("Missing '~Counts.txt' file in the unpacked filelist directory");
            }

            if (new FileInfo(Path.Combine(unpackedFilelistDir, "~Counts.txt")).Length == 0)
            {
                ErrorExit("'~Counts.txt' file does not contain any data");
            }

            if (!Directory.Exists(unpackedFilelistDir))
            {
                ErrorExit("Specified filelist directory is missing");
            }

            if (!Directory.Exists(unpackedKELdir))
            {
                ErrorExit("Specified '_KEL.DAT' directory is missing");
            }

            try
            {
                var linesCount = File.ReadAllLines(Path.Combine(unpackedFilelistDir, "~Counts.txt")).Count();

                if (linesCount < 2)
                {
                    ErrorExit("~Counts.txt file does not contain two lines");
                }

                var isCountsValid = uint.TryParse(File.ReadAllLines(Path.Combine(unpackedFilelistDir, "~Counts.txt"))[1], out uint chunksCount);

                if (!isCountsValid)
                {
                    ErrorExit("~Counts.txt file does not contain a valid chunk count value");
                }

                PathGenerator.GeneratePaths(chunksCount, generatedPathsDir, unpackedFilelistDir, unpackedKELdir);
            }
            catch (Exception ex)
            {
                ErrorExit($"An exception has occured\n{ex}");
            }

            Console.WriteLine("");
            Console.WriteLine("Finished generating paths");
            Console.ReadLine();
        }

        static void ErrorExit(string errorMsg)
        {
            Console.WriteLine("");
            Console.WriteLine($"Error: {errorMsg}");
            Console.ReadLine();
            Environment.Exit(0);
        }
    }
}