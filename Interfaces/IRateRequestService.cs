using ExchangeRateServer.Helpers;

namespace ExchangeRateServer.Interfaces
{
    public interface IRateRequestService
    {
        public string RequestSingleDateRate(DateTime date, string currency);
    }
}
