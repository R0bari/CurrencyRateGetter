using System;
using Domain.Contexts;
using Domain.Models.Rates;
using Domain.Models.Rates.Enums;
using Mongo.Contexts;
using Xunit;

namespace Database.Tests;

public class MongoRateTests
{
    private readonly IContext _context = new MongoContext();
    
    [Fact]
    public async void TestRates()
    {
        var expected = new RateForDate(
            CurrencyCodesEnum.Usd,
            75.7619m,
            DateTime.Today.AddDays(1));

        var insertResult = await _context
            .InsertRateForDate(expected)
            .ConfigureAwait(false);
        Assert.True(insertResult > 0);

        var actual = await _context
            .GetRateForDate(expected.Code, expected.Date)
            .ConfigureAwait(false);
        Assert.Equal(expected, actual);
        
        var deletionResult = await _context
            .DeleteRate(actual)
            .ConfigureAwait(false);
        Assert.True(deletionResult > 0);
    }
}
