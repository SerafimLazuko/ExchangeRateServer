namespace ExchangeRateServer.Helpers
{
    public class DeserializedSingleDateBTCCoinApi
    {
        public DateTime time { get; set; }
        public string asset_id_base { get; set; }
        public string asset_id_quote { get; set; }
        public double rate { get; set; }
    }
}
