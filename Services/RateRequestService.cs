using ExchangeRateServer.Helpers;
using ExchangeRateServer.Interfaces;
using ExchangeRateServer.Services.RequestStrategyBTC;
using Microsoft.AspNetCore.WebUtilities;

namespace ExchangeRateServer.Services
{
    public class RateRequestService : IRateRequestService
    {
        private readonly string dateFormat = "yyyy-MM-dd";

        private IBTCRateRequest btcRateRequestService;
        private IHttpClient httpClient;

        public RateRequestService(IBTCRateRequest btcRateRequestService, IHttpClient httpClient)
        {
            this.btcRateRequestService = btcRateRequestService;
            this.httpClient = httpClient;
        }

        public string RequestSingleDateRate(DateTime date, string currency)
        {
            var result = string.Empty;

            try 
            {
                //if BTC
                if (currency == "BTC")
                {
                    result = btcRateRequestService.RequestSingleDateRate(date);
                }
                else
                {
                    result = RequestSingleRatePrivate(date, currency);
                }

                if (result.Contains("404 - страница не найдена"))
                {
                    var message = $"https://www.nbrb.by doesn't contains information about requested currency: {currency} for specified date: {date}";

                    throw new PageNotFoundException(message);
                }
            }
            catch(Exception e)
            {
                throw e;
            }

            return result;
        }

        private string RequestSingleRatePrivate(DateTime date, string currency)
        {
            var parameters = new Dictionary<string, string>() { { "ondate", date.ToString(dateFormat) } };

            var currencyCode = date <= new DateTime(2021, 07, 08) ? CurrenciesCodes.Currencies[currency].Item1 : CurrenciesCodes.Currencies[currency].Item2;

            var nbrbUrl = $"https://www.nbrb.by/api/exrates/rates/{currencyCode}";

            var parametrizedUrl = new Uri(QueryHelpers.AddQueryString(nbrbUrl, parameters));

            var result = httpClient.Get(parametrizedUrl.ToString());

            return result;
        }
    }
}
