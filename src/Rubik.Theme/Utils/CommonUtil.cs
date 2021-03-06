using System;
using System.IO;
using SWF = System.Windows.Forms;

namespace Rubik.Theme.Utils
{
    public static class CommonUtil
    {
        #region Dialog

        public static string ShowSaveFileDialog(string fileName, string filter)
        {
            var sfd = new SWF.SaveFileDialog { Filter = filter, FileName = Path.GetFileNameWithoutExtension(fileName) };
            if (sfd.ShowDialog() == SWF.DialogResult.OK)
                return sfd.FileName;

            return null;
        }

        public static string ShowOpenFileDialog(string filter)
        {
            var ofd = new SWF.OpenFileDialog { Filter = filter };
            if (ofd.ShowDialog() == SWF.DialogResult.OK)
                return ofd.FileName;

            return null;
        }

        #endregion

        #region Timestamp

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
        public static long DateTimeToTimestamp(DateTime time)
        {
            var startTime = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1, 8, 0, 0), TimeZoneInfo.Local);
            return (long)(time - startTime).TotalMilliseconds;
        }

        #endregion
    }
}