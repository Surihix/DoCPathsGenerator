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
            var val1Binary = Convert.ToString(valArray[3], 2).PadLeft(8, '0');
            var val2Binary = Convert.ToString(valArray[2], 2).PadLeft(8,'0');
            var val3Binary = Convert.ToString(valArray[1], 2).PadLeft(8,'0');
            var val4Binary = Convert.ToString(valArray[0], 2).PadLeft(8,'0');

            return val1Binary + "" + val2Binary + "" + val3Binary + "" + val4Binary;
        }

        public static uint BinaryToUInt(string binaryVal, int startPosition, int count)
        {
            var binaryValSelected = binaryVal.Substring(startPosition, count);

            return Convert.ToUInt32(binaryValSelected, 2);
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