using System.ComponentModel;

namespace RateGetters.Rates.Models
{
    public record Rate
    {
        private readonly CurrencyCodesEnum _code;
        private readonly decimal _value;

            public Rate(CurrencyCodesEnum code, decimal value) => (_code, _value) = (code, value);
        
        public override string ToString() => $"Currency \"{_code.ToString()}\" is {_value}.";
    }
    
    public enum CurrencyCodesEnum
    {
        [Description("R01235")]
        Usd,
        [Description("R01239")]
        Eur
    }
}