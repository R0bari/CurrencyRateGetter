using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace RateGetters.Rates.Models
{
    public record PeriodRateList : IEnumerable<RateForDate>
    {
        private readonly IEnumerable<RateForDate> _list;

        private PeriodRateList(IEnumerable<RateForDate> list) => _list = list;
        
        public static PeriodRateList Prepare(DataTable currency, CurrencyCodesEnum code)
        {
            return new(currency.Rows
                .Cast<DataRow>()
                .Select(row => new RateForDate(
                    new Rate(code, Convert.ToDecimal(row["Value"])),
                    Convert.ToDateTime(row["Date"]))));
        }

        public IEnumerator<RateForDate> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}