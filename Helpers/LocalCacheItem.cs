namespace ExchangeRateServer.Helpers
{
    public class LocalCacheItem
    {
        private DateTime _date;
        private string _currency;
        private double _value;
        private int _amount;

        public string Currency { get => _currency; set => _currency = value; }

        public DateTime Date { get => _date; set => _date = value; }

        public double Value { get => _value; set => _value = double.Parse(value.ToString()); }

        public int Amount { get => _amount; set => _amount = int.Parse(value.ToString()); }

    }
}
