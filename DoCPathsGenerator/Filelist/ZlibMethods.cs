using Ionic.Zlib;
using System.IO;

namespace DoCPathsGenerator.Filelist
{
    internal class ZlibMethods
    {
        public static byte[] ZlibDecompressBuffer(MemoryStream cmpStreamName)
        {
            return ZlibStream.UncompressBuffer(cmpStreamName.ToArray());
        }
    }
}