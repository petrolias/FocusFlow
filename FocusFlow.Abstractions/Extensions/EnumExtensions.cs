namespace FocusFlow.Abstractions.Extensions
{
    public static class EnumExtensions
    {
        private static readonly Random _random = new();

        public static TEnum GetRandomValue<TEnum>() where TEnum : struct, Enum
        {
            var values = Enum.GetValues<TEnum>();
            return values[_random.Next(values.Length)];
        }
    }
}