using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RateGetters.Rates.Models.Enums;

namespace Mongo.Models;

public record RateForDate
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public CurrencyCodesEnum Code { get; set; }
    public decimal Value { get; set; }

    public DateTime Date { get; set; }

    public RateForDate()
    {
    }

    public RateForDate(CurrencyCodesEnum code, decimal value, DateTime date)
    {
        Code = code;
        Value = value;
        Date = date;
    }
}
