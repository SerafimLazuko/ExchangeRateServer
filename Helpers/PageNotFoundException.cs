namespace ExchangeRateServer.Helpers
{
    public class PageNotFoundException : Exception
    {
        public PageNotFoundException(string message)
            : base(message) { }
    }
}
