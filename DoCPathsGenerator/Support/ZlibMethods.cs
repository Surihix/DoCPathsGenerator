using Ionic.Zlib;
using System.IO;

namespace DoCPathsGenerator.Support
{
    internal class ZlibMethods
    {
        public static void ZlibDecompress(Stream cmpStreamName, Stream outStreamName)
        {
            using (ZlibStream decompressor = new ZlibStream(cmpStreamName, CompressionMode.Decompress))
            {
                decompressor.CopyTo(outStreamName);
            }
        }

        public static byte[] ZlibDecompressBuffer(MemoryStream cmpStreamName)
        {
            return ZlibStream.UncompressBuffer(cmpStreamName.ToArray());
        }
    }
}