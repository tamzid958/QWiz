using System.Text;
using System.Text.RegularExpressions;
using static System.Text.RegularExpressions.RegexOptions;

namespace QWiz.Helpers.Extensions;

public static class StringExtension
{
    public static string UrlEncoding(this string str)
    {
        var tempBytes = Encoding.GetEncoding("UTF-8").GetBytes(str);
        var tempStr = Encoding.UTF8.GetString(tempBytes);
        tempStr = Regex.Replace(tempStr, "[^a-zA-Z0-9_.]+", " ", Compiled);
        tempStr = Regex.Replace(tempStr, @"\s+", " ");
        tempStr = Regex.Replace(tempStr, @"\s", "-");
        return tempStr.ToLower();
    }

    public static string ToSnakeCase(this string input)
    {
        if (string.IsNullOrEmpty(input)) return input;

        var startUnderscores = Regex.Match(input, @"^_+");
        return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
    }

    public static string ToCamelCase(this string str)
    {
        var words = str.Split(["_", " "], StringSplitOptions.RemoveEmptyEntries);

        var leadWord = Regex.Replace(words[0], @"([A-Z])([A-Z]+|[a-z0-9]+)($|[A-Z]\w*)",
            m => m.Groups[1].Value.ToLower() + m.Groups[2].Value.ToLower() + m.Groups[3].Value);

        var tailWords = words.Skip(1)
            .Select(word => char.ToUpper(word[0]) + word[1..])
            .ToArray();

        return $"{leadWord}{string.Join(string.Empty, tailWords)}";
    }

    public static string ToTitleCase(this string str)
    {
        return Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
    }

    public static bool Search(this string destination, string? term)
    {
        return string.IsNullOrEmpty(term) || destination.Contains(term, StringComparison.CurrentCultureIgnoreCase);
    }

    public static string ConvertToBase64(this IFormFile file)
    {
        using var ms = new MemoryStream();
        file.CopyTo(ms);
        var fileBytes = ms.ToArray();
        return Convert.ToBase64String(fileBytes);
    }
}