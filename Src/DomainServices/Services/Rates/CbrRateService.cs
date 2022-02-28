using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Infrastructure.Extensions;
using Domain.Models.Rates;
using Domain.Models.Rates.Enums;
using DomainServices.Services.Rates.Interfaces;

namespace DomainServices.Services.Rates;

public record CbrRateService : IRateService
{
    private const string CbrLinkForSingle = "http://www.cbr.ru/scripts/XML_daily.asp";
    private const string CbrLinkForPeriod = "http://www.cbr.ru/scripts/XML_dynamic.asp";

    public CbrRateService()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    public async Task<RateForDate> GetRateAsync(DateTime date, CurrencyCodesEnum code)
    {
        if (code == CurrencyCodesEnum.Rub)
        {
            return new RateForDate(
                CurrencyCodesEnum.Rub,
                1,
                date);
        }

        var currencyRows = ReadTableFromXml(
                $"{CbrLinkForSingle}?date_req={date:dd/MM/yyyy}",
                "Valute")
            ?.Rows;
        
        if (currencyRows is null)
        {
            return await Task.FromResult(RateForDate.Empty);
        }

        var requiredRow = currencyRows
            .Cast<DataRow>()
            .FirstOrDefault(row => row["CharCode"].ToString() == code.ToString().ToUpper());

        return await Task.FromResult(
            requiredRow is not null
                ? new RateForDate(
                    code,
                    Convert.ToDecimal(requiredRow["Value"].ToString()),
                    date)
                : RateForDate.Empty);
    }

    public async Task<PeriodRateList> GetRatesForPeriodAsync(DateTime first, DateTime second,
        CurrencyCodesEnum code) =>
        first == second
            ? new PeriodRateList(new List<RateForDate>
            {
                await GetRateAsync(first, code)
                    .ConfigureAwait(false)
            })
            : await ReadRatesForDateFromXml(first, second, code);


    private static async Task<PeriodRateList> ReadRatesForDateFromXml(DateTime first, DateTime second,
        CurrencyCodesEnum code)
    {
        var currency = ReadTableFromXml(
            $"{CbrLinkForPeriod}" +
            $"?date_req1={(first < second ? first : second):dd/MM/yyyy}" +
            $"&date_req2={(first > second ? first : second):dd/MM/yyyy}" +
            $"&VAL_NM_RQ={code.Description()}",
            "Record");
        return await Task.FromResult(currency?.Rows is null
            ? PeriodRateList.Empty
            : PeriodRateList.Prepare(currency, code));
    }

    private static DataTable ReadTableFromXml(string fileName, string tableName)
    {
        var ds = new DataSet();
        ds.ReadXml(fileName);
        return ds.Tables[tableName];
    }
}