using System;
using System.Globalization;
using Domain.Enum;

namespace Application.Common.Service;

public static class CultureInfoService
{
    public static int CurrentLanguageCode()
    {
        var lang = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

        switch (lang)
        {
            case "ar":
                return (int)LanguageEnum.Arabic;
            case "en":
                return (int)LanguageEnum.English;
            default:
                return (int)LanguageEnum.English;
        }
    }
}