﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using RateGetters.Infrastructure.Extensions;
using RateGetters.Rates.Interfaces;
using RateGetters.Rates.Models;
using RateGetters.Rates.Models.Enums;

namespace RateGetters.Rates.Getters
{
    public record CbrRateGetter : IRateGetter
    {
        private const string CbrLinkForSingle = "http://www.cbr.ru/scripts/XML_daily.asp";
        private const string CbrLinkForPeriod = "http://www.cbr.ru/scripts/XML_dynamic.asp";

        public CbrRateGetter()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public RateForDate GetRate(DateTime dateTime, CurrencyCodesEnum code)
        {
            var ds = new DataSet();
            var dateInRequiredFormat = dateTime
                .ToShortDateString()
                .Replace('.', '/');
            ds.ReadXml($"{CbrLinkForSingle}?date_req={dateInRequiredFormat}");

            var currencyRows = ds.Tables["Valute"]?.Rows;
            if (currencyRows is null)
            {
                return new RateForDate(
                    new Rate(CurrencyCodesEnum.None, 0m),
                    DateTime.MinValue);
            }

            foreach (DataRow row in currencyRows)
            {
                if (row["CharCode"].ToString() != code.ToString().ToUpper())
                {
                    continue;
                }

                return new RateForDate(
                    new Rate(code, Convert.ToDecimal(row["Value"].ToString())),
                    dateTime);
            }

            return new RateForDate(
                new Rate(CurrencyCodesEnum.None, 0m),
                DateTime.MinValue);;
        }

        public PeriodRateList GetRatesForPeriod(DateTime first, DateTime second,
            CurrencyCodesEnum code)
        {
            var ds = new DataSet();
            var periodStartInRequiredFormat = (first < second ? first : second)
                .ToShortDateString()
                .Replace('.', '/');
            var periodEndInRequiredFormat = (first > second ? first : second)
                .ToShortDateString()
                .Replace('.', '/');

            var request = $"{CbrLinkForPeriod}" +
                          $"?date_req1={periodStartInRequiredFormat}" +
                          $"&date_req2={periodEndInRequiredFormat}" +
                          $"&VAL_NM_RQ={code.Description()}";
            ds.ReadXml(request);

            var currency = ds.Tables["Record"];
            return currency?.Rows is null
                ? new PeriodRateList(new List<RateForDate>())
                : PeriodRateList.Prepare(currency, code);
        }

        private static IEnumerable<RateForDate> PrepareRateList(DataTable currency, CurrencyCodesEnum code)
        {
            return currency.Rows
                .Cast<DataRow>()
                .Select(row => new RateForDate(
                    new Rate(code, Convert.ToDecimal(row["Value"])),
                    Convert.ToDateTime(row["Date"])));
        }
    }
}