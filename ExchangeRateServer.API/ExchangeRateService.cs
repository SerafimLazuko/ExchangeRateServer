using ExchangeRateServer.Helpers;
using ExchangeRateServer.Interfaces;
using System.Text.Json;

namespace ExchangeRateServer.ExchangeRateServer.API
{
    public class ExchangeRateService
    {
        private readonly ICacheService _cacheService;

        public ExchangeRateService(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public async Task<string> GetRatesAsync(string startDate, string endDate, string currencyName)
        {
            var start = DateTime.ParseExact(startDate, "yyyy-MM-dd", null);
            var end = DateTime.ParseExact(endDate, "yyyy-MM-dd", null);
            var currency = currencyName == "BTC" ? currencyName : CurrenciesCodes.Currencies.Where(c => c.Key == currencyName).FirstOrDefault().Key;

            var resultList = new List<LocalCacheItem>();

            try 
            {
                var readCache = _cacheService.ReadCacheAsync(start, end, currency);
                resultList = await readCache;
            }
            catch(Exception e)
            {
                throw e;
            } 

            var result = JsonSerializer.Serialize(resultList, new JsonSerializerOptions() { AllowTrailingCommas = true, WriteIndented = true} );

            return result;
        }
    }
}
