using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Rubik.Service.Utils
{
    public static class CommonUtil
    {
        /// <summary>
        /// 获取拷贝名称
        /// </summary>
        /// <param name="originalName">原始名称</param>
        /// <param name="copyFlag">拷贝标识</param>
        /// <param name="isExists">判断新的名称是否存在</param>
        /// <returns>拷贝名</returns>
        public static string GetCopyName(string originalName, string copyFlag, Predicate<string> isExists)
        {
            if (string.IsNullOrWhiteSpace(originalName))
                throw new ArgumentNullException("originalName");

            if (isExists == null)
                return originalName.Trim();

            var copyName = originalName;
            int i = 0;
            while (isExists(copyName.Trim()))
            {
                i++;

                if (i == 1)
                    copyName += copyFlag;
                else
                {
                    if (Regex.IsMatch(copyName, copyFlag + @"\(\d+\)$"))
                        copyName = Regex.Replace(copyName, copyFlag + @"\(\d+\)$", copyFlag + "(" + i + ")");
                    else
                        copyName += ("(" + i + ")");
                }
            }

            return copyName.Trim();
        }

        #region Exception

        /// <summary>
        /// 移除换行
        /// </summary>
        /// <param name="originString">原始串</param>
        public static string RemoveLineBreak(string originString)
        {
            return Regex.Replace(originString, @"[\n\r]", "");
        }

        /// <summary>
        /// 异常格式化
        /// </summary>
        /// <param name="ex">异常</param>
        /// <returns>格式化字符串</returns>
        public static string GetExceptionStringFormat(Exception ex)
        {
            if (ex == null)
                return string.Empty;

            return string.Format("Message = {0}\nStackTrace = {1}\n\r", RemoveLineBreak(ex.Message), ex.StackTrace) + GetAllInnerException(ex);
        }

        /// <summary>
        /// 获取最后的内部异常
        /// </summary>
        /// <param name="ex">异常</param>
        /// <returns>内部异常</returns>
        public static Exception GetLastInnerException(Exception ex)
        {
            if (ex == null)
                return null;

            if (ex.InnerException == null)
                return ex;

            var tempEx = ex.InnerException;

            while (tempEx != null)
            {
                if (tempEx.InnerException == null)
                    break;

                tempEx = tempEx.InnerException;
            }

            return tempEx;
        }

        /// <summary>
        /// 获取所有内部异常
        /// </summary>
        /// <param name="ex">异常</param>
        /// <returns>格式化后的内部异常</returns>
        public static string GetAllInnerException(Exception ex)
        {
            if (ex.InnerException == null)
                return string.Empty;

            var tempEx = ex.InnerException;

            var stringBuilder = new StringBuilder();
            int i = 1;
            while (tempEx != null)
            {
                stringBuilder.AppendLine(string.Format("=================== InnerException[{0}] ===================", i++));
                stringBuilder.AppendLine(string.Format("Message = {0}\nStackTrace = {1}\n", RemoveLineBreak(tempEx.Message), tempEx.StackTrace));

                tempEx = tempEx.InnerException;
            }

            return stringBuilder.ToString();
        }

        #endregion
    }
}
