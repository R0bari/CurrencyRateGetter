using System;
using RateGetters.Rates.Models.Enums;

namespace CommandLayer.Queries.Rates.GetForPeriodQuery
{
    public class GetForPeriodSpecification
    {
        public CurrencyCodesEnum Code { get; set; }
        public DateTime FirstDate { get; set; }
        public DateTime SecondDate { get; set; }
        
        public GetForPeriodSpecification() {}

        public GetForPeriodSpecification(CurrencyCodesEnum code, DateTime firstDate, DateTime secondDate) =>
            (Code, FirstDate, SecondDate) = (code, firstDate, secondDate);
    }
}