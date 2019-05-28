using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace htyWEBlib.data
{
    public class HLines : IEnumerable, IHStringData,IHData
    {
        #region Поля и свойства
        private string name;
        private List<Pair> data;

        public string Name { get => name; set => name = value; }
        public Pair this[int index] { get => data[index]; set => data[index] = value; }
        public int Count { get => data.Count; }
        #endregion
        #region Конструкторы

        public HLines()
        {
            data = new List<Pair>();
        }
        public HLines(string name) : this() => this.name = name;

        public override string ToString()
        {
            return string.Format("{0} {1}", name, Count.ToString());
        }
        #endregion
        #region Функционал
        public void Add(string key, string value) => data.Add(new Pair(key, value));
        public void Add(string key, int value) => data.Add(new Pair(key, value));
        public void Add(string key, bool value) => data.Add(new Pair(key, value));
        public void Add(string key, double value) => data.Add(new Pair(key, value));
        public Pair GetValue(string key)
        {
            foreach (Pair pair in data)
            {
                if (pair.Key == key)
                    return pair;
            }
            return null;
        }
        public bool TryKey(string key)
        {
            foreach (Pair pair in data)
            {
                if (pair.Key == key)
                    return true;
            }
            return false;
        }
        #endregion
        #region Интерфейс записи
        public void Load(StreamReader sr)
        {
            name = "";
            string sep = "{}[]()<>";
            while (name == "" || name[0]=='#') {// пропустить все пустые строки
                name = sr.ReadLine();
                name = name.Trim();
            }
            name = name.Trim(sep.ToCharArray());
            string buf;
            while (!sr.EndOfStream)
            {
                buf = sr.ReadLine();
                buf = buf.Trim();
                if (buf == "" || buf == "end" || buf == "конец" || sep.LastIndexOf(buf[0]) != -1)
                    return;
                if (buf[0] == '#')
                    continue;
                data.Add(new Pair(buf));
            }            
        }
        public void Save(StreamWriter sw)
        {
            sw.WriteLine($"[{name}]");
            foreach (Pair pair in data)
            {
                sw.WriteLine(pair.ToString());
            }
            sw.WriteLine("end");
        }
        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)data).GetEnumerator();
        }
        public void Load(BinaryReader reader)
        {
            name = reader.ReadString();
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var p = new Pair();
                p.Load(reader);
                data.Add(p);
            }            
        }
        public void Save(BinaryWriter writer)
        {
            writer.Write(name);
            writer.Write(Count);
            for (int i = 0; i < Count; i++)
            {
                data[i].Save(writer);
            }
        }
        #endregion

    }
}
