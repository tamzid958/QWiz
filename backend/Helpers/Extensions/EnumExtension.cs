namespace QWiz.Helpers.Extensions;

public static class EnumExtension
{
    public static bool Search(this Enum destination, Enum? search)
    {
        return search == null && Equals(destination, search);
    }
}