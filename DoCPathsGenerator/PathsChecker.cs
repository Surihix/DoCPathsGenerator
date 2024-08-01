using System;
using System.IO;
using System.Threading;

namespace DoCPathsGenerator
{
    internal class PathsChecker
    {
        public static void CheckAvailablePaths(string unpackedKELdir, string generatedPathsDir)
        {
            Console.WriteLine("Checking....");
            Console.WriteLine("");
            Thread.Sleep(200);

            var filesTxtFile = Path.Combine(unpackedKELdir, "file.txt");
            if (!File.Exists(filesTxtFile))
            {
                GeneratorHelpers.ErrorExit("Missing 'file.txt' file in the unpacked KEL directory");
            }

            var missingFilesTxt = Path.Combine(generatedPathsDir, "missing_list.txt");
            var missingCounter = 0;

            if (File.Exists(missingFilesTxt))
            {
                File.Delete(missingFilesTxt);
            }

            using (var filesTxtReader = new StreamReader(filesTxtFile))
            {
                using (var missingFilesTxtWriter = new StreamWriter(missingFilesTxt, true))
                {
                    string currentLine;
                    string[] currentLineData;
                    string currentFile;
                    string currentFileGen;
                    while ((currentLine = filesTxtReader.ReadLine()) != null)
                    {
                        currentLineData = currentLine.Split(':');

                        if (currentLineData.Length == 3)
                        {
                            if (currentLineData[2] != "")
                            {
                                currentFile = Path.Combine(unpackedKELdir, currentLineData[2]);
                                currentFileGen = Path.Combine(generatedPathsDir, currentLineData[2]);

                                if (!File.Exists(currentFile))
                                {
                                    if (!File.Exists(currentFileGen))
                                    {
                                        Console.WriteLine($"Missing: {currentLineData[2]}");
                                        missingFilesTxtWriter.WriteLine(currentLineData[2]);
                                        missingCounter++;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Console.WriteLine("");
            Console.WriteLine($"Files Missing: {missingCounter}");
        }
    }
}