using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace htyWEBlib.data
{
    /// <summary>
    /// Средство для записи простых данных
    /// </summary>
    public class Pair: IHData
    {
        private string key;
        private object value;
        private PairType type;

        public object Value { get => value; }
        public string Key { get => key; set => key = value; }
        public string ValueString { get => (string)value;}
        public int ValueInt { get => (int)value; }
        public bool ValueBool { get => (bool)value;}
        public double ValueDouble { get => (double)value;}
        /**/
        public Pair(string key, object value)
        {
            Key = key;
            var typ = value.GetType();
            if (typ == typeof(int)) type = PairType.i;
            else if (typ == typeof(bool)) type = PairType.b;
            else if (typ == typeof(double)) type = PairType.d;
            else if (typ == typeof(string)) type = PairType.s;
            else if (typ.GetInterfaces().Contains(typeof(IHData))) type = PairType.pairs;
            else throw new ArgumentException($"Это надо предусмотреть! type = {typ}");
            this.value = value;
        }
        public Pair()
        {
        }
        public override string ToString() => string.Format("{0}={1}", key, value);
        public void Load(BinaryReader reader)
        {
            key = reader.ReadString();
            type = (PairType)reader.ReadInt32();
            switch (type)
            {
                case PairType.i: value = reader.ReadInt32(); break;
                case PairType.d: value = reader.ReadDouble(); break;
                case PairType.b: value = reader.ReadBoolean(); break;
                case PairType.s: value = reader.ReadString(); break;
                case PairType.pairs:
                    value = new Pairs(reader);
                    break;
                default: throw new ArgumentException($"Это надо предусмотреть! type = {type}");
            }            
        }
        public void Save(BinaryWriter writer)
        {
            writer.Write(key);
            writer.Write((int)type);
            switch (type)
            {
                case PairType.i: writer.Write((int)value); break;
                case PairType.d: writer.Write((double)value); break;
                case PairType.b: writer.Write((bool)value); break;
                case PairType.s: writer.Write((string)value);  break;
                case PairType.pairs:((IHData)value).Save(writer);
                    break;
                default: throw new ArgumentException($"Это надо предусмотреть! type = {type}");
            }            
        }
        private enum PairType
        {
            i,b,d,s,pairs
        }
    }
    /// <summary>
    /// Умеет сохранять ряд простых данных и объекты с интерфейсом IHData
    /// </summary>
    public class Pairs : IHData, IEnumerable<Pair>
    {
        private List<Pair> data;
        public Pairs()
        {
            data = new List<Pair>();
        }

        public Pairs(BinaryReader reader):this()
        {
            Load(reader);
        }

        public void Add(Pair pair) => data.Add(pair);
        public void Add(string key, object value)
        {
            Add(new Pair(key, value));
        }
        public IEnumerator<Pair> GetEnumerator() => ((IEnumerable<Pair>)data).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<Pair>)data).GetEnumerator();
        public void Load(BinaryReader reader)
        {
            var count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var pair = new Pair();
                pair.Load(reader);
                Add(pair);
            }

        }
        public void Save(BinaryWriter writer)
        {
            writer.Write(data.Count);
            foreach (var pair in data)
            {
                pair.Save(writer);
            }
        }
    }

}
