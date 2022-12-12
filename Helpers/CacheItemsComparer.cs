namespace ExchangeRateServer.Helpers
{
    public class CacheItemsComparer : IComparer<LocalCacheItem>
    {
        public int Compare(LocalCacheItem x, LocalCacheItem y)
        {
            return x.Date.CompareTo(y.Date);
        }
    }
}
