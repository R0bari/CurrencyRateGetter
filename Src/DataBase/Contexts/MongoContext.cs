using Mongo.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using RateGetters.Rates.Models.Enums;

namespace Mongo.Contexts;

public class MongoContext : IContext
{
    private readonly IGridFSBucket _gridFs;
    private readonly IMongoCollection<RateForDate> _ratesForDate;
    private const string ConnectionString = "mongodb://localhost:27017/CurrencyView?readPreference=primary&appname=MongoDB%20Compass&directConnection=true&ssl=false";

    public MongoContext()
    {
        var connection = new MongoUrlBuilder(ConnectionString);
        var client = new MongoClient(ConnectionString);
        var database = client.GetDatabase(connection.DatabaseName);
        _gridFs = new GridFSBucket(database);
        _ratesForDate = database.GetCollection<RateForDate>("RatesForDate");
    }

    public async Task<RateForDate> GetRateForDate(CurrencyCodesEnum code, DateTime date)
    {
        var filter =
            Builders<RateForDate>.Filter.Eq(r => r.Code, code)
            &
            Builders<RateForDate>.Filter.Eq(r => r.Date, date);
        var result = await _ratesForDate
            .Find(filter)
            .Limit(1)
            .FirstOrDefaultAsync()
            .ConfigureAwait(false);
        return result with {Date = result.Date.ToLocalTime()};
    }

    public async Task<DateTime> GetMostRecentDate()
    {
        var result = await _ratesForDate
            .Find(new BsonDocument())
            .SortByDescending(r => r.Date)
            .FirstOrDefaultAsync()
            .ConfigureAwait(false);
        return result?.Date.ToLocalTime() ?? new DateTime(1999, 12, 31);
    }

    public async Task<int> InsertRateForDate(RateForDate rateForDate)
    {
        var result = await _ratesForDate
            .BulkWriteAsync(new WriteModel<RateForDate>[]
            {
                new InsertOneModel<RateForDate>(rateForDate)
            })
            .ConfigureAwait(false);
        
        return result.IsAcknowledged ? 1 : -1;
    }

    public async Task<int> InsertRateForDateList(IEnumerable<RateForDate> ratesForDate)
    {
        var ratesList = ratesForDate.ToArray();
        if (!ratesList.Any())
        {
            return -1;
        }
        var models = ratesList
            .Select(rate => new InsertOneModel<RateForDate>(rate));
        var result = await _ratesForDate
            .BulkWriteAsync(models)
            .ConfigureAwait(false);
        
        return result.IsAcknowledged ? 1 : -1;
    }

    public async Task<int> DeleteRateById(string id)
    {
        var filter = Builders<RateForDate>.Filter.Eq("_id", new ObjectId(id));
        var result = await _ratesForDate
            .DeleteOneAsync(filter)
            .ConfigureAwait(false);

        return result.IsAcknowledged ? 1 : -1;
    }

    public async Task<int> DeleteAllRates(bool confirm = false)
    {
        if (!confirm)
        {
            return -1;
        }
        
        var filter = Builders<RateForDate>.Filter.Empty;
        var deletionResult = await _ratesForDate.DeleteManyAsync(filter);
        return deletionResult.IsAcknowledged ? 1 : -1;
    }
}
