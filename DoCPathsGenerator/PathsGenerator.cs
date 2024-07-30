using DoCPathsGenerator.Dirs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace DoCPathsGenerator
{
    internal class PathsGenerator
    {
        public static string GeneratedPathsDirSet { get; set; }
        public static string LastKey { get; set; }

        public static uint PathsGenerated = 0;

        public static void GeneratePaths(string jsonFile, string generatedPathsDir, string unpackedKELdir)
        {
            Console.WriteLine("");

            GeneratedPathsDirSet = generatedPathsDir;

            using (var jsonReader = new StreamReader(jsonFile))
            {
                object tmpValueRead = string.Empty;

                _ = jsonReader.ReadLine();
                _ = jsonReader.ReadLine();

                // Get fileCount and chunkCount values
                tmpValueRead = GeneratorHelpers.CheckParseJsonProperty(jsonReader, "\"fileCount\":", "fileCount property string in the json file is invalid");
                if (!uint.TryParse(tmpValueRead.ToString().Split(':')[1].TrimEnd(','), out uint fileCount))
                {
                    GeneratorHelpers.ErrorExit("Unable to parse 'fileCount' property's value");
                }

                tmpValueRead = GeneratorHelpers.CheckParseJsonProperty(jsonReader, "\"chunkCount\":", "chunkCount property string in the json file is invalid");
                if (!int.TryParse(tmpValueRead.ToString().Split(':')[1].TrimEnd(','), out int chunkCount))
                {
                    GeneratorHelpers.ErrorExit("Unable to parse 'chunkCount' property's value");
                }

                Console.WriteLine($"TotalChunks: {chunkCount}");
                Console.WriteLine($"No of files: {fileCount}");
                Console.WriteLine("");
                Thread.Sleep(900);

                tmpValueRead = GeneratorHelpers.CheckParseJsonProperty(jsonReader, "\"data\": {", "data property string in the json file is invalid");

                if (Directory.Exists(generatedPathsDir))
                {
                    Directory.Delete(generatedPathsDir, true);
                }


                // Parse each filepath
                var runMainLoop = true;
                bool endLoopNext;

                string currentChunk;
                var chunkStringStartChara = "\"Chunk_";
                var chunkCounter = 0;

                string[] currentData;
                var splitChara = new string[] { ", " };
                var splitChara2 = new string[] { "\": " };
                string fileCodeData;

                string noPathFile;
                uint noPathCounter = 1;
                string fileCodeBinaryVal;
                uint mainTypeVal;

                var generatedPathsDict = new Dictionary<string, List<(string, string)>>();

                var fileCounter = uint.MinValue;

                while (runMainLoop)
                {
                    currentChunk = chunkStringStartChara + chunkCounter + "\"";
                    _ = GeneratorHelpers.CheckParseJsonProperty(jsonReader, currentChunk, $"{currentChunk} property string in the json file is invalid");

                    endLoopNext = false;

                    while (true)
                    {
                        if (endLoopNext)
                        {
                            chunkCounter++;
                            _ = jsonReader.ReadLine();
                            _ = jsonReader.ReadLine();
                            break;
                        }

                        tmpValueRead = jsonReader.ReadLine().TrimStart(' ').TrimEnd(' ');

                        // Assume that the chunk
                        // is going to end if the
                        // string is ending with '}' chara
                        if (tmpValueRead.ToString().EndsWith("}"))
                        {
                            endLoopNext = true;
                        }

                        // Assume that we have the
                        // data at this stage
                        currentData = tmpValueRead.ToString().Split(splitChara, StringSplitOptions.None);
                        if (currentData.Length < 2)
                        {
                            GeneratorHelpers.ErrorExit($"Unable to parse data. occured when parsing {currentChunk}.");
                        }

                        // filecode
                        fileCodeData = currentData[0].ToString();

                        if (!fileCodeData.StartsWith("{ \"fileCode\":"))
                        {
                            GeneratorHelpers.ErrorExit($"fileCode property string in the json file was invalid. occured when parsing {currentChunk}.");
                        }

                        if (!uint.TryParse(fileCodeData.Split(splitChara2, StringSplitOptions.None)[1], out uint fileCode))
                        {
                            GeneratorHelpers.ErrorExit($"unable to parse fileCode property's value. occured when parsing {currentChunk}.");
                        }

                        // path
                        var pathRead = currentData[1];
                        if (!pathRead.StartsWith("\"filePath\":"))
                        {
                            GeneratorHelpers.ErrorExit($"filePath property string in the json file was invalid. occured when parsing {currentChunk}.");
                        }

                        pathRead = pathRead.Split(splitChara2, StringSplitOptions.None)[1];
                        pathRead = pathRead.Remove(0, 1);

                        var pathString = string.Empty;
                        for (int s = 0; s < pathRead.Length; s++)
                        {
                            if (pathRead[s] == '"')
                            {
                                break;
                            }
                            else
                            {
                                pathString += pathRead[s];
                            }
                        }

                        pathRead = pathString.Split(':')[3];

                        // Assume that there
                        // is no path
                        if (pathRead == " ")
                        {
                            noPathFile = Path.Combine(unpackedKELdir, "noPath", $"FILE_{noPathCounter}");

                            if (File.Exists(noPathFile))
                            {
                                fileCodeBinaryVal = fileCode.UIntToBinary();
                                mainTypeVal = fileCodeBinaryVal.BinaryToUInt(0, 8);

                                switch (mainTypeVal)
                                {
                                    case 6:
                                        ZoneDirs.ZFolderNum = fileCodeBinaryVal.BinaryToUInt(8, 8);
                                        ZoneDirs.SubTypeVal = fileCodeBinaryVal.BinaryToUInt(16, 7);
                                        ZoneDirs.SubTypeVal2 = fileCodeBinaryVal.BinaryToUInt(23, 5);
                                        ZoneDirs.Index = fileCodeBinaryVal.BinaryToUInt(28, 4);

                                        ZoneDirs.ProcessZonePath(noPathFile, generatedPathsDict, currentChunk);
                                        break;

                                    case 12:
                                        EventDirs.EvFolderNum = fileCodeBinaryVal.BinaryToUInt(8, 12);
                                        EventDirs.SubTypeVal = fileCodeBinaryVal.BinaryToUInt(20, 4);
                                        EventDirs.SubTypeVal2 = fileCodeBinaryVal.BinaryToUInt(24, 5);
                                        EventDirs.Index = fileCodeBinaryVal.BinaryToUInt(29, 3);

                                        EventDirs.ProcessEventPath(noPathFile, generatedPathsDict, currentChunk);
                                        break;
                                }
                            }

                            noPathCounter++;
                        }

                        fileCounter++;

                        // End the loop if the 
                        // filecounter value is
                        // same as the last file
                        if (fileCounter == fileCount)
                        {
                            runMainLoop = false;
                            break;
                        }
                    }
                }

                Console.WriteLine("");
                Console.WriteLine($"Total paths generated: {PathsGenerated}");

                Console.WriteLine("");
                Console.WriteLine("Generating JSON file....");

                var outJsonFile = Path.Combine(generatedPathsDir, "FileMappings.json");

                if (File.Exists(outJsonFile))
                {
                    File.Delete(outJsonFile);
                }

                using (var jsonWriter = new StreamWriter(outJsonFile, true))
                {
                    jsonWriter.WriteLine("{");
                    jsonWriter.WriteLine("  \"pathCount\": " + PathsGenerated + ",");
                    jsonWriter.WriteLine("  \"pathData\": {");

                    foreach (var key in generatedPathsDict.Keys)
                    {
                        jsonWriter.WriteLine($"             {key}: [");
                        var lastValueSet = generatedPathsDict[key][generatedPathsDict[key].Count - 1];

                        foreach (var values in generatedPathsDict[key])
                        {
                            if (values == lastValueSet)
                            {
                                jsonWriter.WriteLine("               { \"fileName\": " + "\"" + values.Item1 + "\", " + "\"virtualPath\": " + "\"" + values.Item2 + "\" }");

                                if (key == LastKey)
                                {
                                    jsonWriter.WriteLine("             ]");
                                }
                                else
                                {
                                    jsonWriter.WriteLine("             ],");
                                    jsonWriter.WriteLine("");
                                }
                            }
                            else
                            {
                                jsonWriter.WriteLine("               { \"fileName\": " + "\"" + values.Item1 + "\", " + "\"virtualPath\": " + "\"" + values.Item2 + "\" },");
                            }
                        }
                    }

                    jsonWriter.WriteLine("  }");
                    jsonWriter.WriteLine("}");
                }
            }
        }
    }
}