using DoCPathsGenerator.Dirs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace DoCPathsGenerator
{
    internal class PathsGenerator
    {
        public static string GeneratedPathsDir { get; set; }
        public static string LastKey { get; set; }

        public static uint PathsGenerated = 0;

        public static void GeneratePaths(string jsonFile, string unpackedKELdir)
        {
            GeneratedPathsDir = Path.Combine(Path.GetDirectoryName(unpackedKELdir), "#generatedPaths");

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
                Console.WriteLine("Generating....");
                Console.WriteLine("");
                Thread.Sleep(200);

                tmpValueRead = GeneratorHelpers.CheckParseJsonProperty(jsonReader, "\"data\": {", "data property string in the json file is invalid");

                if (Directory.Exists(GeneratedPathsDir))
                {
                    Directory.Delete(GeneratedPathsDir, true);
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

                var processedPathsList = new List<(string, uint)>();
                var generatedPathsDict = new Dictionary<string, List<(uint, string, string)>>();

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
                                processedPathsList.Add(($"FILE_{noPathCounter}", fileCode));

                                fileCodeBinaryVal = fileCode.UIntToBinary();
                                mainTypeVal = fileCodeBinaryVal.BinaryToUInt(0, 8);

                                switch (mainTypeVal)
                                {
                                    case 6:
                                        ZoneDirs.FileCode = fileCode;
                                        ZoneDirs.FileCodeBinary = fileCodeBinaryVal;

                                        ZoneDirs.ProcessZonePath(noPathFile, generatedPathsDict, currentChunk);
                                        break;

                                    case 12:
                                        EventDirs.FileCode = fileCode;
                                        EventDirs.FileCodeBinary = fileCodeBinaryVal;

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
                Console.WriteLine("Generating JSON and txt files....");

                // Generate mappings json file
                var outMappingsJsonFile = Path.Combine(GeneratedPathsDir, "FileMappings.json");

                if (File.Exists(outMappingsJsonFile))
                {
                    File.Delete(outMappingsJsonFile);
                }

                using (var mappingsJsonWriter = new StreamWriter(outMappingsJsonFile, true))
                {
                    mappingsJsonWriter.WriteLine("{");
                    mappingsJsonWriter.WriteLine("  \"pathCount\": " + PathsGenerated + ",");
                    mappingsJsonWriter.WriteLine("  \"pathData\": {");

                    foreach (var key in generatedPathsDict.Keys)
                    {
                        mappingsJsonWriter.WriteLine($"             {key}: [");
                        var lastValueSet = generatedPathsDict[key][generatedPathsDict[key].Count - 1];

                        foreach (var values in generatedPathsDict[key])
                        {
                            mappingsJsonWriter.Write("               { ");
                            mappingsJsonWriter.Write("\"fileCode\": " + values.Item1 + ", ");
                            mappingsJsonWriter.Write("\"fileName\": " + "\"" + values.Item2 + "\", ");
                            mappingsJsonWriter.Write("\"virtualPath\": " + "\"" + values.Item3 + "\" ");

                            if (values == lastValueSet)
                            {
                                mappingsJsonWriter.WriteLine("}");

                                if (key == LastKey)
                                {
                                    mappingsJsonWriter.WriteLine("             ]");
                                }
                                else
                                {
                                    mappingsJsonWriter.WriteLine("             ],");
                                    mappingsJsonWriter.WriteLine("");
                                }
                            }
                            else
                            {
                                mappingsJsonWriter.WriteLine("},");
                            }
                        }
                    }

                    mappingsJsonWriter.WriteLine("  }");
                    mappingsJsonWriter.WriteLine("}");
                }

                // Generate processed paths txt file
                var outProcessedTxtFile = Path.Combine(GeneratedPathsDir, "ProcessedPaths.txt");

                if (File.Exists(outProcessedTxtFile))
                {
                    File.Delete(outProcessedTxtFile);
                }

                using (var processedPathsWriter = new StreamWriter(outProcessedTxtFile, true))
                {
                    processedPathsWriter.WriteLine("");
                    processedPathsWriter.WriteLine("processed: " + processedPathsList.Count);
                    processedPathsWriter.WriteLine();

                    foreach (var procItem in processedPathsList)
                    {
                        processedPathsWriter.Write("fileName: " + procItem.Item1 + " | ");
                        processedPathsWriter.WriteLine("fileCode: " + procItem.Item2);
                    }
                }
            }
        }
    }
}