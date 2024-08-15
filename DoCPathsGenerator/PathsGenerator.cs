using DoCPathsGenerator.Dirs;
using DoCPathsGenerator.Filelist;
using DoCPathsGenerator.Support;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace DoCPathsGenerator
{
    internal class PathsGenerator
    {
        public static bool MoveFiles { get; set; }
        public static string GeneratedPathsDir { get; set; }
        public static string LastKey { get; set; }

        public static uint PathsGenerated = 0;

        public static void GeneratePaths(bool shouldMove, string filelistFile, string unpackedKELdir)
        {
            Console.WriteLine("");
            Console.WriteLine("Reading filelist data....");
            Console.WriteLine("");
            Thread.Sleep(900);

            MoveFiles = shouldMove;
            GeneratedPathsDir = Path.Combine(Path.GetDirectoryName(unpackedKELdir), "#generatedPaths");

            var filelistVariables = new FilelistVariables();

            using (var filelistStream = new FileStream(filelistFile, FileMode.Open, FileAccess.Read))
            {
                using (var filelistReader = new BinaryReader(filelistStream))
                {
                    FilelistChunksPrep.GetFilelistOffsets(filelistReader, filelistVariables);
                    FilelistChunksPrep.BuildChunks(filelistStream, filelistVariables);
                }
            }

            Console.WriteLine($"TotalChunks: {filelistVariables.TotalChunks}");
            Console.WriteLine($"No of files: {filelistVariables.TotalFiles}");
            Console.WriteLine("");
            Thread.Sleep(900);

            if (Directory.Exists(GeneratedPathsDir))
            {
                Console.WriteLine("Detected old generated paths directory. deleting....");
                Console.WriteLine("");

                SharedMethods.IfFileFolderExistsDel(GeneratedPathsDir, false);
            }

            Directory.CreateDirectory(GeneratedPathsDir);

            var generatedPathsDict = new Dictionary<string, List<(uint, string, string)>>();

            using (var entriesStream = new MemoryStream())
            {
                entriesStream.Write(filelistVariables.EntriesData, 0, filelistVariables.EntriesData.Length);
                entriesStream.Seek(0, SeekOrigin.Begin);

                using (var entriesReader = new BinaryReader(entriesStream))
                {
                    // Generate total list of
                    // noPath files on to the
                    // txt file
                    var outNoPathsListTxtFile = Path.Combine(GeneratedPathsDir, "NoPathsList.txt");
                    SharedMethods.IfFileFolderExistsDel(outNoPathsListTxtFile, true);

                    using (var processedPathsWriter = new StreamWriter(outNoPathsListTxtFile, true))
                    {
                        processedPathsWriter.WriteLine();

                        long entriesReadPos = 0;
                        string fileCodeBinaryVal;
                        uint mainTypeVal;

                        for (int f = 0; f < filelistVariables.TotalFiles; f++)
                        {
                            FilelistProcesses.GetCurrentFileEntry(entriesReader, entriesReadPos, filelistVariables);
                            entriesReadPos += 8;

                            filelistVariables.ConvertedStringData = filelistVariables.PathString.Split(':');
                            filelistVariables.MainPath = filelistVariables.ConvertedStringData[3].Replace("/", Convert.ToString(Path.DirectorySeparatorChar));

                            if (filelistVariables.MainPath == " ")
                            {
                                filelistVariables.NoPathFileCount++;
                                filelistVariables.DirectoryPath = "noPath";
                                filelistVariables.FileName = "FILE_" + filelistVariables.NoPathFileCount;
                                filelistVariables.FullFilePath = Path.Combine(unpackedKELdir, filelistVariables.DirectoryPath, filelistVariables.FileName);
                                filelistVariables.MainPath = Path.Combine(filelistVariables.DirectoryPath, filelistVariables.FileName);

                                processedPathsWriter.WriteLine($"fileName: {filelistVariables.FileName} | fileCode: {filelistVariables.FileCode}");

                                if (File.Exists(filelistVariables.FullFilePath))
                                {
                                    fileCodeBinaryVal = filelistVariables.FileCode.UIntToBinary();
                                    mainTypeVal = fileCodeBinaryVal.BinaryToUInt(0, 8);

                                    switch (mainTypeVal)
                                    {
                                        // data/zone
                                        // data/effect/field
                                        // data/bmd
                                        case 6:
                                        case 10:
                                            ZoneCategory.FileCode = filelistVariables.FileCode;
                                            ZoneCategory.FileCodeBinary = fileCodeBinaryVal;
                                            ZoneCategory.ZoneDirType = mainTypeVal;

                                            ZoneCategory.ProcessZonePath(filelistVariables.FullFilePath, generatedPathsDict, $"\"Chunk_{filelistVariables.ChunkNumber}\"");
                                            break;

                                        // data/event
                                        case 12:
                                            EventCategory.FileCode = filelistVariables.FileCode;
                                            EventCategory.FileCodeBinary = fileCodeBinaryVal;

                                            EventCategory.ProcessEventPath(filelistVariables.FullFilePath, generatedPathsDict, $"\"Chunk_{filelistVariables.ChunkNumber}\"");
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Console.WriteLine("");
            Console.WriteLine($"Total paths generated: {PathsGenerated}");


            Console.WriteLine("");
            Console.WriteLine("Generating mappings JSON file....");

            // Generate mappings json file
            var outMappingsJsonFile = Path.Combine(GeneratedPathsDir, "FileMappings.json");
            SharedMethods.IfFileFolderExistsDel(outMappingsJsonFile, true);

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
        }
    }
}