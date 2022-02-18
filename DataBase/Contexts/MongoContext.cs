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

    public async Task<RateForDate> GetRateForDate(CurrencyCodesEnum code, DateTime dateTime)
    {
        var filter = 
            Builders<RateForDate>.Filter.Eq("Code", code)
            &
            Builders<RateForDate>.Filter.Eq("DateTime", dateTime.Date);
        var result = await _ratesForDate
            .Find(filter)
            .Limit(1)
            .FirstOrDefaultAsync()
            .ConfigureAwait(false);
        return result with {DateTime = result.DateTime.ToLocalTime()};
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

    public async Task<int> DeleteRateForDate(string id)
    {
        var filter = Builders<RateForDate>.Filter.Eq("_id", new ObjectId(id));
        var result = await _ratesForDate.DeleteOneAsync(filter);

        return result.IsAcknowledged ? 1 : -1;
    }
}
