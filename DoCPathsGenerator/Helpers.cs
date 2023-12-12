using System;

namespace DoCPathsGenerator
{
    internal class Helpers
    {
        public static void ErrorExit(string errorMsg)
        {
            Console.WriteLine("");
            Console.WriteLine($"Error: {errorMsg}");
            Console.ReadLine();
            Environment.Exit(0);
        }

        public static string GetBaseBinaryValue(uint valToParse)
        {
            var valArray = BitConverter.GetBytes(valToParse);

            return Convert.ToString(valArray[3], 2).PadLeft(8, '0') + "" + Convert.ToString(valArray[2], 2).PadLeft(8, '0') +
                "" + Convert.ToString(valArray[1], 2).PadLeft(8, '0') + "" + Convert.ToString(valArray[0], 2).PadLeft(8, '0');
        }

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
    }
}