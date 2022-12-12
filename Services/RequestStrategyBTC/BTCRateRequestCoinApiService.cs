using System;

namespace ExchangeRateServer.Services.RequestStrategyBTC
{
    public class BTCRateRequestCoinApiService : IBTCRateRequest
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public string RequestSingleDateRate(DateTime date)
        {
            var time = date.ToString("yyyy-MM-dd");

            var url = $"https://rest.coinapi.io/v1/exchangerate/BTC/USD?time={time}";

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);

            request.Headers.Add("X-CoinAPI-Key", "D813CFD6-1FF9-4611-9AAD-39D5E5E8D173");
            
            var responce = _httpClient.Send(request);

            var result = responce.Content.ReadAsStringAsync().Result;

            return result;
        }

    }
}
