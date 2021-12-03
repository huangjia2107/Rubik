using System;

namespace Rubik.Toolkit.Utils
{
    public static class DateTimeUtil
    {
        /// <summary>
        /// 时间戳转时间
        /// </summary>
        /// <param name="timestamp">时间戳</param>
        /// <returns>时间</returns>
        public static DateTime TimestampToDateTime(long timestamp)
        {
            var startTime = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1, 8, 0, 0), TimeZoneInfo.Local);
            return startTime.Add(new TimeSpan(long.Parse($"{timestamp}0000")));
        }

        /// <summary>
        /// 时间转时间戳
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>时间戳</returns>
        public static long DateTimeToTimestamp(this DateTime time)
        {
            var startTime = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1, 8, 0, 0), TimeZoneInfo.Local);
            return (long)(time - startTime).TotalMilliseconds;
        }
    }
}
