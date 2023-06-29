using Common.Settings;

namespace Nebula.Caching.InMemory.Settings
{
    public class InMemoryOptions : BaseOptions
    {
        public override string ConfigurationRoot { get; set; } = "InMemory";
    }
}