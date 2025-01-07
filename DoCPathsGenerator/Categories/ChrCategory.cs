using DoCPathsGenerator.Support;
using System.Collections.Generic;
using System.IO;
using static DoCPathsGenerator.PathsGenerator;
using static DoCPathsGenerator.Support.PathStructures;

namespace DoCPathsGenerator.Categories
{
    internal class ChrCategory
    {
        public static uint FileCode { get; set; }
        public static string FileCodeBinary { get; set; }

        private static uint _subTypeVal;
        private static uint _folderNumber;
        private static uint _index;

        public static void ProcessChrPath(string noPathFile, Dictionary<string, List<(uint, string, string)>> generatedPathsDict, string currentChunk)
        {
            _subTypeVal = FileCodeBinary.BinaryToUInt(8, 8);
            _folderNumber = FileCodeBinary.BinaryToUInt(16, 10);
            _index = FileCodeBinary.BinaryToUInt(26, 6);

            var isMrfdInitialFile = _index == 0;
            var isModelsRfd = _index == 1;
            var isTxtrTxd = _index == 2;
            var isEffectFed = _index == 3;
            var isMrfdFileSet = _index >= 10;

            var chrDirNumSet = string.Empty;
            if (_folderNumber < 10)
            {
                chrDirNumSet = "00" + _folderNumber.ToString();
            }

            if (_folderNumber >= 10)
            {
                chrDirNumSet = "0" + _folderNumber.ToString();
            }

            if (_folderNumber >= 100)
            {
                chrDirNumSet = _folderNumber.ToString();
            }

            chrDirNumSet = Path.Combine(chrDirNumSet[0].ToString(), chrDirNumSet[1].ToString(), chrDirNumSet[2].ToString());

            var idFolderRoot = string.Empty;
            if (_folderNumber < 50)
            {
                idFolderRoot = "0000";
            }

            if (_folderNumber >= 50 && _folderNumber < 100)
            {
                idFolderRoot = "0050";
            }

            if (_folderNumber >= 100 && _folderNumber < 200)
            {
                idFolderRoot = "0100";
            }

            if (_folderNumber >= 200 && _folderNumber < 300)
            {
                idFolderRoot = "0200";
            }

            string generatedVPath;
            string id;

            switch (_subTypeVal)
            {
                // chr/e/#/#/#/
                // effect/enemy/e####/e####/fer/e####.fed
                case 101:
                    if (isMrfdInitialFile)
                    {
                        generatedVPath = Path.Combine(ChrEdir, chrDirNumSet, "m000.rfd");
                        GenerateChrPath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                    }

                    if (isModelsRfd)
                    {
                        generatedVPath = Path.Combine(ChrEdir, chrDirNumSet, "models.rfd");
                        GenerateChrPath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                    }

                    if (isTxtrTxd)
                    {
                        generatedVPath = Path.Combine(ChrEdir, chrDirNumSet, "texture.txd");
                        GenerateChrPath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                    }

                    if (isEffectFed)
                    {
                        id = _folderNumber.ToString().PadLeft(4, '0');
                        generatedVPath = Path.Combine(EffectEnemyDir, $"e{idFolderRoot}", $"e{id}", "fer", $"e{id}.fed");
                        GenerateChrPath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                    }

                    if (isMrfdFileSet)
                    {
                        id = (_index - 10).ToString().PadLeft(3, '0');
                        generatedVPath = Path.Combine(ChrEdir, chrDirNumSet, $"m{id}.rfd");
                        GenerateChrPath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                    }
                    break;

                // chr/g/#/#/#/
                // effect/ground/g####/g####/fer/g####.fed
                case 103:
                    if (isMrfdInitialFile)
                    {
                        generatedVPath = Path.Combine(ChrGdir, chrDirNumSet, "m000.rfd");
                        GenerateChrPath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                    }

                    if (isModelsRfd)
                    {
                        generatedVPath = Path.Combine(ChrGdir, chrDirNumSet, "models.rfd");
                        GenerateChrPath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                    }

                    if (isTxtrTxd)
                    {
                        generatedVPath = Path.Combine(ChrGdir, chrDirNumSet, "texture.txd");
                        GenerateChrPath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                    }

                    if (isEffectFed)
                    {
                        id = _folderNumber.ToString().PadLeft(4, '0');
                        generatedVPath = Path.Combine(EffectGroundDir, $"g{idFolderRoot}", $"g{id}", "fer", $"g{id}.fed");
                        GenerateChrPath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                    }

                    if (isMrfdFileSet)
                    {
                        id = (_index - 10).ToString().PadLeft(3, '0');
                        generatedVPath = Path.Combine(ChrGdir, chrDirNumSet, $"m{id}.rfd");
                        GenerateChrPath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                    }
                    break;

                // chr/m/#/#/#/
                case 109:
                    if (isMrfdInitialFile)
                    {
                        generatedVPath = Path.Combine(ChrMdir, chrDirNumSet, "m000.rfd");
                        GenerateChrPath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                    }

                    if (isModelsRfd)
                    {
                        generatedVPath = Path.Combine(ChrMdir, chrDirNumSet, "models.rfd");
                        GenerateChrPath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                    }

                    if (isTxtrTxd)
                    {
                        generatedVPath = Path.Combine(ChrMdir, chrDirNumSet, "texture.txd");
                        GenerateChrPath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                    }

                    if (isMrfdFileSet)
                    {
                        id = (_index - 10).ToString().PadLeft(3, '0');
                        generatedVPath = Path.Combine(ChrMdir, chrDirNumSet, $"m{id}.rfd");
                        GenerateChrPath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                    }
                    break;

                // chr/n/#/#/#/
                // effect/npc/n####/n####/fer/n####.fed
                case 110:
                    if (isMrfdInitialFile)
                    {
                        generatedVPath = Path.Combine(ChrNdir, chrDirNumSet, "m000.rfd");
                        GenerateChrPath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                    }

                    if (isModelsRfd)
                    {
                        generatedVPath = Path.Combine(ChrNdir, chrDirNumSet, "models.rfd");
                        GenerateChrPath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                    }

                    if (isTxtrTxd)
                    {
                        generatedVPath = Path.Combine(ChrNdir, chrDirNumSet, "texture.txd");
                        GenerateChrPath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                    }

                    if (isEffectFed)
                    {
                        id = _folderNumber.ToString().PadLeft(4, '0');
                        generatedVPath = Path.Combine(EffectNpcDir, $"n{idFolderRoot}", $"n{id}", "fer", $"n{id}.fed");
                        GenerateChrPath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                    }

                    if (isMrfdFileSet)
                    {
                        id = (_index - 10).ToString().PadLeft(3, '0');
                        generatedVPath = Path.Combine(ChrNdir, chrDirNumSet, $"m{id}.rfd");
                        GenerateChrPath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                    }
                    break;

                // chr/o/#/#/#/
                // effect/player/l####/l####/fer/l####.fed
                case 111:
                    if (isMrfdInitialFile)
                    {
                        generatedVPath = Path.Combine(ChrOdir, chrDirNumSet, "m000.rfd");
                        GenerateChrPath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                    }

                    if (isModelsRfd)
                    {
                        generatedVPath = Path.Combine(ChrOdir, chrDirNumSet, "models.rfd");
                        GenerateChrPath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                    }

                    if (isTxtrTxd)
                    {
                        generatedVPath = Path.Combine(ChrOdir, chrDirNumSet, "texture.txd");
                        GenerateChrPath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                    }

                    if (isEffectFed)
                    {
                        id = _folderNumber.ToString().PadLeft(4, '0');
                        generatedVPath = Path.Combine(EffectLineDir, $"l{idFolderRoot}", $"l{id}", "fer", $"l{id}.fed");
                        GenerateChrPath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                    }

                    if (isMrfdFileSet)
                    {
                        id = (_index - 10).ToString().PadLeft(3, '0');
                        generatedVPath = Path.Combine(ChrOdir, chrDirNumSet, $"m{id}.rfd");
                        GenerateChrPath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                    }
                    break;

                // chr/p/#/#/#/
                // effect/player/p####/p####/fer/p####.fed
                case 112:
                    if (isMrfdInitialFile)
                    {
                        generatedVPath = Path.Combine(ChrPdir, chrDirNumSet, "m000.rfd");
                        GenerateChrPath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                    }

                    if (isModelsRfd)
                    {
                        generatedVPath = Path.Combine(ChrPdir, chrDirNumSet, "models.rfd");
                        GenerateChrPath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                    }

                    if (isTxtrTxd)
                    {
                        generatedVPath = Path.Combine(ChrPdir, chrDirNumSet, "texture.txd");
                        GenerateChrPath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                    }

                    if (isEffectFed)
                    {
                        id = _folderNumber.ToString().PadLeft(4, '0');
                        generatedVPath = Path.Combine(EffectPlayerDir, $"p{idFolderRoot}", $"p{id}", "fer", $"p{id}.fed");
                        GenerateChrPath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                    }

                    if (isMrfdFileSet)
                    {
                        id = (_index - 10).ToString().PadLeft(3, '0');
                        generatedVPath = Path.Combine(ChrPdir, chrDirNumSet, $"m{id}.rfd");
                        GenerateChrPath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                    }
                    break;

                // chr/w/#/#/#/
                // effect/weapon/w####/w####/fer/w####.fed
                case 119:
                    if (isMrfdInitialFile)
                    {
                        generatedVPath = Path.Combine(ChrWdir, chrDirNumSet, "m000.rfd");
                        GenerateChrPath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                    }

                    if (isModelsRfd)
                    {
                        generatedVPath = Path.Combine(ChrWdir, chrDirNumSet, "models.rfd");
                        GenerateChrPath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                    }

                    if (isTxtrTxd)
                    {
                        generatedVPath = Path.Combine(ChrWdir, chrDirNumSet, "texture.txd");
                        GenerateChrPath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                    }

                    if (isEffectFed)
                    {
                        id = _folderNumber.ToString().PadLeft(4, '0');
                        generatedVPath = Path.Combine(EffectWeaponDir, $"w{idFolderRoot}", $"w{id}", "fer", $"w{id}.fed");
                        GenerateChrPath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                    }

                    if (isMrfdFileSet)
                    {
                        id = (_index - 10).ToString().PadLeft(3, '0');
                        generatedVPath = Path.Combine(ChrWdir, chrDirNumSet, $"m{id}.rfd");
                        GenerateChrPath(currentChunk, generatedVPath, noPathFile, generatedPathsDict);
                    }
                    break;
            }
        }


        private static void GenerateChrPath(string currentChunk, string generatedVPath, string noPathFile, Dictionary<string, List<(uint, string, string)>> generatedPathsDict)
        {
            LastKey = currentChunk;
            SharedMethods.ProcessGeneratedPath(MoveFiles, generatedVPath, noPathFile, generatedPathsDict, currentChunk, FileCode);
            PathsGenerated++;
        }
    }
}