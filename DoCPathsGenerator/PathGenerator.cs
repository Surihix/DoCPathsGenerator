using System;
using System.IO;
using System.Threading;
using static DoCPathsGenerator.PathStructures;

namespace DoCPathsGenerator
{
    internal class PathGenerator
    {
        public static void GeneratePaths(string generatedPathsDir, string unpackedFilelistDir, string unpackedKELdir)
        {
            Console.WriteLine("");

            if (Directory.Exists(generatedPathsDir))
            {
                Console.WriteLine("Removing previously generated paths folder....");
                Console.WriteLine("");
                Directory.Delete(generatedPathsDir, true);
            }

            uint chunksCount;
            using (var countsFileReader = new StreamReader(Path.Combine(unpackedFilelistDir, "~Counts.txt")))
            {
                _ = countsFileReader.ReadLine();
                chunksCount = uint.Parse(countsFileReader.ReadLine());
            }

            Console.WriteLine($"Total Chunks: {chunksCount}");
            Console.WriteLine("");
            Thread.Sleep(1000);

            int chunkFileNum = 0;
            int emptyPathsCounter = 1;

            int lineCount;
            string currentChunkFile;
            string[] currentLineData;
            string currentFilePath;
            uint fileCodeInfo;
            string filePath;
            string generatedVPath;

            string fileCodeBaseVal;
            uint mainTypeVal;
            uint zFolderNum;
            uint evFolderNum;
            uint subTypeVal;
            uint subTypeVal2;
            uint index;

            string generatedFPath;
            int pathsGenerated = 0;

            string appendZeroes;

            for (int i = 0; i < chunksCount; i++)
            {
                currentChunkFile = Path.Combine(unpackedFilelistDir, $"Chunk_{chunkFileNum}.txt");

                if (File.Exists(currentChunkFile))
                {
                    lineCount = File.ReadAllLines(currentChunkFile).Length;

                    using (var sr = new StreamReader(currentChunkFile))
                    {

                        for (int l = 0; l < lineCount; l++)
                        {
                            currentLineData = sr.ReadLine().Split(':');
                            fileCodeInfo = uint.Parse(currentLineData[0].Split('|')[0]);
                            filePath = currentLineData[3];

                            if (filePath == null || filePath == " " || filePath == "")
                            {
                                filePath = $"FILE_{emptyPathsCounter}";
                                currentFilePath = Path.Combine(unpackedKELdir, "noPath", filePath);

                                if (File.Exists(currentFilePath))
                                {
                                    fileCodeBaseVal = Helpers.GetBaseBinaryValue(fileCodeInfo);
                                    mainTypeVal = Helpers.BinaryToUInt(fileCodeBaseVal, 0, 8);

                                    switch (mainTypeVal)
                                    {
                                        case 6:
                                            zFolderNum = Helpers.BinaryToUInt(fileCodeBaseVal, 8, 8);
                                            subTypeVal = Helpers.BinaryToUInt(fileCodeBaseVal, 16, 7);
                                            subTypeVal2 = Helpers.BinaryToUInt(fileCodeBaseVal, 23, 5);
                                            index = Helpers.BinaryToUInt(fileCodeBaseVal, 28, 4);

                                            appendZeroes = Helpers.AppendZeroes("zone", zFolderNum);

                                            var isZoneTxtBin = subTypeVal == 0 && subTypeVal2 == 0 && index == 3;
                                            var isZoneLocaleTxtBin = subTypeVal == 0 && subTypeVal2 == 2;
                                            var isZoneClass = subTypeVal == 0 && subTypeVal2 == 0 && index == 2;


                                            if (isZoneTxtBin)
                                            {
                                                generatedVPath = Path.Combine(ZoneDir, $"z{appendZeroes}{zFolderNum}", $"{filePath}.bin");
                                                Console.WriteLine(generatedVPath);

                                                generatedFPath = Path.Combine(generatedPathsDir, generatedVPath);

                                                CopyToGenPathDir(generatedFPath, currentFilePath);
                                                pathsGenerated++;
                                            }

                                            if (isZoneLocaleTxtBin)
                                            {
                                                generatedVPath = Path.Combine(ZoneLocaleDir, $"z{appendZeroes}{zFolderNum}", $"{filePath}.bin");
                                                Console.WriteLine(generatedVPath);

                                                generatedFPath = Path.Combine(generatedPathsDir, generatedVPath);

                                                CopyToGenPathDir(generatedFPath, currentFilePath);
                                                pathsGenerated++;
                                            }

                                            if (isZoneClass)
                                            {
                                                generatedVPath = Path.Combine(ZoneDir, $"z{appendZeroes}{zFolderNum}", $"{filePath}.class");
                                                Console.WriteLine(generatedVPath);

                                                generatedFPath = Path.Combine(generatedPathsDir, generatedVPath);

                                                CopyToGenPathDir(generatedFPath, currentFilePath);
                                                pathsGenerated++;
                                            }
                                            break;

                                        case 12:
                                            evFolderNum = Helpers.BinaryToUInt(fileCodeBaseVal, 8, 12);
                                            subTypeVal = Helpers.BinaryToUInt(fileCodeBaseVal, 20, 4);
                                            subTypeVal2 = Helpers.BinaryToUInt(fileCodeBaseVal, 24, 5);
                                            index = Helpers.BinaryToUInt(fileCodeBaseVal, 29, 3);

                                            appendZeroes = Helpers.AppendZeroes("event", evFolderNum);

                                            var isEventSceneTxtBinType0 = subTypeVal == 1 && subTypeVal2 == 0 && index < 8;
                                            var isEventSceneTxtBinType1 = subTypeVal == 1 && subTypeVal2 == 1 && index < 8;
                                            var isEventSceneClassType0 = subTypeVal == 0 && subTypeVal2 == 0 && index < 8;
                                            var isEventSceneClassType1 = subTypeVal == 0 && subTypeVal2 == 1 && index < 8;
                                            var isEventLocaleTxtBin = subTypeVal == 1 && subTypeVal2 == 25;


                                            if (isEventSceneTxtBinType0)
                                            {
                                                generatedVPath = Path.Combine(EventSceneDir, $"ev{appendZeroes}{evFolderNum}", $"{filePath}.bin");
                                                Console.WriteLine(generatedVPath);

                                                generatedFPath = Path.Combine(generatedPathsDir, generatedVPath);

                                                CopyToGenPathDir(generatedFPath, currentFilePath);
                                                pathsGenerated++;
                                            }

                                            if (isEventSceneTxtBinType1)
                                            {
                                                generatedVPath = Path.Combine(EventSceneDir, $"ev{appendZeroes}{evFolderNum}", $"{filePath}.bin");
                                                Console.WriteLine(generatedVPath);

                                                generatedFPath = Path.Combine(generatedPathsDir, generatedVPath);

                                                CopyToGenPathDir(generatedFPath, currentFilePath);
                                                pathsGenerated++;
                                            }

                                            if (isEventSceneClassType0)
                                            {
                                                generatedVPath = Path.Combine(EventSceneDir, $"ev{appendZeroes}{evFolderNum}", $"{filePath}.class");
                                                Console.WriteLine(generatedVPath);

                                                generatedFPath = Path.Combine(generatedPathsDir, generatedVPath);

                                                CopyToGenPathDir(generatedFPath, currentFilePath);
                                                pathsGenerated++;
                                            }

                                            if (isEventSceneClassType1)
                                            {
                                                generatedVPath = Path.Combine(EventSceneDir, $"ev{appendZeroes}{evFolderNum}", $"{filePath}.class");
                                                Console.WriteLine(generatedVPath);

                                                generatedFPath = Path.Combine(generatedPathsDir, generatedVPath);

                                                CopyToGenPathDir(generatedFPath, currentFilePath);
                                                pathsGenerated++;
                                            }

                                            if (isEventLocaleTxtBin)
                                            {
                                                generatedVPath = Path.Combine(EventLocaleDir, $"ev{appendZeroes}{evFolderNum}", $"{filePath}.bin");
                                                Console.WriteLine(generatedVPath);

                                                generatedFPath = Path.Combine(generatedPathsDir, generatedVPath);

                                                CopyToGenPathDir(generatedFPath, currentFilePath);
                                                pathsGenerated++;
                                            }
                                            break;
                                    }
                                }

                                emptyPathsCounter++;
                            }
                        }
                    }
                }

                chunkFileNum++;
            }

            Console.WriteLine("");
            Console.WriteLine($"Total paths generated: {pathsGenerated}");
        }

        static void CopyToGenPathDir(string generatedFPath, string currentFilePath)
        {
            if (!Directory.Exists(Path.GetDirectoryName(generatedFPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(generatedFPath));
            }
            File.Copy(currentFilePath, generatedFPath);
        }
    }
}