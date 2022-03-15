using System;

namespace DataBase.Extensions;

public static class DateTimeExtensions
{
    public static DateTime PrepareForMongo(this DateTime dateTime) =>
        dateTime == dateTime.ToUniversalTime()
            ? dateTime - (DateTime.Now - DateTime.UtcNow)
            : dateTime;
}
