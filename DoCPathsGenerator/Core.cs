using DoCPathsGenerator.Support;
using System;
using System.IO;

namespace DoCPathsGenerator
{
    internal class Core
    {
        static void Main(string[] args)
        {
            Console.WriteLine("");

            try
            {
                if (args.Length > 0)
                {
                    if (args[0] == "-?" || args[0] == "-h")
                    {
                        Help.ShowCommands();
                    }
                }

                if (args.Length < 3)
                {
                    Console.WriteLine("Warning: Enough arguments not specified. Please use -? or -h switches for more information.");
                    Console.WriteLine("");
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
                    case ActionSwitches.gc:
                    case ActionSwitches.gm:
                        var filelistFile = args[1];
                        unpackedKELdir = args[2];

                        if (!File.Exists(filelistFile))
                        {
                            SharedMethods.ErrorExit("Specified 'FILELIST.BIN' file is missing");
                        }

                        if (!Directory.Exists(unpackedKELdir))
                        {
                            SharedMethods.ErrorExit("Specified '_KEL.DAT' unpacked folder is missing");
                        }

                        var shouldMove = actionSwitch == ActionSwitches.gm;
                        PathsGenerator.GeneratePaths(shouldMove, filelistFile, unpackedKELdir);

                        Console.WriteLine("");
                        Console.WriteLine("");
                        Console.WriteLine("Finished generating paths");
                        Console.ReadLine();
                        break;


                    case ActionSwitches.c:
                        unpackedKELdir = args[1];
                        var generatedPathsDir = args[2];

                        if (!Directory.Exists(unpackedKELdir))
                        {
                            SharedMethods.ErrorExit("Specified '_KEL.DAT' unpacked folder is missing");
                        }

                        if (!Directory.Exists(generatedPathsDir))
                        {
                            SharedMethods.ErrorExit("Specified '#generatedPaths' folder is missing");
                        }

                        PathsChecker.CheckAvailablePaths(unpackedKELdir, generatedPathsDir);

                        Console.WriteLine("");
                        Console.WriteLine("");
                        Console.WriteLine("Finished checking paths");
                        Console.ReadLine();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("");
                Console.WriteLine("An exception has occured");
                Console.WriteLine("");
                Console.WriteLine(ex);
                Console.ReadLine();
                Environment.Exit(2);
            }
        }


        enum ActionSwitches
        {
            c,
            gc,
            gm
        }
    }
}