namespace Common.Settings
{
    public abstract class BaseOptions
    {
        public virtual string ConfigurationRoot { get; set; }
        public virtual string CacheServiceUrl { get; set; }
    }
}