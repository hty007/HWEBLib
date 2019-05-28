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
        /*
        public string ValueString { get => value; set => this.value = value; }
        public int ValueInt { get => int.Parse(value); set => this.value = value.ToString(); }
        public bool ValueBool { get => bool.Parse(value); set => this.value = value.ToString(); }
        public double ValueDouble { get => double.Parse(value); set => this.value = value.ToString(); }
        /**/
        

        public Pair(string key, object value)
        {
            Key = key;
            var typ = value.GetType();
            if (typ == typeof(int)) type = PairType.i;
            else if (typ == typeof(bool)) type = PairType.b;
            else if (typ == typeof(double)) type = PairType.d;
            else if (typ == typeof(string)) type = PairType.s;
            else if (typ.GetInterfaces().Contains(typeof(IHData))) type = PairType.IHdata;
            else throw new ArgumentException($"Это надо предусмотреть! type = {typ}");
            this.value = value;
        }

        public Pair()
        {
        }

        /*public Pair() : this("","") { }
public Pair(string key, int value):this(key, value.ToString()) { }
public Pair(string key, bool value) : this(key, value.ToString()) { }
public Pair(string key, double value) : this(key, value.ToString()) { }
public Pair(string line) => InString(line);/**/

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
                case PairType.IHdata: ((IHData)value).Load(reader); break;
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
                case PairType.IHdata: ((IHData)value).Save(writer); break;
                default: throw new ArgumentException($"Это надо предусмотреть! type = {type}");
            }            
        }

        private enum PairType
        {
            i,b,d,s,IHdata
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

        public void Add(Pair pair) => data.Add(pair);
        public void Add(string key, object value)
        {
            Add(new Pair(key, value));
        }

        

        public void Load(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        public void Save(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<Pair>)data).GetEnumerator();
    }

}
