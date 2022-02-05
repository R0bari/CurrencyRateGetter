using System;
using RateGetters.Rates.Models.Enums;

namespace CommandLayer.Queries.Rates.GetForDateQuery
{
    
    public record GetForDateSpecification
    {
        public CurrencyCodesEnum Code { get; set; }
        public DateTime DateTime { get; set; }
        
        public GetForDateSpecification() {}
        public GetForDateSpecification(DateTime dateTime, CurrencyCodesEnum code) => (DateTime, Code) = (dateTime, code);

    }
}