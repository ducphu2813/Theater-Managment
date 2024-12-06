namespace MovieService.Infrastructure.Helper;

public static class TimeZoneHelper
{
    private static readonly TimeZoneInfo _timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Asia/Ho_Chi_Minh");

    public static DateTime ConvertToTimeZone(DateTime dateTime)
    {
        return TimeZoneInfo.ConvertTime(dateTime, _timeZoneInfo);
    }

    public static DateTime ConvertToUtc(DateTime dateTime)
    {
        return TimeZoneInfo.ConvertTimeToUtc(dateTime, _timeZoneInfo);
    }
}