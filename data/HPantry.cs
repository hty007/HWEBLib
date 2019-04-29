using System;
using System.IO;

namespace htyWEBlib.data
{
    public static class HPantry
    {
        private const string Path = "Data";

        static public void SaveData(string name, params IHData[] db)
        {
            string path = GetName(name);
            if (!Directory.Exists(Path))// Создаем директорию!
                Directory.CreateDirectory(Path);
            // создаем объект BinaryWriter
            if (File.Exists(path))
                File.Delete(path);
            using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.CreateNew)))
            {
                foreach (var data in db)
                {
                    data.Save(writer);
                }
            }
        }

        static public void LoadData(string name, params IHData[] db)
        {
            string path = GetName(name);
            if (!File.Exists(path))
                throw new FileNotFoundException($"Файл отсутвует в файловой системе по указонному адресу.\n{path}");
            using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
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
        static public bool FileExists(string name) => File.Exists(GetName(name));
        /// <summary>
        /// Составить имя файла с относительным путём
        /// </summary>
        /// <param name="name">название данных</param>
        /// <returns></returns>
        static public string GetName(string name, string path = Path) => string.Format("{1}/{0}.dbh", name, path);

    }
}
