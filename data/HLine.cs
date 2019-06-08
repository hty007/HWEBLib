using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace htyWEBlib.data
{
    public class HLines : IEnumerable 
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




        #endregion

    }

    public class Pair
    {
        private string key;
        private string value;
        
        public string Key { get => key; set => key = value; }
        public string ValueString { get => value; set => this.value = value; }
        public int ValueInt { get => int.Parse(value); set => this.value = value.ToString(); }
        public bool ValueBool { get => bool.Parse(value); set => this.value = value.ToString(); }
        public double ValueDouble { get => double.Parse(value); set => this.value = value.ToString(); }
        

        public Pair(string key, string value)
        {
            Key = key;
            this.value = value;
        }

        public Pair(string key, int value):this(key, value.ToString()) { }
        public Pair(string key, bool value) : this(key, value.ToString()) { }
        public Pair(string key, double value) : this(key, value.ToString()) { }
        public Pair(string line) => InString(line);

        public override string ToString() => string.Format("{0}={1}", key, value);
        public void InString(string line)
        {
            var pair = line.Split('=');
            if (pair.Length != 2)
                throw new ArgumentException("Должно быть пара значений");
            key = pair[0];
            value = pair[1];
        }
    }
}
