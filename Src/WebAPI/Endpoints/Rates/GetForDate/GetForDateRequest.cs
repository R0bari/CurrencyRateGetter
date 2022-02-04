using System;
using RateGetters.Rates.Models.Enums;

namespace WebAPI.Endpoints.Rates.GetForDate
{
    
    public class GetForDateRequest
    {
        public CurrencyCodesEnum Code { get; set; }
        public DateTime DateTime { get; set; }
        
        public GetForDateRequest() {}

        public GetForDateRequest(DateTime dateTime, CurrencyCodesEnum code) => (DateTime, Code) = (dateTime, code);

    }
}