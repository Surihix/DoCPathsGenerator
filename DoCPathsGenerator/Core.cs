using System;
using System.IO;

namespace DoCPathsGenerator
{
    internal class Core
    {
        static void Main(string[] args)
        {
            Console.WriteLine("");

            if (args.Length < 3)
            {
                Console.WriteLine("Enough arguments not specified\n");
                Console.WriteLine("Examples:");
                Console.WriteLine("DoCPathsGenerator.exe -g [FILELIST.BIN.json filePath] [unpacked _KEL.DAT folderPath]");
                Console.WriteLine("DoCPathsGenerator.exe -gm [FILELIST.BIN.json filePath] [unpacked _KEL.DAT folderPath]");
                Console.WriteLine("DoCPathsGenerator.exe -c [unpacked _KEL.DAT folderPath] [#generatedPaths folder path]");
                Console.ReadLine();
                Environment.Exit(0);
            }

            if (Enum.TryParse(args[0].Replace("-", ""), false, out ActionSwitches actionSwitch) == false)
            {
                Console.WriteLine("Warning: Specified tool action was invalid");
                Console.ReadLine();
                Environment.Exit(0);
            }

            string unpackedKELdir;

            switch (actionSwitch)
            {
                case ActionSwitches.c:
                    unpackedKELdir = args[1];
                    var generatedPathsDir = args[2];

                    if (!Directory.Exists(unpackedKELdir))
                    {
                        GeneratorHelpers.ErrorExit("Specified '_KEL.DAT' folder is missing");
                    }

                    if (!Directory.Exists(generatedPathsDir))
                    {
                        GeneratorHelpers.ErrorExit("Specified '_KEL.DAT' folder is missing");
                    }

                    PathsChecker.CheckAvailablePaths(unpackedKELdir, generatedPathsDir);

                    Console.WriteLine("");
                    Console.WriteLine("");
                    Console.WriteLine("Finished checking paths");
                    Console.ReadLine();
                    break;

                case ActionSwitches.g:
                case ActionSwitches.gm:
                    var jsonFile = args[1];
                    unpackedKELdir = args[2];

                    if (!File.Exists(jsonFile))
                    {
                        GeneratorHelpers.ErrorExit("Specified 'FILELIST.BIN.json' file is missing");
                    }

                    if (!Directory.Exists(unpackedKELdir))
                    {
                        GeneratorHelpers.ErrorExit("Specified '_KEL.DAT' folder is missing");
                    }

                    var shouldMove = actionSwitch == ActionSwitches.gm;
                    PathsGenerator.GeneratePaths(shouldMove, jsonFile, unpackedKELdir);

                    Console.WriteLine("");
                    Console.WriteLine("");
                    Console.WriteLine("Finished generating paths");
                    Console.ReadLine();
                    break;
            }
        }


        enum ActionSwitches
        {
            c,
            g,
            gm
        }
    }
}