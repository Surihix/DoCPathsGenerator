using System;
using System.IO;

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
                Console.WriteLine("DoCPathsGenerator.exe [JSON file] [unpacked _KEL.DAT folder]");
                Console.ReadLine();
                Environment.Exit(0);
            }

            var jsonFile = args[0];
            var unpackedKELdir = args[1];

            if (!File.Exists(jsonFile))
            {
                GeneratorHelpers.ErrorExit("Specified JSON file is missing");
            }

            if (!Directory.Exists(unpackedKELdir))
            {
                GeneratorHelpers.ErrorExit("Specified '_KEL.DAT' directory is missing");
            }

            var generatedPathsDir = Path.Combine(Path.GetDirectoryName(unpackedKELdir), "#generatedPaths");

            PathsGenerator.GeneratePaths(jsonFile, generatedPathsDir, unpackedKELdir);

            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("Finished generating paths");
            Console.ReadLine();
        }
    }
}