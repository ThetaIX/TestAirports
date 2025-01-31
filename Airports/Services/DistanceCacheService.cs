using Airports.Interfaces;
using Airports.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace Airports.Services
{
    public class DistanceCacheService : IDistanceCacheService
    {
        private readonly string _cacheFile = "distance_cache.json";
        private readonly IMemoryCache _memoryCache;
        private readonly SemaphoreSlim _lock = new(1, 1);
        private Dictionary<string, DistResponse> _cache = new();

        public DistanceCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            LoadCacheFromFile();
        }

        private void LoadCacheFromFile()
        {
            if (File.Exists(_cacheFile))
            {
                var json = File.ReadAllText(_cacheFile);
                _cache = JsonSerializer.Deserialize<Dictionary<string, DistResponse>>(json) ?? new();
            }
        }

        private void SaveCacheToFile()
        {
            var json = JsonSerializer.Serialize(_cache);
            File.WriteAllText(_cacheFile, json);
        }

        public async Task<DistResponse?> GetCachedDistanceAsync(string airportCode1, string airportCode2)
        {
            string key = $"{airportCode1}-{airportCode2}";
            string reverseKey = $"{airportCode2}-{airportCode1}";

            if (_memoryCache.TryGetValue(key, out DistResponse response) ||
                _memoryCache.TryGetValue(reverseKey, out response))
            {
                return response;
            }

            await _lock.WaitAsync();
            try
            {
                if (_cache.TryGetValue(key, out response) || _cache.TryGetValue(reverseKey, out response))
                {
                    _memoryCache.Set(key, response, TimeSpan.FromMinutes(30));
                    return response;
                }
            }
            finally
            {
                _lock.Release();
            }

            return null;
        }

        public async Task SaveDistanceAsync(DistResponse response)
        {
            string key = $"{response.From}-{response.To}";

            _memoryCache.Set(key, response, TimeSpan.FromMinutes(30));

            await _lock.WaitAsync();
            try
            {
                _cache[key] = response;
                SaveCacheToFile();
            }
            finally
            {
                _lock.Release();
            }
        }
    }
}
