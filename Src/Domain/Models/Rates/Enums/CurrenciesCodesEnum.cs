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
    [Description("R01035")]
    Gbp,
    [Description("R01375")]
    Cny,
    [Description("-")]
    Rub
}
