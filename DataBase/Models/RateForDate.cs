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
    public DateTime DateTime { get; set; }
    
    public RateForDate() {}
    
    public RateForDate(CurrencyCodesEnum code, decimal value, DateTime dateTime)
    {
        Code = code;
        Value = value;
        DateTime = dateTime.Date;
    }
}
