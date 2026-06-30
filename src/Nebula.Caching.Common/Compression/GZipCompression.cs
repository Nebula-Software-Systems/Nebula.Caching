using System.IO.Compression;

namespace Nebula.Caching.Common.Compression;

public static class GZipCompression
{
    public static byte[] Compress(byte[] dataToCompress)
    {
        using var memoryStream = new MemoryStream();
        using (var gzipStream = new GZipStream(memoryStream, CompressionLevel.Optimal))
        {
            gzipStream.Write(dataToCompress, 0, dataToCompress.Length);
        }
        return memoryStream.ToArray();
    }

    public static byte[] Decompress(byte[] dataToDecompress)
    {
        using var memoryStream = new MemoryStream(dataToDecompress);
        using var outputStream = new MemoryStream();
        using (var decompressStream = new GZipStream(memoryStream, CompressionMode.Decompress))
        {
            decompressStream.CopyTo(outputStream);
        }
        return outputStream.ToArray();
    }

    public static async Task<byte[]> CompressAsync(byte[] bytes)
    {
        using var memoryStream = new MemoryStream();
        var gzipStream = new GZipStream(memoryStream, CompressionLevel.Optimal);
        await using (gzipStream.ConfigureAwait(false))
        {
            await gzipStream.WriteAsync(bytes, 0, bytes.Length).ConfigureAwait(false);
        }
        return memoryStream.ToArray();
    }

    public static async Task<byte[]> DecompressAsync(byte[] bytes)
    {
        using var memoryStream = new MemoryStream(bytes);
        using var outputStream = new MemoryStream();
        var decompressStream = new GZipStream(memoryStream, CompressionMode.Decompress);
        await using (decompressStream.ConfigureAwait(false))
        {
            await decompressStream.CopyToAsync(outputStream).ConfigureAwait(false);
        }
        return outputStream.ToArray();
    }
}
