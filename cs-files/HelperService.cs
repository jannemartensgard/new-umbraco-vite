
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Umbraco.Cms.Core.Composing;

namespace GG.Services
{
    public class HelpersServiceComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.Services.AddTransient<IHelpersService, HelpersService>();
        }
    }

    public interface IHelpersService
    {
        List<ManifestItem>? GetResourcesFromManifest();
    }
    public class HelpersService : IHelpersService
    {
        private readonly IMemoryCache _cache;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HelpersService(IMemoryCache memoryCache, IWebHostEnvironment webHostEnvironment)
        {
            _cache = memoryCache;
            _webHostEnvironment = webHostEnvironment;
        }
        
        public List<ManifestItem>? GetResourcesFromManifest()
        {
            var entry = new List<ManifestItem>();

            if (!_cache.TryGetValue("ResourcesCache", out List<ManifestItem>? tentry))
            {
                if (!File.Exists(Path.Combine(_webHostEnvironment.WebRootPath, "dist/manifest.json"))) return entry;
                using var sr =
                    new StreamReader(Path.Combine(_webHostEnvironment.WebRootPath, "dist/manifest.json"));
                var rev =
                    JsonConvert.DeserializeObject<Dictionary<string, ManifestItem>>(sr.ReadToEnd());
                if (rev != null)
                {
                    entry.AddRange(from item in rev where item.Value.File != null select item.Value);
                }
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(3));
                _cache.Set("ResourcesCache", entry, cacheEntryOptions);
            }
            else
            {
                entry = tentry;
            }
            return entry;
        }
    }
    
    public class ManifestItem
    {
        public string? File { get; set; }
        public string? Src { get; set; }
        public bool IsEntry { get; set; }
        public string?[]? Imports { get; set; }
        public string?[]? Css { get; set; }
    }
}