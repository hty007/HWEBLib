using System;
using System.Collections.Generic;
using System.IO;

namespace htyWEBlib.data
{
    public static class HPantry
    {
        private const string Path = "Data";
        private static string nameDir = null;
        public static string NameDir { get => nameDir; set => nameDir = value; }

        static public void SaveData(string name, params IHData[] db)
        {
            string fullNamePath = GetName(name,nameDir);
            if (!Directory.Exists(Path))// Создаем директорию!
                Directory.CreateDirectory(Path);
            // создаем объект BinaryWriter
            if (File.Exists(fullNamePath))
                File.Delete(fullNamePath);
            using (BinaryWriter writer = new BinaryWriter(File.Open(fullNamePath, FileMode.CreateNew)))
            {
                foreach (var data in db)
                {
                    data.Save(writer);
                }
            }
        }

        static public void LoadData(string nameFile, params IHData[] db)
        {
            string fullNamePath = GetName(nameFile, nameDir);
            if (!File.Exists(fullNamePath))
                throw new FileNotFoundException($"Файл отсутвует в файловой системе по указонному адресу.\n{fullNamePath}");
            using (BinaryReader reader = new BinaryReader(File.Open(fullNamePath, FileMode.Open)))
            {
                foreach (var data in db)
                {
                    data.Load(reader);
                }
            }
        }

        /// <summary>
        /// Проверяет существует ли фаил
        /// </summary>
        /// <param name="name">название данных</param>
        /// <returns>true - существует, false - не существует</returns>
        static public bool FileExists(string name) => File.Exists(name);
        /// <summary>
        /// Составить имя файла с относительным путём
        /// </summary>
        /// <param name="nameFile">название данных</param>
        /// <returns></returns>
        static public string GetName(string nameFile, string expansion=null)
        {
            if (expansion == null)
                expansion = "dbh";
            if (nameDir == null)
                return string.Format("{1}/{0}.{2}", nameFile, Path, expansion);
            return string.Format("{2}/{1}/{0}.{3}", nameFile,nameDir, Path, expansion);
        }

        public static void SaveLines(string nameFile, params HLines[] lines)
        {
            string fileName = GetName(nameFile, nameDir);
            if (!Directory.Exists(nameDir))// Создаем директорию!
                Directory.CreateDirectory(nameDir);
            using (StreamWriter sw = new StreamWriter(fileName, false, System.Text.Encoding.Default))
            {
                foreach (var line in lines)
                    line.Save(sw);
            }
        }

        public static void LoadLines(string nameFile, params HLines[] lines)
        {
            var result = new List<HLines>();
            string fullNameFile = GetName(nameFile, "lin");
            if (!File.Exists(fullNameFile))
                throw new FileNotFoundException($"Файл отсутвует в файловой системе по указонному адресу.\n{fullNameFile}");
            using (StreamReader sr = new StreamReader(fullNameFile, System.Text.Encoding.Default))
            {
                foreach (var line in lines)
                    line.Load(sr);                  
            }
            lines = result.ToArray();
        }
    }
}
