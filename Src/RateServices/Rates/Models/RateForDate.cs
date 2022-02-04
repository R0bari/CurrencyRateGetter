using System;

namespace RateGetters.Rates.Models
{
    public record RateForDate
    {
        public Rate Rate { get; }
        public DateTime DateTime { get; }

        public RateForDate(Rate rate, DateTime dateTime) => (Rate, DateTime) = (rate, dateTime);

        public override string ToString() => $"{Rate} for Date {DateTime.ToShortDateString()}.";
    }
}