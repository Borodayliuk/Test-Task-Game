using System;

namespace Services.DailyBonus
{
    public static class UtilityDateTime
    {
        public static int GetDayInSeason(DateTime date)
        {
            var season = GetSeason(date);
            var months = GetMonthsBySeason(season);

            var elapsedDays = 0;
            foreach (var monthIndex in months)
            {
                if (monthIndex == date.Month)
                {
                    elapsedDays += date.Day;
                    break;
                }

                elapsedDays += DateTime.DaysInMonth(date.Year, monthIndex);
            }

            return elapsedDays;
        }

        private static Season GetSeason(DateTime date) =>
            date switch
            {
                _ when date.Month == 12 || date.Month == 1 || date.Month == 2 => Season.Winter,
                _ when date.Month == 3 || date.Month == 4 || date.Month == 5 => Season.Spring,
                _ when date.Month == 6 || date.Month == 7 || date.Month == 8 => Season.Summer,
                _ when date.Month == 9 || date.Month == 10 || date.Month == 11 => Season.Autumn,
                _ => Season.Autumn
            };

        private static int[] GetMonthsBySeason(Season season) =>
            season switch
            {
                Season.Winter => new[] { 12, 1, 2 },
                Season.Spring => new[] { 3, 4, 5 },
                Season.Summer => new[] { 6, 7, 8 },
                Season.Autumn => new[] { 9, 10, 11 },
                _ => Array.Empty<int>()
            };

        private enum Season
        {
            Winter = 0,
            Spring = 1,
            Summer = 2,
            Autumn = 3
        }
    }
}