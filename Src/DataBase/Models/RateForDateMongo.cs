using Domain.Models.Rates.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Mongo.Models;

public record RateForDateMongo
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public CurrencyCodesEnum Code { get; set; }
    public decimal Value { get; set; }

    public DateTime Date { get; set; }

    public RateForDateMongo()
    {
    }

    public RateForDateMongo(CurrencyCodesEnum code, decimal value, DateTime date)
    {
        Code = code;
        Value = value;
        Date = date;
    }
}
