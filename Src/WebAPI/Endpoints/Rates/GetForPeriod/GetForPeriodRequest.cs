using System;
using RateGetters.Rates.Models.Enums;

namespace WebAPI.Endpoints.Rates.GetForPeriod
{
    public class GetForPeriodRequest
    {
        public CurrencyCodesEnum Code { get; set; }
        public DateTime FirstDate { get; set; }
        public DateTime SecondDate { get; set; }
        
        public GetForPeriodRequest() {}

        public GetForPeriodRequest(CurrencyCodesEnum code, DateTime firstDate, DateTime secondDate) =>
            (Code, FirstDate, SecondDate) = (code, firstDate, secondDate);
    }
}