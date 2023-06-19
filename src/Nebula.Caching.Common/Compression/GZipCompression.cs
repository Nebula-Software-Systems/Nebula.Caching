using System.IO.Compression;

namespace Nebula.Caching.Common.Compression
{
    public class GZipCompression
    {

        public static byte[] Compress(byte[] dataToCompress)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var gzipStream = new GZipStream(memoryStream, CompressionLevel.Optimal))
                {
                    gzipStream.Write(dataToCompress, 0, dataToCompress.Length);
                }
                return memoryStream.ToArray();
            }
        }

        public static byte[] Decompress(byte[] dataToDecompress)
        {
            using (var memoryStream = new MemoryStream(dataToDecompress))
            {

                using (var outputStream = new MemoryStream())
                {
                    using (var decompressStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                    {
                        decompressStream.CopyTo(outputStream);
                    }
                    return outputStream.ToArray();
                }
            }
        }

        public async static Task<byte[]> CompressAsync(byte[] bytes)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var gzipStream = new GZipStream(memoryStream, CompressionLevel.Optimal))
                {
                    await gzipStream.WriteAsync(bytes, 0, bytes.Length);
                }
                return memoryStream.ToArray();
            }
        }

        public async static Task<byte[]> DecompressAsync(byte[] bytes)
        {
            using (var memoryStream = new MemoryStream(bytes))
            {
                using (var outputStream = new MemoryStream())
                {
                    using (var decompressStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                    {
                        await decompressStream.CopyToAsync(outputStream);
                    }
                    return outputStream.ToArray();
                }
            }
        }

    }
}