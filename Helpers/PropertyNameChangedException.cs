namespace ExchangeRateServer.Helpers
{
    public class PropertyNameChangedException : Exception
    {
        public PropertyNameChangedException(string message) 
            : base(message) { }
    }
}
