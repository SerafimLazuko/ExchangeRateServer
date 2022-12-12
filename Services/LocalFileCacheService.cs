using ExchangeRateServer.Helpers;
using ExchangeRateServer.Interfaces;
using System.Text.Json;

namespace ExchangeRateServer.Services
{
    public class LocalFileCacheService : ICacheService
    {
        private readonly IConfiguration _configuration;
        private readonly IRateRequestService _rateRequestService;
        private List<LocalCacheItem> _localCacheList;
        private string _localCacheFilePath;

        public LocalFileCacheService(IConfiguration configuration, IRateRequestService rateRequestService)
        {
            _rateRequestService = rateRequestService;
            _configuration = configuration;
            _localCacheFilePath = _configuration.GetValue<string>("LocalCache");
            _localCacheList = GetCache();
        }

        public async Task<List<LocalCacheItem>> ReadCacheAsync(DateTime start, DateTime end, string currency)
        {
            var resultList = new List<LocalCacheItem>();

            foreach (var item in _localCacheList.Where(i => i.Currency == currency))
            {
                if (item.Date >= start && item.Date <= end)
                {
                    resultList.Add(item);
                }
            }

            // we need to check if all requested dates are in result list 
            // (in case when some rate for some date wasn't load earlier to the local cache file)

            var requestedDates = AllDatesRequested(start, end);
            var obtainedDates = AllDatesObtained(resultList);

            var missingDates = requestedDates.Except(obtainedDates);

            if(missingDates.Count() > 0)
            {
                foreach(var date in missingDates)
                {
                    try
                    {
                        var singleDateRate = _rateRequestService.RequestSingleDateRate(DateTime.Parse(date), currency);

                        bool isBTC = currency == "BTC";
                        var localCacheItem = new LocalCacheItem();

                        if (isBTC)
                        {
                            var deserializedJsonCoinApi = JsonSerializer.Deserialize<DeserializedSingleDateBTCCoinApi>(singleDateRate);

                            localCacheItem.Currency = currency.ToString();
                            localCacheItem.Value = deserializedJsonCoinApi.rate;
                            localCacheItem.Amount = 1;
                            localCacheItem.Date = DateTime.Parse(date).AddMinutes(2);
                        }
                        else
                        {
                            var deserializedJsonNbrb = JsonSerializer.Deserialize<DeserializeSingleDateRateNbrb>(singleDateRate);
                            if (deserializedJsonNbrb.Date == default(DateTime) || deserializedJsonNbrb.Cur_OfficialRate == default(double) || deserializedJsonNbrb.Cur_Scale == default(int))
                                throw new PropertyNameChangedException($"Error occured while attempting to get rate for currency: {currency} for the date: {date}. Getting object: {singleDateRate}");

                            localCacheItem.Currency = currency.ToString();
                            localCacheItem.Value = deserializedJsonNbrb.Cur_OfficialRate;
                            localCacheItem.Amount = deserializedJsonNbrb.Cur_Scale;
                            localCacheItem.Date = DateTime.Parse(date);
                        }

                        resultList.Add(localCacheItem);

                        // write it to local cache

                        await WriteCacheAsync(localCacheItem);
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }

            resultList.Sort(new CacheItemsComparer());

            return resultList;
        }

        public async Task WriteCacheAsync(LocalCacheItem cache)
        {
            _localCacheList.Add(cache);
            _localCacheList.Sort(new CacheItemsComparer());

            var serializationOptions = new JsonSerializerOptions()
            {
                WriteIndented = true,
                AllowTrailingCommas = true
            };

            string serializedCache = JsonSerializer.Serialize(_localCacheList, serializationOptions);

            try
            {
                // override cache file
                File.WriteAllText(_localCacheFilePath, "");

                // write all cache
                using (var writer = new StreamWriter(_localCacheFilePath))
                {
                    if (!File.Exists(_localCacheFilePath))
                        throw new FileNotFoundException($"Occured while performing writing cache to the local file. File {_localCacheFilePath} not found!");
                    
                    await writer.WriteAsync(serializedCache);
                }
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        private List<LocalCacheItem> GetCache()
        {
            var result = new List<LocalCacheItem>();

            try
            {
                using (var fs = new FileStream(_localCacheFilePath, FileMode.OpenOrCreate))
                {
                    if (!File.Exists(_localCacheFilePath))
                        throw new FileNotFoundException($"Occured while performing reading cache from the local file. File {_localCacheFilePath} not found!");

                    var deserialized = JsonSerializer.Deserialize<List<LocalCacheItem>>(fs);

                    foreach (var item in deserialized)
                    {
                        result.Add(item);
                    }
                }
            } catch (Exception e)
            {
                throw;
            }
            

            result.Sort(new CacheItemsComparer());
            return result;
        }

        private List<string> AllDatesRequested(DateTime start, DateTime end)
        {
            var result = new List<string>();

            for (var dt = start; dt <= end; dt = dt.AddDays(1))
            {
                result.Add(dt.ToString("yyyy.M.d"));
            }

            return result;
        }

        private List<string> AllDatesObtained(List<LocalCacheItem> localCacheItems)
        {
            return localCacheItems.Select(i => i.Date.ToString("yyyy.M.d")).ToList();
        }
    }
}