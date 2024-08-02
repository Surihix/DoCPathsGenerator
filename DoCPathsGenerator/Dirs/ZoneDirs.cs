using System.Collections.Generic;
using System.IO;
using static DoCPathsGenerator.PathsGenerator;
using static DoCPathsGenerator.PathStructures;

namespace DoCPathsGenerator.Dirs
{
    internal class ZoneDirs
    {
        public static uint FileCode { get; set; }
        public static string FileCodeBinary { get; set; }

        private static uint _zFolderNum;
        private static uint _dependantVal;

        private static uint _subTypeVal;
        private static uint _index;

        public static void ProcessZonePath(string noPathFile, Dictionary<string, List<(uint, string, string)>> generatedPathsDict, string currentChunk)
        {
            _zFolderNum = FileCodeBinary.BinaryToUInt(8, 8);
            _dependantVal = FileCodeBinary.BinaryToUInt(16, 8);

            var zFolderNumPadded = GeneratorHelpers.GenerateFolderNameWithNumber("z", _zFolderNum, 3);
            string generatedVPath;
            bool isDone = false;

            switch (_dependantVal)
            {
                // Assume the filepath
                // is a map/mm##.bin
                case 253:
                    _index = FileCodeBinary.BinaryToUInt(24, 8);
                    var mmName = GeneratorHelpers.GenerateFNameWithNumber("mm", _index, 2, ".bin");

                    generatedVPath = Path.Combine(ZoneDir, zFolderNumPadded, "map", mmName);

                    GenerateZonePath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                    break;

                // Assume the filepath
                // is a m255/##.txd
                case 255:
                    _index = FileCodeBinary.BinaryToUInt(24, 8);
                    var txdNum = _index.ToString("x").PadLeft(2, '0');
                    string txdDir = "0";

                    if (_index >= 0x00 && _index <= 0x3f)
                    {
                        txdDir = "0";
                    }

                    if (_index >= 0x40 && _index <= 0x7f)
                    {
                        txdDir = "1";
                    }

                    if (_index >= 0x80 && _index <= 0xbf)
                    {
                        txdDir = "2";
                    }

                    if (_index >= 0xc0 && _index <= 0xff)
                    {
                        txdDir = "3";
                    }

                    generatedVPath = Path.Combine(ZoneDir, zFolderNumPadded, "m255", txdDir, txdNum + ".txd");

                    GenerateZonePath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                    break;

                default:
                    _subTypeVal = FileCodeBinary.BinaryToUInt(24, 4);
                    _index = FileCodeBinary.BinaryToUInt(28, 4);

                    var isZoneCnf = _dependantVal == 0 && _subTypeVal == 0 && _index == 0;
                    var isZoneBzdBin = _dependantVal == 0 && _subTypeVal == 0 && _index == 1;
                    var isZoneClass = _dependantVal == 0 && _subTypeVal == 0 && _index == 2;
                    var isZoneStrBin = _dependantVal == 0 && _subTypeVal == 0 && _index == 3;
                    var isZoneBrdBin = _dependantVal == 0 && _subTypeVal == 0 && _index == 4;
                    var isZoneSepBin = _dependantVal == 0 && _subTypeVal == 0 && _index == 5;
                    var isZoneShpBin = _dependantVal == 0 && _subTypeVal == 0 && _index == 6;
                    var isZoneSdbBin = _dependantVal == 0 && _subTypeVal == 0 && _index == 8;

                    var isZoneAnmKfd = _subTypeVal == 1 && _index == 0;
                    var isZonemapidRfd = _subTypeVal == 1 && _index == 1;
                    var isZoneModelRfd = _subTypeVal == 1 && _index == 2;
                    var isZoneEffectFed = _subTypeVal == 1 && _index == 4;

                    var isZoneLocaleStrBin = _dependantVal == 0 && _subTypeVal == 2 && _index >= 0 && _index <= 6;

                    string generatedDirName = string.Empty;

                    if (_subTypeVal == 1 && _index != 4)
                    {
                        generatedDirName = GeneratorHelpers.GenerateFolderNameWithNumber("m", _dependantVal, 3);
                    }

                    if (isZoneCnf && !isDone)
                    {
                        generatedVPath = Path.Combine(ZoneDir, zFolderNumPadded, "zone.cnf");

                        GenerateZonePath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                        isDone = true;
                    }

                    if (isZoneBzdBin && !isDone)
                    {
                        generatedVPath = Path.Combine(ZoneDir, zFolderNumPadded, "bzd.bin");

                        GenerateZonePath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                        isDone = true;
                    }

                    if (isZoneClass && !isDone)
                    {
                        generatedVPath = Path.Combine(ZoneDir, zFolderNumPadded, "gmap.class");

                        GenerateZonePath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                        isDone = true;
                    }

                    if (isZoneStrBin && !isDone)
                    {
                        generatedVPath = Path.Combine(ZoneDir, zFolderNumPadded, "gmap_str.bin");

                        GenerateZonePath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                        isDone = true;
                    }

                    if (isZoneBrdBin && !isDone)
                    {
                        generatedVPath = Path.Combine(ZoneDir, zFolderNumPadded, "brd.bin");

                        GenerateZonePath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                        isDone = true;
                    }

                    if (isZoneSepBin && !isDone)
                    {
                        generatedVPath = Path.Combine(ZoneDir, zFolderNumPadded, "sound", "sep.bin");

                        GenerateZonePath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                        isDone = true;
                    }

                    if (isZoneShpBin && !isDone)
                    {
                        generatedVPath = Path.Combine(ZoneDir, zFolderNumPadded, "shp.bin");

                        GenerateZonePath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                        isDone = true;
                    }

                    if (isZoneSdbBin && !isDone)
                    {
                        generatedVPath = Path.Combine(ZoneDir, zFolderNumPadded, "sdb.bin");

                        GenerateZonePath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                        isDone = true;
                    }

                    if (isZoneAnmKfd && !isDone)
                    {
                        generatedVPath = Path.Combine(ZoneDir, zFolderNumPadded, generatedDirName, "anm.kfd");

                        GenerateZonePath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                        isDone = true;
                    }

                    if (isZonemapidRfd && !isDone)
                    {
                        generatedVPath = Path.Combine(ZoneDir, zFolderNumPadded, generatedDirName, "mapid.rfd");

                        GenerateZonePath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                        isDone = true;
                    }

                    if (isZoneModelRfd && !isDone)
                    {
                        generatedVPath = Path.Combine(ZoneDir, zFolderNumPadded, generatedDirName, "model.rfd");

                        GenerateZonePath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                        isDone = true;
                    }

                    if (isZoneEffectFed && !isDone)
                    {
                        zFolderNumPadded = GeneratorHelpers.GenerateFolderNameWithNumber("z", _zFolderNum, 4);
                        generatedDirName = GeneratorHelpers.GenerateFolderNameWithNumber("f", _dependantVal, 4);
                        generatedVPath = Path.Combine(ZoneEffectDir, zFolderNumPadded, generatedDirName, "fer", generatedDirName + ".fed");

                        GenerateZonePath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                        isDone = true;
                    }

                    if (isZoneLocaleStrBin && !isDone)
                    {
                        var generatedFName = GeneratorHelpers.ComputeFNameLanguage(_index, "gmap_str");

                        generatedVPath = Path.Combine(ZoneLocaleDir, zFolderNumPadded, generatedFName);

                        GenerateZonePath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                    }

                    break;
            }
        }


        private static void GenerateZonePath(string currentChunk, string generatedVPath, string noPathFile, Dictionary<string, List<(uint, string, string)>> generatedPathsDict)
        {
            LastKey = currentChunk;
            GeneratorHelpers.ProcessGeneratedPath(MoveFiles, generatedVPath, noPathFile, generatedPathsDict, currentChunk, FileCode);
            PathsGenerated++;
        }
    }
}