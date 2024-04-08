namespace QWiz.Helpers.Extensions;

public static class NumberExtension
{
    public static bool Range<T>(this T number, T min, T max) where T : IComparable<T>

    {
        return number.CompareTo(min) > 0 &&
               number.CompareTo(max) < 0;
    }

    private static T MinValue<T>(this Type self)
    {
        return (T)self.GetField(nameof(MinValue))?.GetRawConstantValue()!;
    }

    private static T MaxValue<T>(this Type self)
    {
        return (T)self.GetField(nameof(MaxValue))?.GetRawConstantValue()!;
    }
}