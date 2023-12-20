using System;

namespace Services.DailyBonus
{
    public static class UtilityDate
    {
        public static int GetDayInSeason(DateTime date)
        {
            const int februaryNumber = 2;
            const int decemberNumber = 12;
            const int springDays = 92;
            const int summerDays = 92;
            const int winterDays = 62;
            const int decemberDays = 31;

            var month = date.Month;
            var day = date.Day;
            var year = date.Year;
            var daysInFebruary = DateTime.DaysInMonth(year, februaryNumber);
            var originalDay = decemberDays + day;

            if (month == decemberNumber)
                return day;

            for (var i = 1; i < month; i++)
                originalDay += DateTime.DaysInMonth(year, i);

            var winterOriginalDays = daysInFebruary + winterDays;
            var springOriginalDays = winterOriginalDays + springDays;
            var summerOriginalDays = springOriginalDays + summerDays;

            if (originalDay <= winterOriginalDays)
                return originalDay;

            if (originalDay <= springOriginalDays)
                return originalDay - winterOriginalDays;

            if (originalDay <= summerOriginalDays)
                return originalDay - springOriginalDays;

            return originalDay - summerOriginalDays;
        }
    }
}