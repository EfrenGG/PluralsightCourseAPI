using System;

namespace PluralsightCourseAPI.Helpers
{
    public static class DateTimeOffsetExtensions
    {
        public static int GetCurrentAge(this DateTimeOffset dateOfBirth)
        {
            DateTime today = DateTime.Now;
            int age = today.Year - dateOfBirth.Year;

            if (today < dateOfBirth.AddYears(age))
            {
                age--;
            }

            return age;
        }
    }
}