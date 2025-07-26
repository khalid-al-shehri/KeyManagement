using System;

namespace Application.Common.Interface;

public interface IDateTimeService
{
    DateTime Now { get; }
    string ConvertGergToHijri(DateTime date);
    long ConvertGergToHijriNumber(DateTime date);
    string ConvertGergToHijriWithTime(DateTime date);
    string DateTimeToString(DateTime? dateTime, string format = "yyyy-MM-dd HH:mm:ss");
    TimeSpan? ParseTimeString(string timeString, string timeDayFormat = "hh:mm tt");
    bool IsTimeStringValid(string timeString, string timeDayFormat = "hh:mm tt");
    bool IsStringDateParsable(string dateString);
    bool IsFromLessThanToPeriod(DateTime fromDate, DateTime toDate);
    DateTime? GetDateFromString(string dateString);
    DateOnly? GetDateOnlyFromString(string dateString);
    bool IsFromLessThanToPeriod(DateOnly fromDate, DateOnly toDate);
    TimeSpan? ParseTimeString(string timeString);
}
