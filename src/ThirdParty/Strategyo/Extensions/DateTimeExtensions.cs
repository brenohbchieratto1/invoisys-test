using System.Globalization;

namespace Strategyo.Extensions;

public static class DateTimeExtensions
{
    public static readonly DateTime Epoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    
    /// <summary>
    ///     Gets the first of a month from datetime.
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static DateTime GetFirstDayOfMonth(this DateTime date)
        => new DateTime(date.Year, date.Month, 1);
    
    /// <summary>
    ///     Gets the last of a month from datetime.
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static DateTime GetLastDayOfMonth(this DateTime date)
        => new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
    
    /// <summary>
    ///     Gets the 12:00:00 instance of a DateTime
    /// </summary>
    public static DateTime AbsoluteStart(this DateTime dateTime)
        => dateTime.Date;
    
    /// <summary>
    ///     Gets the 11:59:59 instance of a DateTime
    /// </summary>
    public static DateTime AbsoluteEnd(this DateTime dateTime)
        => AbsoluteStart(dateTime).AddDays(1).AddTicks(-1);
    
    /// <summary>
    ///     Sets the DateTime kind to UTC.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static DateTime SetKindToUtc(this DateTime data)
        => DateTime.SpecifyKind(data, DateTimeKind.Utc);
    
    /// <summary>
    ///     Sets the DateTime kind to Local.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static DateTime SetKindToLocal(this DateTime data)
        => DateTime.SpecifyKind(data, DateTimeKind.Local);
    
    /// <summary>
    ///     Gets date time from unix timestamp.
    /// </summary>
    /// <param name="unixTime"></param>
    /// <returns></returns>
    public static DateTime FromUnixTime(this double unixTime)
        => Epoch.AddSeconds(unixTime);
    
    public static DateTime ChangeTime(this DateTime dateTime, int hours, int minutes, int seconds, int milliseconds)
        => new(
            dateTime.Year,
            dateTime.Month,
            dateTime.Day,
            hours,
            minutes,
            seconds,
            milliseconds,
            dateTime.Kind);
    
    public static DateTime? ParseDateTime(this string dateTimeString, string format, CultureInfo? cultureInfo = null)
        => DateTime.TryParseExact(dateTimeString, format, cultureInfo ?? CultureInfoExtensions.PortugueseBrazilian,
                                  DateTimeStyles.None, out var dateTime)
            ? dateTime
            : null;
    
    public static DateTime? ParseDateTime(this string dateTimeString, CultureInfo? cultureInfo = null)
        => DateTime.TryParse(dateTimeString, cultureInfo ?? CultureInfoExtensions.PortugueseBrazilian,
                             DateTimeStyles.None, out var dateTime)
            ? dateTime
            : null;
    
    public static string DateTimeToString(this DateTime dateTime, string format, CultureInfo? cultureInfo = null) 
        => dateTime.ToString(format, cultureInfo ?? CultureInfoExtensions.PortugueseBrazilian);

    public static string DateTimeToString(this DateTime dateTime, CultureInfo? cultureInfo = null) 
        => dateTime.ToString(cultureInfo ?? CultureInfoExtensions.PortugueseBrazilian);

    public static TimeOnly ToTimeOnly(this DateTime dateTime) 
        => TimeOnly.FromDateTime(dateTime);

    public static DateOnly ToDateOnly(this DateTime dateTime) 
        => DateOnly.FromDateTime(dateTime);
}