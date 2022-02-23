using System;
using RateGetters.Rates.Models.Enums;

namespace RateGetters.Rates.Models;

public record RateForDate(CurrencyCodesEnum Code, decimal Value, DateTime Date)
{
    public static RateForDate Empty =>
        new RateForDate(CurrencyCodesEnum.None, 0, DateTime.MinValue);

    public override string ToString() =>
        $"Currency \"{Code.ToString()}\" for Date {Date.ToShortDateString()} is {Value} .";
}
