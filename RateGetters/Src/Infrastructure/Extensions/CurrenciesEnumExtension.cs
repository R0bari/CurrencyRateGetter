using System;
using System.ComponentModel;

namespace RateGetters.Infrastructure.Extensions
{
    public static class CurrenciesEnumExtension
    {
        public static string Description<T>(this T enumValue) 
            where T : struct, IConvertible
        {
            var description = enumValue.ToString();
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString() ?? string.Empty);
            
            var attrs = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
            if (attrs.Length > 0)
            {
                description = ((DescriptionAttribute)attrs[0]).Description;
            }

            return description;
        }
    }
}