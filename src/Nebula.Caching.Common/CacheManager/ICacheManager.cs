namespace Nebula.Caching.Common.CacheManager
{
    public interface ICacheManager
    {
        Task SetAsync(string key, string value, TimeSpan expiration);
        Task<string> GetAsync(string key);
        Task<bool> CacheExistsAsync(string key);
    }
}