using System.IO;
using System.Text;

namespace DoCPathsGenerator.Filelist
{
    internal class FilelistProcesses
    {
        public static void GetCurrentFileEntry(BinaryReader entriesReader, long entriesReadPos, FilelistVariables filelistVariables)
        {
            entriesReader.BaseStream.Position = entriesReadPos;
            filelistVariables.FileCode = entriesReader.ReadUInt32();
            filelistVariables.ChunkNumber = entriesReader.ReadUInt16();
            filelistVariables.PathStringPos = entriesReader.ReadUInt16();
            filelistVariables.LastChunkNumber = filelistVariables.ChunkNumber;

            GeneratePathString(filelistVariables.PathStringPos, filelistVariables.ChunkDataDict[filelistVariables.ChunkNumber], filelistVariables);
        }

        private static void GeneratePathString(ushort pathPos, byte[] currentChunkData, FilelistVariables filelistVariables)
        {
            var length = 0;

            for (int i = pathPos; i < currentChunkData.Length && currentChunkData[i] != 0; i++)
            {
                length++;
            }

            filelistVariables.PathString = Encoding.UTF8.GetString(currentChunkData, pathPos, length);
        }
    }
}