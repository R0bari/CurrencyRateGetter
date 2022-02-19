using System;
using RateGetters.Rates.Models.Enums;

namespace RateGetters.Rates.Models;

public record RateForDate
{
    public CurrencyCodesEnum Code { get; }
    public decimal Value { get; }
    public DateTime Date { get; }

    public static RateForDate Empty =>
        new RateForDate(CurrencyCodesEnum.None, 0, DateTime.MinValue);            
    public RateForDate(CurrencyCodesEnum code, decimal value, DateTime date) => 
        (Code, Value, Date) = (code, value, date);

    public override string ToString() =>
        $"Currency \"{Code.ToString()}\" for Date {Date.ToShortDateString()} is {Value} .";
}
