using System;

namespace RateGetters.Rates.Models
{
    public record RateForDate
    {
        private readonly Rate _rate;
        private readonly DateTime _dateTime;

        public RateForDate(Rate rate, DateTime dateTime) => (_rate, _dateTime) = (rate, dateTime);

        public override string ToString() => $"{_rate} for Date {_dateTime.ToShortDateString()}.";
    }
}