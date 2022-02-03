namespace RateGetters.Rates.Models
{
    public record Rate
    {
        public CurrencyCodesEnum Code { get; }
        public decimal Value { get; }
        
        public Rate(CurrencyCodesEnum code, decimal value)
        {
            Code = code;
            Value = value;
        }
        
        public override string ToString() => $"Currency \"{Code.ToString()}\" is {Value}.";
    }
}