using System;
using System.ComponentModel;

namespace RateGetters.Extensions
{
    public static class CurrenciesEnumExtension
    {
        public static string Description<T>(this T enumValue) 
            where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                return null;
            }

            var description = enumValue.ToString();
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString() ?? string.Empty);

            if (fieldInfo == null)
            {
                return description;
            }
            
            var attrs = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
            if (attrs.Length > 0)
            {
                description = ((DescriptionAttribute)attrs[0]).Description;
            }

            return description;
        }
    }
}