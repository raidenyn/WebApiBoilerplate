using System;

namespace WebApiBoilerplate.WebApi.Helpers
{
    public static class CopyrightInfo
    {
        public static string DateRange(int startYear)
        {
            var current = DateTime.Now.Year;
            if (current == startYear)
            {
                return startYear.ToString();
            }

            return $"{startYear}&ndash;{current}";
        }
    }
}
