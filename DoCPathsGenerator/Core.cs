using System;
using System.IO;

namespace DoCPathsGenerator
{
    internal class Core
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Enough arguments not specified\n");
                Console.WriteLine("Example:");
                Console.WriteLine("DoCPathsGenerator.exe [unpacked filelist folder] [unpacked _KEL.DAT folder]");
                Console.ReadLine();
                Environment.Exit(0);
            }

            var unpackedFilelistDir = args[0];
            var unpackedKELdir = args[1];
            var generatedPathsDir = Path.Combine(Path.GetDirectoryName(unpackedKELdir), "generatedPaths");

            var countsFile = Path.Combine(unpackedFilelistDir, "~Counts.txt");

            if (!File.Exists(countsFile))
            {
                Helpers.ErrorExit("Missing '~Counts.txt' file in the unpacked filelist directory");
            }

            if (!Directory.Exists(unpackedFilelistDir))
            {
                Helpers.ErrorExit("Specified filelist directory is missing");
            }

            if (!Directory.Exists(unpackedKELdir))
            {
                Helpers.ErrorExit("Specified '_KEL.DAT' directory is missing");
            }

            // add try catch block
            if (Directory.Exists(generatedPathsDir))
            {
                Console.WriteLine("Removing previously generated paths folder....");
                Console.WriteLine("");
                Directory.Delete(generatedPathsDir, true);
            }

            uint chunksCount;
            using (var countsFileReader = new StreamReader(countsFile))
            {
                _ = countsFileReader.ReadLine();
                chunksCount = uint.Parse(countsFileReader.ReadLine());
            }

            Console.WriteLine($"Total Chunks: {chunksCount}");
            Console.WriteLine("");

            int chunkFileNum = 0;
            int emptyPathsCounter = 1;

            int lineCount;
            string currentChunkFile;
            string[] currentLineData;
            string currentFilePath;
            uint fileCodeInfo;
            string filePath;
            string generatedFilePath;

            string fileCodeBaseVal;
            uint typeVal;
            uint zFolderNum;
            uint evFolderNum;
            uint subTypeVal;
            uint subTypeVal2;
            uint index;

            string generatedOutPath;
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
                                    typeVal = Helpers.BinaryToUInt(fileCodeBaseVal, 0, 8);

                                    switch (typeVal)
                                    {
                                        case 6:
                                            zFolderNum = Helpers.BinaryToUInt(fileCodeBaseVal, 8, 8);
                                            subTypeVal = Helpers.BinaryToUInt(fileCodeBaseVal, 16, 7);
                                            subTypeVal2 = Helpers.BinaryToUInt(fileCodeBaseVal, 23, 5);
                                            index = Helpers.BinaryToUInt(fileCodeBaseVal, 28, 4);

                                            appendZeroes = Helpers.AppendZeroes("zone", zFolderNum);

                                            if (subTypeVal == 0 && subTypeVal2 == 0 && index == 3)
                                            {
                                                generatedFilePath = Path.Combine(PathStructures.ZoneDir, $"z{appendZeroes}{zFolderNum}", $"{filePath}.bin");
                                                Console.WriteLine(generatedFilePath);

                                                generatedOutPath = Path.Combine(generatedPathsDir, generatedFilePath);
                                                if (!Directory.Exists(Path.GetDirectoryName(generatedOutPath)))
                                                {
                                                    Directory.CreateDirectory(Path.GetDirectoryName(generatedOutPath));
                                                }
                                                File.Copy(currentFilePath, generatedOutPath);

                                                pathsGenerated++;
                                            }

                                            if (subTypeVal == 0 && subTypeVal2 == 2)
                                            {
                                                generatedFilePath = Path.Combine(PathStructures.ZoneLocaleDir, $"z{appendZeroes}{zFolderNum}", $"{filePath}.bin");
                                                Console.WriteLine(generatedFilePath);

                                                generatedOutPath = Path.Combine(generatedPathsDir, generatedFilePath);
                                                if (!Directory.Exists(Path.GetDirectoryName(generatedOutPath)))
                                                {
                                                    Directory.CreateDirectory(Path.GetDirectoryName(generatedOutPath));
                                                }
                                                File.Copy(currentFilePath, generatedOutPath);

                                                pathsGenerated++;
                                            }
                                            break;

                                        case 12:
                                            evFolderNum = Helpers.BinaryToUInt(fileCodeBaseVal, 8, 12);
                                            subTypeVal = Helpers.BinaryToUInt(fileCodeBaseVal, 20, 4);
                                            subTypeVal2 = Helpers.BinaryToUInt(fileCodeBaseVal, 24, 5);
                                            index = Helpers.BinaryToUInt(fileCodeBaseVal, 29, 3);

                                            appendZeroes = Helpers.AppendZeroes("event", evFolderNum);

                                            if (subTypeVal == 1 && subTypeVal2 == 0 && index < 8)
                                            {
                                                generatedFilePath = Path.Combine(PathStructures.EventSceneDir, $"ev{appendZeroes}{evFolderNum}", $"{filePath}.bin");
                                                Console.WriteLine(generatedFilePath);

                                                generatedOutPath = Path.Combine(generatedPathsDir, generatedFilePath);
                                                if (!Directory.Exists(Path.GetDirectoryName(generatedOutPath)))
                                                {
                                                    Directory.CreateDirectory(Path.GetDirectoryName(generatedOutPath));
                                                }
                                                File.Copy(currentFilePath, generatedOutPath);

                                                pathsGenerated++;
                                            }

                                            if (subTypeVal == 1 && subTypeVal2 == 1 && index < 8)
                                            {
                                                generatedFilePath = Path.Combine(PathStructures.EventSceneDir, $"ev{appendZeroes}{evFolderNum}", $"{filePath}.bin");
                                                Console.WriteLine(generatedFilePath);

                                                generatedOutPath = Path.Combine(generatedPathsDir, generatedFilePath);
                                                if (!Directory.Exists(Path.GetDirectoryName(generatedOutPath)))
                                                {
                                                    Directory.CreateDirectory(Path.GetDirectoryName(generatedOutPath));
                                                }
                                                File.Copy(currentFilePath, generatedOutPath);

                                                pathsGenerated++;
                                            }

                                            if (subTypeVal == 1 && subTypeVal2 == 25)
                                            {
                                                generatedFilePath = Path.Combine(PathStructures.EventLocaleDir, $"ev{appendZeroes}{evFolderNum}", $"{filePath}.bin");
                                                Console.WriteLine(generatedFilePath);

                                                generatedOutPath = Path.Combine(generatedPathsDir, generatedFilePath);
                                                if (!Directory.Exists(Path.GetDirectoryName(generatedOutPath)))
                                                {
                                                    Directory.CreateDirectory(Path.GetDirectoryName(generatedOutPath));
                                                }
                                                File.Copy(currentFilePath, generatedOutPath);

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
            Console.WriteLine("Finished generating paths");
            Console.WriteLine("");
            Console.WriteLine($"Total paths generated: {pathsGenerated}");
            Console.ReadLine();
        }
    }
}