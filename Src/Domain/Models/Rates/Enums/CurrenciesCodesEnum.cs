using System.ComponentModel;

namespace Domain.Models.Rates.Enums;

public enum CurrencyCodesEnum
{
    [Description("None")]
    None,
    [Description("R01235")]
    Usd,
    [Description("R01239")]
    Eur,
    [Description("-")]
    Rub
}
