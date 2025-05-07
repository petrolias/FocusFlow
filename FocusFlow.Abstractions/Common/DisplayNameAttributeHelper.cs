using System.ComponentModel.DataAnnotations;
using System.Reflection;
namespace FocusFlow.Abstractions.Common
{
    internal class DisplayNameAttributeHelper
    {
        public static string GetDisplayName<TEnum>(TEnum value) where TEnum : struct
        {
            return typeof(TEnum)
                .GetMember(value.ToString())[0]
                .GetCustomAttribute<DisplayAttribute>()?.Name
                ?? value.ToString();
        }
    }
}
