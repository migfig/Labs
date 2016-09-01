using System;
using System.IO;

namespace Log.Common.Services.Tests.Common
{
    public static class TextHelper
    {
        public static string GetFullName(string name)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, name);
        }

        public static string GetFileContent(string fileName)
        {
            using (var stream = File.OpenText(fileName))
            {
                return stream.ReadToEnd();
            }
        }

        public static void SaveFileContent(string fileName, string content)
        {
            fileName = GetFullName(fileName);

            if (File.Exists(fileName)) File.Delete(fileName);

            using (var stream = File.CreateText(fileName))
            {
                stream.Write(content);
            }
        }
    }
}
