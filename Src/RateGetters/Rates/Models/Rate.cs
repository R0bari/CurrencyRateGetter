﻿using System.ComponentModel;
using RateGetters.Rates.Models.Enums;

namespace RateGetters.Rates.Models
{
    public record Rate
    {
        public CurrencyCodesEnum Code { get; }
        public decimal Value { get; }

            public Rate(CurrencyCodesEnum code, decimal value) => (Code, Value) = (code, value);
        
        public override string ToString() => $"Currency \"{Code.ToString()}\" is {Value}.";
    }
    

}