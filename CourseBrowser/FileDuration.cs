using System;

namespace CourseBrowser
{
    public class FileDuration
    {
        public string FileName { get; set; }
        public string Duration => $"{DurationSpan.Minutes.ToString("00")}:{DurationSpan.Seconds.ToString("00")}";
        public TimeSpan DurationSpan { get; set; }
        public bool IsDone { get; set; }
    }
}
