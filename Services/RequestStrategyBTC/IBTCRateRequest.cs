namespace ExchangeRateServer.Services.RequestStrategyBTC
{
    public interface IBTCRateRequest
    {
        public string RequestSingleDateRate(DateTime date);
    }
}
