using System;
using System.ComponentModel;

namespace RateGetters.Currencies
{
    public record RateForDate
    {
        private DateTime DateTime { get; }
        private CurrencyCodesEnum Code { get; }
        private decimal Value { get; }

        public RateForDate(CurrencyCodesEnum code, DateTime dateTime, decimal value)
        {
            Code = code;
            DateTime = dateTime;
            Value = value;
        }

        public override string ToString()
        {
            return $"Currency \"{Code.ToString()}\" for Date {DateTime.ToShortDateString()} is {Value}.";
        }
    }

    public enum CurrencyCodesEnum
    {
        [Description("USD")]
        Usd,
        [Description("EUR")]
        Eur
    }
}