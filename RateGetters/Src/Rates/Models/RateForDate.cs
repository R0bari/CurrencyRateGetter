using System;
using System.ComponentModel;

namespace RateGetters.Rates.Models
{
    public record RateForDate
    {
        public Rate Rate { get; }
        public DateTime DateTime { get; }

        public RateForDate(Rate rate, DateTime dateTime)
        {
            Rate = rate;
            DateTime = dateTime;
        }

        public override string ToString() => $"{Rate} for Date {DateTime.ToShortDateString()}.";
    }

    public enum CurrencyCodesEnum
    {
        [Description("R01235")]
        Usd,
        [Description("R01239")]
        Eur
    }
}