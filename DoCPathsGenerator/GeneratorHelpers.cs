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
    }
}