using Domain.Models.Rates;
using Domain.Models.Rates.Enums;

namespace UserInterface.Pages.Convert
{
    public record ConvertModel
    {
        private readonly List<RateForDate> _rates;

        private CurrencyCodesEnum _leftCode;
        public CurrencyCodesEnum LeftCode
        {
            get => _leftCode;
            set
            {
                _leftCode = value;
                _rightValue = CalculateRightValue();
            }
        }

        private decimal _leftValue;
        public decimal LeftValue
        {
            get => _leftValue;
            set
            {
                _leftValue = value;
                _rightValue = CalculateRightValue();
            }
        }

        private CurrencyCodesEnum _rightCode;
        public CurrencyCodesEnum RightCode
        {
            get => _rightCode;
            set
            {
                _rightCode = value;
                _leftValue = CalculateLeftValue();
            }
        }

        private decimal _rightValue;
        public decimal RightValue
        {
            get => _rightValue;
            set
            {
                _rightValue = value;
                _leftValue = CalculateLeftValue();
            }
        }

        public static ConvertModel Empty { get; } = new ConvertModel(CurrencyCodesEnum.Usd, CurrencyCodesEnum.Rub, 1m, new List<RateForDate>());
        public ConvertModel(CurrencyCodesEnum left, CurrencyCodesEnum right, decimal leftValue, List<RateForDate> rates)
        {
            (_leftCode, _rightCode, _leftValue, _rates) = (left, right, leftValue, rates);
            _rightValue = CalculateRightValue();
        }

        private decimal GetValueForCurrency(CurrencyCodesEnum code) =>
        code switch
        {
            CurrencyCodesEnum.Rub => 1,
            _ => _rates.Where(c => c.Code == code).FirstOrDefault()?.Value ?? 0m
        };

        private decimal CalculateRightValue() => GetValueForCurrency(_leftCode) / GetValueForCurrency(_rightCode) * _leftValue;
        private decimal CalculateLeftValue() => _leftValue = GetValueForCurrency(_rightCode) / GetValueForCurrency(_leftCode) * _rightValue;

    }
}
