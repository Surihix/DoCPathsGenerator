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
        private static uint _subTypeVal;
        private static uint _subTypeVal2;
        private static uint _index;

        public static void ProcessZonePath(string noPathFile, Dictionary<string, List<(uint, string, string)>> generatedPathsDict, string currentChunk)
        {
            _zFolderNum = FileCodeBinary.BinaryToUInt(8, 8);
            _subTypeVal = FileCodeBinary.BinaryToUInt(16, 7);
            _subTypeVal2 = FileCodeBinary.BinaryToUInt(23, 5);
            _index = FileCodeBinary.BinaryToUInt(28, 4);

            var zFolderNumPadded = GeneratorHelpers.GenerateFolderNameWithNumber("z", _zFolderNum, 3);

            var isZoneCnf = _subTypeVal == 0 && _subTypeVal2 == 0 && _index == 0;
            var isZoneBzdBin = _subTypeVal == 0 && _subTypeVal2 == 0 && _index == 1;
            var isZoneClass = _subTypeVal == 0 && _subTypeVal2 == 0 && _index == 2;
            var isZoneStrBin = _subTypeVal == 0 && _subTypeVal2 == 0 && _index == 3;
            var isZoneBrdBin = _subTypeVal == 0 && _subTypeVal2 == 0 && _index == 4;
            var isZoneSepBin = _subTypeVal == 0 && _subTypeVal2 == 0 && _index == 5;
            var isZoneShpBin = _subTypeVal == 0 && _subTypeVal2 == 0 && _index == 6;
            var isZoneSdbBin = _subTypeVal == 0 && _subTypeVal2 == 0 && _index == 8;
            var isZoneLocaleStrBin = _subTypeVal == 0 && _subTypeVal2 == 2;

            string generatedVPath;

            if (isZoneCnf)
            {
                generatedVPath = Path.Combine(ZoneDir, zFolderNumPadded, "zone.cnf");

                GenerateZonePath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
            }

            if (isZoneBzdBin)
            {
                generatedVPath = Path.Combine(ZoneDir, zFolderNumPadded, "bzd.bin");

                GenerateZonePath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
            }

            if (isZoneClass)
            {
                generatedVPath = Path.Combine(ZoneDir, zFolderNumPadded, "gmap.class");

                GenerateZonePath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
            }

            if (isZoneStrBin)
            {
                generatedVPath = Path.Combine(ZoneDir, zFolderNumPadded, "gmap_str.bin");

                GenerateZonePath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
            }

            if (isZoneBrdBin)
            {
                generatedVPath = Path.Combine(ZoneDir, zFolderNumPadded, "brd.bin");

                GenerateZonePath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
            }

            if (isZoneSepBin)
            {
                generatedVPath = Path.Combine(ZoneDir, zFolderNumPadded, "sound", "sep.bin");

                GenerateZonePath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
            }

            if (isZoneShpBin)
            {
                generatedVPath = Path.Combine(ZoneDir, zFolderNumPadded, "shp.bin");

                GenerateZonePath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
            }

            if (isZoneSdbBin)
            {
                generatedVPath = Path.Combine(ZoneDir, zFolderNumPadded, "sdb.bin");

                GenerateZonePath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
            }

            if (isZoneLocaleStrBin)
            {
                var generatedFName = GeneratorHelpers.ComputeFNameLanguage(_index, "gmap_str");

                generatedVPath = Path.Combine(ZoneLocaleDir, zFolderNumPadded, generatedFName);

                GenerateZonePath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
            }
        }


        private static void GenerateZonePath(string currentChunk, string generatedVPath, string noPathFile, Dictionary<string, List<(uint, string, string)>> generatedPathsDict)
        {
            LastKey = currentChunk;
            GeneratorHelpers.ProcessGeneratedPath(generatedVPath, noPathFile, generatedPathsDict, currentChunk, FileCode);
            PathsGenerated++;
        }
    }
}