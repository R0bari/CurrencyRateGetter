﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using RateGetters.Infrastructure.Extensions;
using RateGetters.Rates.Models;
using RateGetters.Rates.Models.Enums;
using RateGetters.Rates.Services.Interfaces;

namespace RateGetters.Rates.Services
{
    public record CbrRateService : IRateService
    {
        private const string CbrLinkForSingle = "http://www.cbr.ru/scripts/XML_daily.asp";
        private const string CbrLinkForPeriod = "http://www.cbr.ru/scripts/XML_dynamic.asp";

        public CbrRateService()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public RateForDate GetRate(DateTime dateTime, CurrencyCodesEnum code)
        {
            var ds = new DataSet();
            ds.ReadXml($"{CbrLinkForSingle}?date_req={dateTime:dd/MM/yyyy}");

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
                DateTime.MinValue);
        }

        public PeriodRateList GetRatesForPeriod(DateTime first, DateTime second,
            CurrencyCodesEnum code)
        {
            var ds = new DataSet();

            ds.ReadXml($"{CbrLinkForPeriod}" +
                       $"?date_req1={(first < second ? first : second):dd/MM/yyyy}" +
                       $"&date_req2={(first > second ? first : second):dd/MM/yyyy}" +
                       $"&VAL_NM_RQ={code.Description()}");

            var currency = ds.Tables["Record"];
            return currency?.Rows is null
                ? new PeriodRateList(new List<RateForDate>())
                : PeriodRateList.Prepare(currency, code);
        }
    }
}