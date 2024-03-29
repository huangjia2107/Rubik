﻿using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.Security.Cryptography;

namespace Rubik.Toolkit.IO
{
    public static class FileUtil
    {
        public static bool IsDirectory(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return false;

            return (File.GetAttributes(filePath) & FileAttributes.Directory) != 0;
        }

        public static bool CanAccess(string file, bool ignoreZeroSize = false)
        {
            if (file == null || string.IsNullOrEmpty(file.Trim()) || !File.Exists(file))
                throw new FileNotFoundException("The file is invalid. File = " + file);

            if (!ignoreZeroSize && new FileInfo(file).Length == 0)
            {
                Trace.WriteLine("[ FileUtil ] The file size is zero, File = " + file);
                return false;
            }

            try
            {
                using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                }
            }
            catch
            {
                Trace.WriteLine("[ FileUtil ] The file is locked, File = " + file);
                return false;
            }

            return true;
        }
        
        public static void CopyDirectory(string sourcePath, string destinationPath, bool overwrite)
        {
            DirectoryInfo sourceDir = new DirectoryInfo(sourcePath);
            if (!sourceDir.Exists)
                return;

            if (Directory.Exists(destinationPath) && overwrite)
                Directory.Delete(destinationPath, true);

            if (!Directory.Exists(destinationPath))
                Directory.CreateDirectory(destinationPath);

            foreach (FileInfo fileInfo in sourceDir.GetFiles("*.*", SearchOption.TopDirectoryOnly))
            {
                if ((overwrite || !File.Exists(destinationPath + "\\" + fileInfo.Name)) && fileInfo.Exists)
                    File.Copy(fileInfo.FullName, destinationPath + "\\" + fileInfo.Name, overwrite);
            }

            foreach (DirectoryInfo directoryInfo in sourceDir.GetDirectories("*", SearchOption.TopDirectoryOnly))
            {
                CopyDirectory(directoryInfo.FullName, destinationPath + "\\" + directoryInfo.Name, overwrite);
            }
        }

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

        //When dragging the file to the exe to open, File.Exists will go wrong in some cases.
        //FileHelper.Exists(xxxxx.xaml) == true
        public static bool Exists(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return false;

            if (string.IsNullOrEmpty(Path.GetDirectoryName(fileName)) || String.Equals(Path.GetFileName(fileName), fileName, StringComparison.OrdinalIgnoreCase))
                return false;

            return File.Exists(fileName);
        }

        #region MD5

        /// <summary>
        /// 计算 MD5
        /// </summary>
        /// <param name="fileName">文件路径</param>
        /// <returns>MD5</returns>
        public static string ComputeMD5(string fileName)
        {
            if (!Exists(fileName))
                return null;

            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return ComputeMD5(fs);
            }
        }

        /// <summary>
        /// 计算 MD5
        /// </summary>
        /// <param name="fs">文件流</param>
        /// <returns>MD5</returns>
        public static string ComputeMD5(FileStream fs)
        {
            if (fs == null)
                return null;

            var md5Provider = MD5.Create();
            return string.Join("", md5Provider.ComputeHash(fs).Select(b => b.ToString("X2")).ToArray());
        }

        #endregion

        #region XML

        /// <summary>
        /// 序列化实体到 XML 文件
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="filePath">文件路径</param>
        /// <param name="instance">实体实例</param>
        public static bool SaveToXmlFile<T>(string filePath, T instance)
        {
            return SaveToFile(filePath, sw =>
            {
                new XmlSerializer(typeof(T)).Serialize(sw, instance);
            });
        }

        /// <summary>
        /// 反序列化 XML 到实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="filePath">文件路径</param>
        /// <returns>实体</returns>
        public static T LoadFromXmlFile<T>(string filePath)
        {
            return LoadFromFile(filePath, sr =>
            {
                return (T)(new XmlSerializer(typeof(T))).Deserialize(sr);
            });
        }

        #endregion

        #region JSON

        /// <summary>
        /// 序列化实体到 Json 文件
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="filePath">文件路径</param>
        /// <param name="instance">实体实例</param>
        /// <param name="writeIndented">是否格式化</param>
        public static bool SaveToJsonFile<T>(string filePath, T instance, bool writeIndented = true, JavaScriptEncoder encoder = null)
        {
            return SaveToFile(filePath, sw =>
            {
                var jsonOption = new JsonSerializerOptions { WriteIndented = writeIndented };

                if (encoder != null)
                    jsonOption.Encoder = encoder;

                sw.Write(JsonSerializer.Serialize<T>(instance, jsonOption));
            });
        }

        /// <summary>
        /// 反序列化 Json 到实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="filePath">文件路径</param>
        /// <returns>实体</returns>
        public static T LoadFromJsonFile<T>(string filePath)
        {
            return LoadFromFile(filePath, sr =>
            {
                return JsonSerializer.Deserialize<T>(sr.ReadToEnd());
            });
        }

        #endregion

        #region String

        /// <summary>
        /// 存储字符串到文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="content">文件内容</param>
        public static bool SaveToFile(string filePath, string content)
        {
            return SaveToFile(filePath, sw =>
            {
                sw.Write(content);
            });
        }

        /// <summary>
        /// 从文件加载字符串内容
        /// </summary>
        /// <param name="filePath">文件路径</param>
        public static string LoadFromFile(string filePath)
        {
            return LoadFromFile(filePath, sr =>
            {
                return sr.ReadToEnd();
            });
        }

        #endregion

        #region Provate

        /// <summary>
        /// 序列化实体到文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="serialize">序列化</param>
        private static bool SaveToFile(string filePath, Action<TextWriter> serialize)
        {
            try
            {
                var directory = Path.GetDirectoryName(filePath);

                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    using (var sw = new StreamWriter(fs, Encoding.UTF8))
                    {
                        serialize(sw);
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"[ FileUtil ] SaveToFile, Error = {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 从文件反序列化实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="filePath">文件路径</param>
        /// <param name="deserialize">反序列化</param>
        /// <returns>实体</returns>
        private static T LoadFromFile<T>(string filePath, Func<TextReader, T> deserialize)
        {
            try
            {
                if (!Exists(filePath) || new FileInfo(filePath).Length == 0)
                    return default(T);

                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (var sr = new StreamReader(fs, Encoding.UTF8))
                    {
                        return deserialize(sr);
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"[ FileUtil ] LoadFromFile, Error = {ex.Message}");
                return default(T);
            }
        }

        #endregion
    }
}
