using ExchangeRateServer.Helpers;

namespace ExchangeRateServer.Interfaces
{
    public interface ICacheService
    {
        public Task WriteCacheAsync(LocalCacheItem cache);

        public Task<List<LocalCacheItem>> ReadCacheAsync(DateTime start, DateTime end, string currency);
    }
}
