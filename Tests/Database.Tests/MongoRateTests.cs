using System;
using Mongo.Contexts;
using Mongo.Models;
using RateGetters.Rates.Models.Enums;
using Xunit;

namespace Database.Tests;

public class MongoRateTests
{
    private readonly IContext _context = new MongoContext();
    
    [Fact]
    public async void TestRates()
    {
        var expected = new RateForDate(CurrencyCodesEnum.Usd, 75, DateTime.Today);

        var insertResult = await _context
            .InsertRateForDate(expected)
            .ConfigureAwait(false);
        Assert.True(insertResult > 0);

        var actual = await _context
            .GetRateForDate(expected.Code, expected.DateTime)
            .ConfigureAwait(false);

        Assert.Equal(expected, actual);
        
        var deletionResult = await _context
            .DeleteRateForDate(actual.Id)
            .ConfigureAwait(false);
        Assert.True(deletionResult > 0);
        
    }
}