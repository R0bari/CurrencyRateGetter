using System;
using Domain.Models.Rates.Enums;

namespace Domain.Models.Rates;

public record RateForDate(CurrencyCodesEnum Code, decimal Value, DateTime Date)
{
    public static RateForDate Empty => new(CurrencyCodesEnum.None, 0, DateTime.MinValue);

    public override string ToString() =>
        $"Currency \"{Code.ToString()}\" for Date {Date.ToShortDateString()} is {Value} .";
}
