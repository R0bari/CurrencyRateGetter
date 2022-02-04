﻿using System;
using RateGetters.Rates.Getters;
using RateGetters.Rates.Models;
using RateGetters.Rates.Models.Enums;

namespace RateGetters.Rates.Interfaces
{
    public interface ISingleRateGetter
    {
        public RateForDate GetRate(DateTime dateTime, CurrencyCodesEnum code);
    }
}