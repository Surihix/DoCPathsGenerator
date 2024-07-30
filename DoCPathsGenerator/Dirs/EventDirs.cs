using System.Collections.Generic;
using System.IO;
using static DoCPathsGenerator.PathsGenerator;

namespace DoCPathsGenerator.Dirs
{
    internal class EventDirs
    {
        // Main variables
        public static uint EvFolderNum { get; set; }
        public static uint SubTypeVal { get; set; }
        public static uint SubTypeVal2 { get; set; }
        public static uint Index { get; set; }

        public static void ProcessEventPath(string noPathFile, Dictionary<string, List<(string, string)>> generatedPathsDict, string currentChunk)
        {
            var appendZeroes = GeneratorHelpers.AppendZeroes("event", EvFolderNum);

            var isEventSceneStrBinType0 = SubTypeVal == 1 && SubTypeVal2 == 0 && Index < 8;
            var isEventSceneStrBinType1 = SubTypeVal == 1 && SubTypeVal2 == 1 && Index < 8;
            var isEventSceneClassType0 = SubTypeVal == 0 && SubTypeVal2 == 0 && Index < 8;
            var isEventSceneClassType1 = SubTypeVal == 0 && SubTypeVal2 == 1 && Index < 8;
            var isEventLocaleTxtBin = SubTypeVal == 1 && SubTypeVal2 == 25;

            string generatedVPath;
            string generatedFName;

            if (isEventSceneStrBinType0)
            {
                generatedFName = GeneratorHelpers.ComputeFNameNum(Index, "string", "bin");
                generatedVPath = Path.Combine(PathStructures.EventSceneDir, $"ev{appendZeroes}{EvFolderNum}", generatedFName);

                LastKey = currentChunk;
                GeneratorHelpers.ProcessGeneratedPath(generatedVPath, noPathFile, generatedPathsDict, currentChunk);
                PathsGenerated++;
            }

            if (isEventSceneStrBinType1)
            {
                generatedVPath = Path.Combine(PathStructures.EventSceneDir, $"ev{appendZeroes}{EvFolderNum}", "string08.bin");

                LastKey = currentChunk;
                GeneratorHelpers.ProcessGeneratedPath(generatedVPath, noPathFile, generatedPathsDict, currentChunk);
                PathsGenerated++;
            }

            if (isEventSceneClassType0)
            {
                generatedFName = GeneratorHelpers.ComputeFNameNum(Index, "scr0", "class");
                generatedVPath = Path.Combine(PathStructures.EventSceneDir, $"ev{appendZeroes}{EvFolderNum}", generatedFName);

                LastKey = currentChunk;
                GeneratorHelpers.ProcessGeneratedPath(generatedVPath, noPathFile, generatedPathsDict, currentChunk);
                PathsGenerated++;
            }

            if (isEventSceneClassType1)
            {
                generatedVPath = Path.Combine(PathStructures.EventSceneDir, $"ev{appendZeroes}{EvFolderNum}", "scr008.class");

                LastKey = currentChunk;
                GeneratorHelpers.ProcessGeneratedPath(generatedVPath, noPathFile, generatedPathsDict, currentChunk);
                PathsGenerated++;
            }

            if (isEventLocaleTxtBin)
            {
                generatedFName = GeneratorHelpers.ComputeFNameLanguage(Index, "string");
                generatedVPath = Path.Combine(PathStructures.EventLocaleDir, $"ev{appendZeroes}{EvFolderNum}", generatedFName);

                LastKey = currentChunk;
                GeneratorHelpers.ProcessGeneratedPath(generatedVPath, noPathFile, generatedPathsDict, currentChunk);
                PathsGenerated++;
            }
        }
    }
}