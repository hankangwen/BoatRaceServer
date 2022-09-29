using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BoatRace;

/* 以下注释为获取exe相对路径的办法
// 获取模块的完整路径，包括文件名。
Console.WriteLine(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);

// 获取和设置当前目录(该进程从中启动的目录)的完全限定目录。
Console.WriteLine(System.Environment.CurrentDirectory);

// 获取应用程序的当前工作目录。这个不一定是程序从中启动的目录啊，有可能程序放在C:\www里,这个函数有可能返回C:\Documents and Settings\ZYB\,或者C:\Program Files\Adobe\,有时不一定返回什么东东，这是任何应用程序最后一次操作过的目录，比如你用Word打开了E:\doc\my.doc这个文件，此时执行这个方法就返回了E:\doc了。
Console.WriteLine(System.IO.Directory.GetCurrentDirectory());

// 获取程序的基目录。
Console.WriteLine(System.AppDomain.CurrentDomain.BaseDirectory);

// 获取和设置包括该应用程序的目录的名称。
Console.WriteLine(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase);
 */
namespace BoatRaceServer.Tools
{
    public class DataManager : Singleton<DataManager>
    {
        DataManager() { }

        /// <summary>
        /// 将txt中的内容读取到列表data中, 路径相对于exe下的文件
        /// </summary>
        public List<string> ReadTextAsset(string fileName, string suffix = ".txt")
        {
            string filePath = $"{AppDomain.CurrentDomain.BaseDirectory}{fileName}{suffix}";
            Debug.Log($"Read file = {filePath}");
            List<string> data = new List<string>();
            StreamReader sr = new StreamReader(filePath);
            string line = string.Empty;
            while ((line = sr.ReadLine()) != null)
            {
                data.Add(line);
            }

            sr.Close();
            return data;
        }

        /// <summary>
        /// 将data中的内容写入txt中
        /// </summary>
        public void WriteTextAsset(List<string> data, string fileName, string suffix = ".txt")
        {
            string filePath = $"{AppDomain.CurrentDomain.BaseDirectory}{fileName}{suffix}";
            Debug.Log($"Write file = {filePath}");
            StreamWriter sw = new StreamWriter(filePath);
            for (int i = 0; i < data.Count; i++)
            {
                sw.WriteLine(data[i]);
            }

            sw.Flush();
            sw.Close();
        }
        
        public unsafe string ToLower(string text)
        {
            fixed (char* newText = text)
            {
                char* itor = newText;
                char* end = newText + text.Length;
                char c;

                while (itor < end)
                {
                    c = *itor;

                    if ('A' <= c && c <= 'Z')
                    {
                        *itor = (char)(c | 0x20);
                    }

                    ++itor;
                }
            }

            return text;
        }
    }
}