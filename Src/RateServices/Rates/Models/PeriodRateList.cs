using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.VisualBasic;
using RateGetters.Rates.Models.Enums;

namespace RateGetters.Rates.Models
{
    public class PeriodRateList : IEnumerable<RateForDate>
    {
        private readonly IEnumerable<RateForDate> _list;

        public PeriodRateList(IEnumerable<RateForDate> list) => _list = list;

        public static PeriodRateList Prepare(DataTable currency, CurrencyCodesEnum code) =>
            new(currency.Rows
                .Cast<DataRow>()
                .Select(row => new RateForDate(
                    new Rate(code, Convert.ToDecimal(row["Value"])),
                    Convert.ToDateTime(row["Date"]))));

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
            var list = _list.ToList();
            var secondList = other._list.ToList();
            return list.Count == secondList.Count
                   &&
                   Enumerable
                       .Range(0, list.Count)
                       .All(index => list[index] == secondList[index]);
        }

        public override int GetHashCode() => _list != null ? _list.GetHashCode() : 0;

        public IEnumerator<RateForDate> GetEnumerator() => _list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}