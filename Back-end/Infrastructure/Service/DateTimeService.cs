using System;
using System.Globalization;
using Application.Common.Interface;

namespace Infrastructure.Service;

public class DateTimeService : IDateTimeService
{
    public DateTime Now => DateTime.Now;
    public string ConvertGergToHijri(DateTime date)
    {
        UmAlQuraCalendar hijriCalendar = new UmAlQuraCalendar();

        int hijriYear = hijriCalendar.GetYear(date);
        int hijriMonth = hijriCalendar.GetMonth(date);
        int hijriDay = hijriCalendar.GetDayOfMonth(date);
        return $"{hijriYear}-{hijriMonth}-{hijriDay}";
    }
    public long ConvertGergToHijriNumber(DateTime date)
    {
        UmAlQuraCalendar hijriCalendar = new UmAlQuraCalendar();

        int hijriYear = hijriCalendar.GetYear(date);
        int hijriMonth = hijriCalendar.GetMonth(date);
        int hijriDay = hijriCalendar.GetDayOfMonth(date);
        return long.Parse($"{hijriYear}{hijriMonth}{hijriDay}");
    }

    public string ConvertGergToHijriWithTime(DateTime date)
    {
        UmAlQuraCalendar hijriCalendar = new UmAlQuraCalendar();

        int hijriYear = hijriCalendar.GetYear(date);
        int hijriMonth = hijriCalendar.GetMonth(date);
        int hijriDay = hijriCalendar.GetDayOfMonth(date);

        // Get the Hijri hours, minutes, and seconds
        int hijriHours = hijriCalendar.GetHour(date);
        int hijriMinutes = hijriCalendar.GetMinute(date);
        int hijriSeconds = hijriCalendar.GetSecond(date);

        string formattedDate = $"{hijriYear}-{hijriMonth}-{hijriDay} {hijriHours}:{hijriMinutes}:{hijriSeconds}";

        return formattedDate;
    }

    public string DateTimeToString(DateTime? dateTime, string format = "yyyy-MM-dd HH:mm:ss")
    {
        // Check if the dateTime has a value (i.e., it's not null)
        if (dateTime.HasValue)
        {
            // Return the DateTime value formatted as a string
            return dateTime.Value.ToString(format); // Adjust format as needed
        }
        else
        {
            // Return an empty string if dateTime is null
            return string.Empty;
        }
    }

    public TimeSpan? ParseTimeString(string timeString, string timeDayFormat = "hh:mm tt")
    {
        DateTime parsedDateTime;
        if (DateTime.TryParseExact(timeString, timeDayFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDateTime))
        {
            return parsedDateTime.TimeOfDay;
        }
        else
        {
            return null;
        }
    }

    public TimeSpan? ParseTimeString(string timeString)
    {
        DateTime parsedDateTime;
        var formats = new[] { "HH:mm", "hh:mm tt" }; // Try both 24-hour and 12-hour formats
        if (DateTime.TryParseExact(timeString, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDateTime))
        {
            return parsedDateTime.TimeOfDay;
        }

        return null;
    }


    public bool IsTimeStringValid(string timeString, string timeDayFormat = "hh:mm tt")
    {
        DateTime parsedDateTime;
        return DateTime.TryParseExact(timeString, timeDayFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDateTime);
    }

    public bool IsStringDateParsable(string dateString)
    {
        if (string.IsNullOrWhiteSpace(dateString))
            return false;
        DateTime result;
        return DateTime.TryParse(dateString, out result);
    }
    
    public bool IsFromLessThanToPeriod(DateTime fromDate, DateTime toDate)
    {
        return fromDate < toDate;
    }

    public bool IsFromLessThanToPeriod(DateOnly fromDate, DateOnly toDate)
    {
        return fromDate < toDate;
    }


    public DateTime? GetDateFromString(string dateString)
    {
        if (!IsStringDateParsable(dateString))
            return null;
        return DateTime.Parse(dateString).Date;
    }

    public DateOnly? GetDateOnlyFromString(string dateString)
    {
        if (!IsStringDateParsable(dateString))
            return null;
        return DateOnly.FromDateTime(DateTime.Parse(dateString).Date);
    }
}

