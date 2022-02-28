using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Domain.Models.Rates.Enums;
using Microsoft.VisualBasic;

namespace Domain.Models.Rates;

public class PeriodRateList : IEnumerable<RateForDate>
{
    private readonly IEnumerable<RateForDate> _list;

    public PeriodRateList(IEnumerable<RateForDate> list) => _list = list;

    public static PeriodRateList Empty => new(new List<RateForDate>());
        
    public static PeriodRateList Prepare(DataTable currency, CurrencyCodesEnum code) =>
        new(currency.Rows
            .Cast<DataRow>()
            .Select(row => new RateForDate(
                code,
                Convert.ToDecimal(row["Value"]),
                Convert.ToDateTime(row["Date"])))
            .ToList());


    public override string ToString() =>
        Strings.Join(
            _list.Select(el => el.ToString())
                .ToArray(),
            "\n");

    public override bool Equals(object obj) =>
        (obj is PeriodRateList) is not false
        &&
        Equals((PeriodRateList) obj);

    private bool Equals(PeriodRateList other)
    {
        var thisList = _list.ToList();
        var otherList = other.ToList();
        return thisList.Count == otherList.Count
               &&
               Enumerable
                   .Range(0, thisList.Count)
                   .All(index => thisList[index] == otherList[index]);
    }

    public override int GetHashCode() => _list != null ? _list.GetHashCode() : 0;

    public IEnumerator<RateForDate> GetEnumerator() => _list.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
