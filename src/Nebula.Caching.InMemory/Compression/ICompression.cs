namespace Nebula.Caching.Common.Compression
{
    public interface ICompression
    {
        Task<byte[]> CompressAsync(byte[] bytes);
        Task<byte[]> DecompressAsync(byte[] bytes);
    }
}
