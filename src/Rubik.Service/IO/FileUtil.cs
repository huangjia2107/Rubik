using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Xml.Serialization;

using Rubik.Service.Log;

namespace Rubik.Service.IO
{
    public static class FileUtil
    {
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
                Logger.Instance.Main.Error($"[ FileHelper ] SaveToFile, Error = {ex.Message}");
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
                Logger.Instance.Main.Error($"[ FileHelper ] LoadFromFile, Error = {ex.Message}");
                return default(T);
            }
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

            var md5Provider = new MD5CryptoServiceProvider();
            return string.Join("", md5Provider.ComputeHash(fs).Select(b => b.ToString("X2")).ToArray());
        }

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
        public static bool SaveToJsonFile<T>(string filePath, T instance, bool writeIndented = true)
        {
            return SaveToFile(filePath, sw =>
            {
                sw.Write(JsonSerializer.Serialize<T>(instance, new JsonSerializerOptions { WriteIndented = writeIndented }));
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

        #region Zip

        /// <summary>
        /// 解压 Zip
        /// </summary>
        /// <param name="fileName">文件路径</param>
        /// <param name="targetFolder">目标文件夹</param>
        public static bool UnZip(string fileName, string targetFolder)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return false;

            if (!Directory.Exists(targetFolder))
                Directory.CreateDirectory(targetFolder);

            try
            {
                ZipFile.ExtractToDirectory(fileName, targetFolder, true);
            }
            catch(Exception ex)
            {
                Logger.Instance.Main.Error($"[ FileHelper ] UnZip, Error = {ex.Message}");
                return false;
            }
            
            return true;
        }

        /// <summary>
        /// 压缩 Zip
        /// </summary>
        /// <param name="sourceFolder">源文件夹</param>
        /// <param name="fileName">目标文件</param>
        public static bool Zip(string sourceFolder, string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName) || !Directory.Exists(sourceFolder))
                return false;

            try
            {
                ZipFile.CreateFromDirectory(sourceFolder, fileName);
            }
            catch (Exception ex)
            {
                Logger.Instance.Main.Error($"[ FileHelper ] Zip, Error = {ex.Message}");
                return false;
            }

            return true;
        }

        #endregion
    }
}
