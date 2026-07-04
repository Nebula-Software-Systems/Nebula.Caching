using System.IO.Compression;

namespace Nebula.Caching.InMemory.Compression;

public static class GZipCompression
{
    public static byte[] Compress(byte[] dataToCompress)
    {
        using MemoryStream memoryStream = new();
        using (GZipStream gzipStream = new(memoryStream, CompressionLevel.Optimal))
        {
            gzipStream.Write(dataToCompress, 0, dataToCompress.Length);
        }
        return memoryStream.ToArray();
    }

    public static byte[] Decompress(byte[] dataToDecompress)
    {
        using MemoryStream memoryStream = new(dataToDecompress);
        using MemoryStream outputStream = new();
        using (GZipStream decompressStream = new(memoryStream, CompressionMode.Decompress))
        {
            decompressStream.CopyTo(outputStream);
        }
        return outputStream.ToArray();
    }

    public static async Task<byte[]> CompressAsync(byte[] bytes)
    {
        using MemoryStream memoryStream = new();
        GZipStream gzipStream = new(memoryStream, CompressionLevel.Optimal);
        await using (gzipStream.ConfigureAwait(false))
        {
            await gzipStream.WriteAsync(bytes, 0, bytes.Length).ConfigureAwait(false);
        }
        return memoryStream.ToArray();
    }

    public static async Task<byte[]> DecompressAsync(byte[] bytes)
    {
        using MemoryStream memoryStream = new(bytes);
        using MemoryStream outputStream = new();
        GZipStream decompressStream = new(memoryStream, CompressionMode.Decompress);
        await using (decompressStream.ConfigureAwait(false))
        {
            await decompressStream.CopyToAsync(outputStream).ConfigureAwait(false);
        }
        return outputStream.ToArray();
    }
}
