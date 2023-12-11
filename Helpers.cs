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
            var valHex = valToParse.ToString("X8");
            var hexVal1Binary = Convert.ToString(Convert.ToUInt32(valHex[0] + "" + valHex[1], 16), 2).PadLeft(8, '0');
            var hexVal2Binary = Convert.ToString(Convert.ToUInt32(valHex[2] + "" + valHex[3], 16), 2).PadLeft(8, '0');
            var hexVal3Binary = Convert.ToString(Convert.ToUInt32(valHex[4] + "" + valHex[5], 16), 2).PadLeft(8, '0');
            var hexVal4Binary = Convert.ToString(Convert.ToUInt32(valHex[6] + "" + valHex[7], 16), 2).PadLeft(8, '0');

            return hexVal1Binary + "" + hexVal2Binary + "" + hexVal3Binary + "" + hexVal4Binary;
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