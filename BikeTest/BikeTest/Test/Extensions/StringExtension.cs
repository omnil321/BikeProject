namespace BikeTest.Test.Extensions
{
    using System;

    public static class StringExtension
    {
        public static string UniqueSuffix(this string name)
        {
            return string.Format("{0} - [{1}]", name, Guid.NewGuid().ToString());
        }

        public static string GetCurrentDate()
        {
            var cDate = DateTime.Now;
            var currentDate = cDate.ToString($"{cDate:dd-MM-yyyy}");
            return currentDate;
        }

        public static string GetCurrentDateTimeWithMinutes()
        {
            var cDate = DateTime.Now;
            var currentDateTime = cDate.ToString($"{cDate:dd-MM_HH-mm}");
            return currentDateTime;
        }

        public static string GetCurrentDateTimeWithSeconds()
        {
            var cDate = DateTime.Now;
            var currentDateTime = cDate.ToString($"{cDate:dd-MM_yyyy_HH-mm-ss}");
            return currentDateTime;
        }

        public static string GetTomorrowDate()
        {
            var cDate = DateTime.Now.AddDays(1);
            var tomorrowDate = cDate.ToString($"{cDate:dd-MM-yyyy}");
            return tomorrowDate;
        }
    }
}
