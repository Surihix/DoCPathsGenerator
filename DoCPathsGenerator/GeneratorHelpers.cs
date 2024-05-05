using System;
using System.IO;

namespace DoCPathsGenerator
{
    internal class GeneratorHelpers
    {
        public static uint BinaryToUInt(string binaryVal, int startPosition, int count)
        {
            return Convert.ToUInt32(binaryVal.Substring(startPosition, count), 2);
        }


        public static string AppendZeroes(string type, uint folderNumber)
        {
            var appendedStr = "";

            switch (type)
            {
                case "event":
                    if (folderNumber < 10)
                    {
                        appendedStr = "000";
                    }

                    if (folderNumber >= 10 && folderNumber < 100)
                    {
                        appendedStr = "00";
                    }

                    if (folderNumber >= 100 && folderNumber < 1000)
                    {
                        appendedStr = "0";
                    }
                    break;

                case "zone":
                    if (folderNumber < 10)
                    {
                        appendedStr = "00";
                    }

                    if (folderNumber >= 10 && folderNumber < 100)
                    {
                        appendedStr = "0";
                    }
                    break;
            }

            return appendedStr;
        }


        public static void CopyFileToPath(string generatedVPath, string generatedPathsDir, string currentFPath)
        {
            Console.WriteLine(generatedVPath);

            if (!Directory.Exists(Path.GetDirectoryName(Path.Combine(generatedPathsDir, generatedVPath))))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(generatedPathsDir, generatedVPath)));
            }

            File.Copy(currentFPath, Path.Combine(generatedPathsDir, generatedVPath));
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


        public static void WriteMapping(string generatedPathsDir, string generatedVPath, string generatedFName)
        {
            var mappingFile = Path.Combine(generatedPathsDir, Path.GetDirectoryName(generatedVPath), "#Names.txt");
            if (!File.Exists(mappingFile))
            {
                File.WriteAllText(mappingFile, $"{Path.GetFileName(generatedVPath)} = {generatedFName}");
            }
            else
            {
                var mappingData = File.ReadAllText(mappingFile);
                mappingData += $"\n{Path.GetFileName(generatedVPath)} = {generatedFName}";
                File.WriteAllText(mappingFile, mappingData);
            }
        }
    }
}