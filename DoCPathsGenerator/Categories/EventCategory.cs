using DoCPathsGenerator.Support;
using System.Collections.Generic;
using System.IO;
using static DoCPathsGenerator.PathsGenerator;
using static DoCPathsGenerator.Support.PathStructures;

namespace DoCPathsGenerator.Categories
{
    internal class EventCategory
    {
        public static uint FileCode { get; set; }
        public static string FileCodeBinary { get; set; }

        private static uint _evFolderNum;
        private static uint _subTypeVal;
        private static uint _subTypeVal2;
        private static uint _index;

        public static void ProcessEventPath(string noPathFile, Dictionary<string, List<(uint, string, string)>> generatedPathsDict, string currentChunk)
        {
            _evFolderNum = FileCodeBinary.BinaryToUInt(8, 12);
            _subTypeVal = FileCodeBinary.BinaryToUInt(20, 4);

            var evFolderNumPadded = SharedMethods.GenerateFolderNameWithNumber("ev", _evFolderNum, 4);

            switch (_subTypeVal)
            {
                case 0:
                case 1:
                    _subTypeVal2 = FileCodeBinary.BinaryToUInt(24, 5);
                    _index = FileCodeBinary.BinaryToUInt(29, 3);

                    var isEventSceneStrBinType0 = _subTypeVal == 1 && _subTypeVal2 == 0 && _index < 8;
                    var isEventSceneStrBinType1 = _subTypeVal == 1 && _subTypeVal2 == 1 && _index < 8;
                    var isEventSceneClassType0 = _subTypeVal == 0 && _subTypeVal2 == 0 && _index < 8;
                    var isEventSceneClassType1 = _subTypeVal == 0 && _subTypeVal2 == 1 && _index < 8;
                    var isEventLocaleTxtBin = _subTypeVal == 1 && _subTypeVal2 == 25;

                    string generatedVPath;
                    string generatedFName;

                    if (isEventSceneStrBinType0)
                    {
                        generatedFName = SharedMethods.ComputeFNameNum(_index, "string", "bin");
                        generatedVPath = Path.Combine(EventSceneDir, evFolderNumPadded, generatedFName);

                        GenerateEventPath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                    }

                    if (isEventSceneStrBinType1)
                    {
                        generatedVPath = Path.Combine(EventSceneDir, evFolderNumPadded, "string08.bin");

                        GenerateEventPath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                    }

                    if (isEventSceneClassType0)
                    {
                        generatedFName = SharedMethods.ComputeFNameNum(_index, "scr0", "class");
                        generatedVPath = Path.Combine(EventSceneDir, evFolderNumPadded, generatedFName);

                        GenerateEventPath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                    }

                    if (isEventSceneClassType1)
                    {
                        generatedVPath = Path.Combine(EventSceneDir, evFolderNumPadded, "scr008.class");

                        GenerateEventPath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                    }

                    if (isEventLocaleTxtBin)
                    {
                        generatedFName = SharedMethods.ComputeFNameLanguage(_index, "string");
                        generatedVPath = Path.Combine(EventLocaleDir, evFolderNumPadded, generatedFName);

                        GenerateEventPath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                    }
                    break;

                case 2:
                case 4:
                    _index = FileCodeBinary.BinaryToUInt(24, 8);

                    var isPtdBin = _subTypeVal == 2;
                    var isEvmRfd = _subTypeVal == 4;

                    if (isPtdBin)
                    {
                        generatedFName = SharedMethods.GenerateFNameWithNumber("ptd", _index, 3, ".bin");
                        generatedVPath = Path.Combine(EventSceneDir, evFolderNumPadded, generatedFName);

                        GenerateEventPath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                    }

                    if (isEvmRfd)
                    {
                        generatedFName = SharedMethods.GenerateFNameWithNumber("evm", _index, 3, ".rfd");
                        generatedVPath = Path.Combine(EventSceneDir, evFolderNumPadded, generatedFName);

                        GenerateEventPath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                    }
                    break;

                case 6:
                case 8:
                    _index = FileCodeBinary.BinaryToUInt(24, 8);

                    var isTexRfd = _subTypeVal == 6;
                    var isSepBin = _subTypeVal == 8;

                    if (isTexRfd)
                    {
                        generatedFName = SharedMethods.GenerateFNameWithNumber("tex", _index, 3, ".rfd");
                        generatedVPath = Path.Combine(EventSceneDir, evFolderNumPadded, generatedFName);

                        GenerateEventPath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                    }

                    if (isSepBin)
                    {
                        if (_index == 0)
                        {
                            generatedVPath = Path.Combine(EventSceneDir, evFolderNumPadded, "sep.bin");
                        }
                        else
                        {
                            generatedFName = SharedMethods.GenerateFNameWithNumber("sep", _index, 3, ".bin");
                            generatedVPath = Path.Combine(EventSceneDir, evFolderNumPadded, generatedFName);
                        }

                        GenerateEventPath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                    }
                    break;

                case 9:
                    _index = FileCodeBinary.BinaryToUInt(24, 8);

                    generatedVPath = Path.Combine(EventSceneDir, evFolderNumPadded, "evtvib.bin");

                    GenerateEventPath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                    break;
            }
        }


        private static void GenerateEventPath(string currentChunk, string generatedVPath, string noPathFile, Dictionary<string, List<(uint, string, string)>> generatedPathsDict)
        {
            LastKey = currentChunk;
            SharedMethods.ProcessGeneratedPath(MoveFiles, generatedVPath, noPathFile, generatedPathsDict, currentChunk, FileCode);
            PathsGenerated++;
        }
    }
}