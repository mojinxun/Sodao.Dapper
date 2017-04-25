using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Sodao.Dapper.Console
{
    public class TextHelper
    {
        public static int dayCount = 0;

        /// <summary>
        /// 吸入
        /// </summary>
        /// <param name="message"></param>
        public static void Write(string message)
        {
            var filePath = $"{AppDomain.CurrentDomain.BaseDirectory}/txtlog/{DateTime.Now.ToString("yyyy-MM-dd")}.txt";
            Write(filePath, message);
        }

        //最大 10M
        /// <summary>
        /// 写入文本文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="message"></param>
        public static void Write(string filePath, string message)
        {
            Encoding gb2312 = Encoding.GetEncoding("gb2312");//设置一下编码格式
            if (!File.Exists(filePath))
            {
                var dir = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                StreamWriter swCreate = new StreamWriter(filePath, false, gb2312);
                swCreate.Flush();
                swCreate.Close();
                swCreate.Dispose();
            }
            else
            {
                var fileInfo = new FileInfo(filePath);
                if (fileInfo.Length > 10 * 1024 * 1024)
                {
                    filePath = fileInfo.DirectoryName + $"/{DateTime.Now.ToString("yyyy-MM-dd")}_{++dayCount}.txt";
                    Write(filePath, message);
                }
            }

            using (StreamWriter sw = File.AppendText(filePath))
            {
                sw.WriteLine(message);
            }
        }
    }
}
