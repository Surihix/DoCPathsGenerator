using System;
using System.Collections.Generic;
using System.IO;

namespace DoCPathsGenerator
{
    internal class GeneratorHelpers
    {
        public static void ErrorExit(string errorMsg)
        {
            Console.WriteLine("");
            Console.WriteLine($"Error: {errorMsg}");
            Console.ReadLine();
            Environment.Exit(1);
        }


        public static void ProcessGeneratedPath(bool moveFiles, string virtualPath, string noPathFile, Dictionary<string, List<(uint, string, string)>> generatedPathsDict, string currentChunk, uint fileCode)
        {
            var dirInGenPathsDir = Path.Combine(PathsGenerator.GeneratedPathsDir, Path.GetDirectoryName(virtualPath));
            if (!Directory.Exists(dirInGenPathsDir))
            {
                Directory.CreateDirectory(dirInGenPathsDir);
            }

            if (moveFiles)
            {
                File.Move(noPathFile, Path.Combine(PathsGenerator.GeneratedPathsDir, virtualPath));
            }
            else
            {
                File.Copy(noPathFile, Path.Combine(PathsGenerator.GeneratedPathsDir, virtualPath));
            }

            if (generatedPathsDict.ContainsKey(currentChunk))
            {
                generatedPathsDict[currentChunk].Add((fileCode, Path.GetFileName(noPathFile), virtualPath));
            }
            else
            {
                generatedPathsDict.Add(currentChunk, new List<(uint, string, string)>());
                generatedPathsDict[currentChunk].Add((fileCode, Path.GetFileName(noPathFile), virtualPath));
            }

            Console.WriteLine($"Generated: {virtualPath.Replace("\\", "/")}");
        }


        public static string ComputeFNameLanguage(uint index, string fNamePattern)
        {
            var generatedFName = "";
            switch (index)
            {
                case 0:
                    generatedFName = fNamePattern + "_jp.bin";
                    break;

                case 1:
                    generatedFName = fNamePattern + "_us.bin";
                    break;

                case 2:
                    generatedFName = fNamePattern + "_uk.bin";
                    break;

                case 3:
                    generatedFName = fNamePattern + "_it.bin";
                    break;

                case 4:
                    generatedFName = fNamePattern + "_sp.bin";
                    break;

                case 5:
                    generatedFName = fNamePattern + "_fr.bin";
                    break;

                case 6:
                    generatedFName = fNamePattern + "_gr.bin";
                    break;
            }

            return generatedFName;
        }


        public static string ComputeFNameNum(uint index, string fNamePattern, string fExtension)
        {
            var generatedFName = "";
            switch (index)
            {
                case 0:
                    if (fExtension == "class")
                    {
                        generatedFName = fNamePattern + $"00.{fExtension}";
                    }
                    if (fExtension == "bin")
                    {
                        generatedFName = fNamePattern + $".{fExtension}";
                    }
                    break;

                case 1:
                    generatedFName = fNamePattern + $"01.{fExtension}";
                    break;

                case 2:
                    generatedFName = fNamePattern + $"02.{fExtension}";
                    break;

                case 3:
                    generatedFName = fNamePattern + $"03.{fExtension}";
                    break;

                case 4:
                    generatedFName = fNamePattern + $"04.{fExtension}";
                    break;

                case 5:
                    generatedFName = fNamePattern + $"05.{fExtension}";
                    break;

                case 6:
                    generatedFName = fNamePattern + $"06.{fExtension}";
                    break;

                case 7:
                    generatedFName = fNamePattern + $"07.{fExtension}";
                    break;
            }

            return generatedFName;
        }


        public static string GenerateFolderNameWithNumber(string fNamePattern, uint number, int padCount)
        {
            return fNamePattern + number.ToString().PadLeft(padCount, '0');
        }


        public static string GenerateFNameWithNumber(string fNamePattern, uint number, int padCount, string fExtn)
        {
            return fNamePattern + number.ToString().PadLeft(padCount, '0') + fExtn;
        }
    }
}