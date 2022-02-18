using System;
using RateGetters.Rates.Models.Enums;

namespace RateGetters.Rates.Models
{
    public record RateForDate
    {
        public CurrencyCodesEnum Code { get; }
        public decimal Value { get; }
        public DateTime DateTime { get; }

        public static RateForDate Empty =>
            new RateForDate(CurrencyCodesEnum.None, 0, DateTime.MinValue);            
        public RateForDate(CurrencyCodesEnum code, decimal value, DateTime dateTime) => 
            (Code, Value, DateTime) = (code, value, dateTime);

        public override string ToString() =>
            $"Currency \"{Code.ToString()}\" for Date {DateTime.ToShortDateString()} is {Value} .";
    }
}