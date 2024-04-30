namespace QWiz.Helpers.Extensions;

public static class DateTimeExtension
{
    public static DateTime ConvertToSpecificTimeZone(
        this DateTime dateTime,
        string timeZoneId = "Bangladesh Standard Time"
    )
    {
        // Get the Dhaka time zone
        var dhakaTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

        // Convert UTC time to Dhaka time
        var dhakaTime = TimeZoneInfo.ConvertTimeFromUtc(dateTime, dhakaTimeZone);

        return dhakaTime;
    }

    public static DateTime ConvertUnixTimeStampToDateTime(long unixTimestamp)
    {
        var dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp);
        return dateTimeOffset.UtcDateTime;
    }

    public static long GetCurrentUnixTimestamp()
    {
        // Get the current time in UTC
        var currentTime = DateTime.UtcNow;

        // Calculate the Unix timestamp
        var unixTimestamp = (long) (currentTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;

        return unixTimestamp;
    }

    public static long AddDaysToUnixTimestamp(long unixTimestamp, int days)
    {
        // Calculate the number of seconds in the specified number of days
        long secondsInDays = days * 24 * 60 * 60;

        // Add the seconds to the Unix timestamp
        return unixTimestamp + secondsInDays;
    }
}