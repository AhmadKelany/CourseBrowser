using System;
using System.Collections.Generic;
using System.Text;

namespace CourseBrowser
{
    public static class Extensions
    {
        public static string ToHourMinuteString(this TimeSpan s) => $"{((int)s.TotalHours).ToString("00")}:{s.Minutes.ToString("00")}";
    }
}
