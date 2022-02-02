using System;
using System.Data;
using System.Text;
using RateGetters.Extensions;

namespace RateGetters.Currencies.Getters
{
    public record CbrRateGetter : IRateGetter
    {
        private const string CbrLink = "http://www.cbr.ru/scripts/XML_daily.asp";
        public CbrRateGetter()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
        
        public RateGetterResult<RateForDate> GetRate(DateTime dateTime, CurrencyCodesEnum code)
        {
            var ds = new DataSet();
            var dateInRequiredFormat = dateTime
                .ToShortDateString()
                .Replace('.', '/');
            ds.ReadXml($"{CbrLink}?date_req={dateInRequiredFormat}");
            var currency = ds.Tables["Valute"];
            if (currency?.Rows is null)
            {
                return RateGetterResult<RateForDate>.Failed("Given rate for given currency not found.");
            }
            
            foreach (DataRow row in currency.Rows)
            {
                if (row["CharCode"].ToString() != code.Description())
                {
                    continue;
                }
                return RateGetterResult<RateForDate>.Successful(
                    new RateForDate(
                        code,
                        dateTime,
                        Convert.ToDecimal(row["Value"].ToString()))); //Возвращаю значение курсы валюты
            }
            return RateGetterResult<RateForDate>.Failed("Currency with given code not found.");
            
        }
    }
    
}