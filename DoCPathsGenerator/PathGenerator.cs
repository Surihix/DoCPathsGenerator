using System;
using System.IO;
using System.Threading;
using static DoCPathsGenerator.PathStructures;

namespace DoCPathsGenerator
{
    internal class PathGenerator
    {
        public static void GeneratePaths(uint chunksCount, string generatedPathsDir, string unpackedFilelistDir, string unpackedKELdir)
        {
            Console.WriteLine("");

            if (Directory.Exists(generatedPathsDir))
            {
                Console.WriteLine("Removing previously generated paths folder....");
                Console.WriteLine("");
                Directory.Delete(generatedPathsDir, true);
            }

            Console.WriteLine($"Total Chunks: {chunksCount}");
            Console.WriteLine("");
            Thread.Sleep(900);


            var chunkFileNum = 0;
            var emptyPathsCounter = 1;
            var pathsGenerated = 0;

            int lineCount;
            string currentChunkFile, currentFPath, filePath, generatedVPath, fileCodeBinaryVal, appendZeroes;
            string[] currentChunkData, currentLineData;

            uint fileCode, mainTypeVal, zFolderNum, evFolderNum, subTypeVal, subTypeVal2, index;

            for (int i = 0; i < chunksCount; i++)
            {
                currentChunkFile = Path.Combine(unpackedFilelistDir, $"Chunk_{chunkFileNum}.txt");

                if (File.Exists(currentChunkFile))
                {
                    currentChunkData = File.ReadAllLines(currentChunkFile);
                    lineCount = currentChunkData.Length;

                    for (int l = 0; l < lineCount; l++)
                    {
                        currentLineData = currentChunkData[l].Split(':');
                        fileCode = uint.Parse(currentLineData[0].Split('|')[0]);
                        filePath = currentLineData[3];

                        if (filePath == null || filePath == " " || filePath == "")
                        {
                            filePath = $"FILE_{emptyPathsCounter}";
                            currentFPath = Path.Combine(unpackedKELdir, "noPath", filePath);

                            if (File.Exists(currentFPath))
                            {
                                var valueArray = BitConverter.GetBytes(fileCode);
                                fileCodeBinaryVal = Convert.ToString(valueArray[3], 2).PadLeft(8, '0') + "" +
                                    Convert.ToString(valueArray[2], 2).PadLeft(8, '0') + "" +
                                    Convert.ToString(valueArray[1], 2).PadLeft(8, '0') + "" +
                                    Convert.ToString(valueArray[0], 2).PadLeft(8, '0');

                                mainTypeVal = GeneratorHelpers.BinaryToUInt(fileCodeBinaryVal, 0, 8);

                                switch (mainTypeVal)
                                {
                                    case 6:
                                        zFolderNum = GeneratorHelpers.BinaryToUInt(fileCodeBinaryVal, 8, 8);
                                        subTypeVal = GeneratorHelpers.BinaryToUInt(fileCodeBinaryVal, 16, 7);
                                        subTypeVal2 = GeneratorHelpers.BinaryToUInt(fileCodeBinaryVal, 23, 5);
                                        index = GeneratorHelpers.BinaryToUInt(fileCodeBinaryVal, 28, 4);

                                        appendZeroes = GeneratorHelpers.AppendZeroes("zone", zFolderNum);

                                        var isZoneTxtBin = subTypeVal == 0 && subTypeVal2 == 0 && index == 3;
                                        var isZoneLocaleTxtBin = subTypeVal == 0 && subTypeVal2 == 2;
                                        var isZoneClass = subTypeVal == 0 && subTypeVal2 == 0 && index == 2;


                                        if (isZoneTxtBin)
                                        {
                                            generatedVPath = Path.Combine(ZoneDir, $"z{appendZeroes}{zFolderNum}", $"{filePath}.bin");

                                            GeneratorHelpers.CopyFileToPath(generatedVPath, generatedPathsDir, currentFPath);
                                            pathsGenerated++;
                                        }

                                        if (isZoneLocaleTxtBin)
                                        {
                                            generatedVPath = Path.Combine(ZoneLocaleDir, $"z{appendZeroes}{zFolderNum}", $"{filePath}.bin");

                                            GeneratorHelpers.CopyFileToPath(generatedVPath, generatedPathsDir, currentFPath);
                                            pathsGenerated++;
                                        }

                                        if (isZoneClass)
                                        {
                                            generatedVPath = Path.Combine(ZoneDir, $"z{appendZeroes}{zFolderNum}", $"{filePath}.class");

                                            GeneratorHelpers.CopyFileToPath(generatedVPath, generatedPathsDir, currentFPath);
                                            pathsGenerated++;
                                        }
                                        break;

                                    case 12:
                                        evFolderNum = GeneratorHelpers.BinaryToUInt(fileCodeBinaryVal, 8, 12);
                                        subTypeVal = GeneratorHelpers.BinaryToUInt(fileCodeBinaryVal, 20, 4);
                                        subTypeVal2 = GeneratorHelpers.BinaryToUInt(fileCodeBinaryVal, 24, 5);
                                        index = GeneratorHelpers.BinaryToUInt(fileCodeBinaryVal, 29, 3);

                                        appendZeroes = GeneratorHelpers.AppendZeroes("event", evFolderNum);

                                        var isEventSceneTxtBinType0 = subTypeVal == 1 && subTypeVal2 == 0 && index < 8;
                                        var isEventSceneTxtBinType1 = subTypeVal == 1 && subTypeVal2 == 1 && index < 8;
                                        var isEventSceneClassType0 = subTypeVal == 0 && subTypeVal2 == 0 && index < 8;
                                        var isEventSceneClassType1 = subTypeVal == 0 && subTypeVal2 == 1 && index < 8;
                                        var isEventLocaleTxtBin = subTypeVal == 1 && subTypeVal2 == 25;


                                        if (isEventSceneTxtBinType0)
                                        {
                                            generatedVPath = Path.Combine(EventSceneDir, $"ev{appendZeroes}{evFolderNum}", $"{filePath}.bin");

                                            GeneratorHelpers.CopyFileToPath(generatedVPath, generatedPathsDir, currentFPath);
                                            pathsGenerated++;
                                        }

                                        if (isEventSceneTxtBinType1)
                                        {
                                            generatedVPath = Path.Combine(EventSceneDir, $"ev{appendZeroes}{evFolderNum}", $"{filePath}.bin");

                                            GeneratorHelpers.CopyFileToPath(generatedVPath, generatedPathsDir, currentFPath);
                                            pathsGenerated++;
                                        }

                                        if (isEventSceneClassType0)
                                        {
                                            generatedVPath = Path.Combine(EventSceneDir, $"ev{appendZeroes}{evFolderNum}", $"{filePath}.class");

                                            GeneratorHelpers.CopyFileToPath(generatedVPath, generatedPathsDir, currentFPath);
                                            pathsGenerated++;
                                        }

                                        if (isEventSceneClassType1)
                                        {
                                            generatedVPath = Path.Combine(EventSceneDir, $"ev{appendZeroes}{evFolderNum}", $"{filePath}.class");

                                            GeneratorHelpers.CopyFileToPath(generatedVPath, generatedPathsDir, currentFPath);
                                            pathsGenerated++;
                                        }

                                        if (isEventLocaleTxtBin)
                                        {
                                            generatedVPath = Path.Combine(EventLocaleDir, $"ev{appendZeroes}{evFolderNum}", $"{filePath}.bin");

                                            GeneratorHelpers.CopyFileToPath(generatedVPath, generatedPathsDir, currentFPath);
                                            pathsGenerated++;
                                        }
                                        break;
                                }
                            }

                            emptyPathsCounter++;
                        }
                    }
                }

                chunkFileNum++;
            }

            Console.WriteLine("");
            Console.WriteLine($"Total paths generated: {pathsGenerated}");
        }
    }
}