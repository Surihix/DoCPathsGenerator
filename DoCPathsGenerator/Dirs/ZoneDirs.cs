using System.Collections.Generic;
using System.IO;
using static DoCPathsGenerator.PathsGenerator;
using static DoCPathsGenerator.PathStructures;

namespace DoCPathsGenerator.Dirs
{
    internal class ZoneDirs
    {
        // Main variables
        public static uint ZFolderNum { get; set; }
        public static uint SubTypeVal { get; set; }
        public static uint SubTypeVal2 { get; set; }
        public static uint Index { get; set; }

        public static void ProcessZonePath(string noPathFile, Dictionary<string, List<(string, string)>> generatedPathsDict, string currentChunk)
        {
            var appendZeroes = GeneratorHelpers.AppendZeroes("zone", ZFolderNum);

            var isZoneCnf = SubTypeVal == 0 && SubTypeVal2 == 0 && Index == 0;
            var isZoneBzdBin = SubTypeVal == 0 && SubTypeVal2 == 0 && Index == 1;
            var isZoneClass = SubTypeVal == 0 && SubTypeVal2 == 0 && Index == 2;
            var isZoneStrBin = SubTypeVal == 0 && SubTypeVal2 == 0 && Index == 3;
            var isZoneBrdBin = SubTypeVal == 0 && SubTypeVal2 == 0 && Index == 4;
            var isZoneSepBin = SubTypeVal == 0 && SubTypeVal2 == 0 && Index == 5;
            var isZoneShpBin = SubTypeVal == 0 && SubTypeVal2 == 0 && Index == 6;
            var isZoneSdbBin = SubTypeVal == 0 && SubTypeVal2 == 0 && Index == 8;
            var isZoneLocaleStrBin = SubTypeVal == 0 && SubTypeVal2 == 2;

            string generatedVPath;

            if (isZoneCnf)
            {
                generatedVPath = Path.Combine(ZoneDir, $"z{appendZeroes}{ZFolderNum}", "zone.cnf");

                LastKey = currentChunk;
                GeneratorHelpers.ProcessGeneratedPath(generatedVPath, noPathFile, generatedPathsDict, currentChunk);
                PathsGenerated++;
            }

            if (isZoneBzdBin)
            {
                generatedVPath = Path.Combine(ZoneDir, $"z{appendZeroes}{ZFolderNum}", "bzd.bin");

                LastKey = currentChunk;
                GeneratorHelpers.ProcessGeneratedPath(generatedVPath, noPathFile, generatedPathsDict, currentChunk);
                PathsGenerated++;
            }

            if (isZoneClass)
            {
                generatedVPath = Path.Combine(ZoneDir, $"z{appendZeroes}{ZFolderNum}", "gmap.class");

                LastKey = currentChunk;
                GeneratorHelpers.ProcessGeneratedPath(generatedVPath, noPathFile, generatedPathsDict, currentChunk);
                PathsGenerated++;
            }

            if (isZoneStrBin)
            {
                generatedVPath = Path.Combine(ZoneDir, $"z{appendZeroes}{ZFolderNum}", "gmap_str.bin");

                LastKey = currentChunk;
                GeneratorHelpers.ProcessGeneratedPath(generatedVPath, noPathFile, generatedPathsDict, currentChunk);
                PathsGenerated++;
            }

            if (isZoneBrdBin)
            {
                generatedVPath = Path.Combine(ZoneDir, $"z{appendZeroes}{ZFolderNum}", "brd.bin");

                LastKey = currentChunk;
                GeneratorHelpers.ProcessGeneratedPath(generatedVPath, noPathFile, generatedPathsDict, currentChunk);
                PathsGenerated++;
            }

            if (isZoneSepBin)
            {
                generatedVPath = Path.Combine(ZoneDir, $"z{appendZeroes}{ZFolderNum}", "sound", "sep.bin");

                LastKey = currentChunk;
                GeneratorHelpers.ProcessGeneratedPath(generatedVPath, noPathFile, generatedPathsDict, currentChunk);
                PathsGenerated++;
            }

            if (isZoneShpBin)
            {
                generatedVPath = Path.Combine(ZoneDir, $"z{appendZeroes}{ZFolderNum}", "shp.bin");

                LastKey = currentChunk;
                GeneratorHelpers.ProcessGeneratedPath(generatedVPath, noPathFile, generatedPathsDict, currentChunk);
                PathsGenerated++;
            }

            if (isZoneSdbBin)
            {
                generatedVPath = Path.Combine(ZoneDir, $"z{appendZeroes}{ZFolderNum}", "sdb.bin");

                LastKey = currentChunk;
                GeneratorHelpers.ProcessGeneratedPath(generatedVPath, noPathFile, generatedPathsDict, currentChunk);
                PathsGenerated++;
            }

            if (isZoneLocaleStrBin)
            {
                var generatedFName = GeneratorHelpers.ComputeFNameLanguage(Index, "gmap_str");

                generatedVPath = Path.Combine(ZoneLocaleDir, $"z{appendZeroes}{ZFolderNum}", generatedFName);

                LastKey = currentChunk;
                GeneratorHelpers.ProcessGeneratedPath(generatedVPath, noPathFile, generatedPathsDict, currentChunk);
                PathsGenerated++;
            }
        }
    }
}