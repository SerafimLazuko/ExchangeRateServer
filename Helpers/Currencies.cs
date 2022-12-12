

namespace ExchangeRateServer.Helpers
{
    public static class CurrenciesCodes
    {
        // in nbrb since 2016 up to 2021.07.08 currencies were under first codes,
        // after 2021.07.08 codes been changed to second codes
        // ¯\_(ツ)_/¯

        public static Dictionary<string, (int, int)> Currencies = new Dictionary<string, (int, int)>()
        {
            { "USD", (145, 431) },
            { "EUR", (292, 451) },
            { "RUB", (298, 456) }
        };
    }
   
    
}
