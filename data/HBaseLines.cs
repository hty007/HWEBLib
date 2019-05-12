using System.Collections.Generic;
using System.IO;

namespace htyWEBlib.data
{
    public static class HBaseLines
    {
        public static void SaveLines(string name, string path, params HLines[] lines)
        {
            string fileName = GetName(name, path);
            if (!Directory.Exists(path))// Создаем директорию!
                Directory.CreateDirectory(path);
            using (StreamWriter sw = new StreamWriter(fileName, false, System.Text.Encoding.Default))
            {
                foreach (var line in lines)
                    line.Save(sw);
            }
        }

        public static  HLines[] LoadLines(string name, string path)
        {
            var result = new List<HLines>();
            string fileName = GetName(name, path);
            if (!File.Exists(fileName))
                throw new FileNotFoundException("Файл не существует!");
            using (StreamReader sr = new StreamReader(fileName, System.Text.Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    HLines line = new HLines();
                    try
                    {
                        line.Load(sr);
                    }
                    catch
                    {}
                    finally
                    { result.Add(line); }
                    
                }                
                    
            }
            return result.ToArray();
        }
        /// <summary>
        /// Проверяет существует ли фаил
        /// </summary>
        /// <param name="name">название данных</param>
        /// <returns>true - существует, false - не существует</returns>
        static public bool FileExists(string name, string path) => File.Exists(GetName(name,path));
        static public string GetName(string name, string path) => string.Format("{1}/{0}.lin", name, path);
    }
}
