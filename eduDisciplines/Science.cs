using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using htyWEBlib.data;

namespace htyWEBlib.eduDisciplines
{
    public class Science: IHData, IComparable
    {
        
        /// <summary>Название </summary>
        public string Name { get; set; }
        /// <summary>Индивидуальный порядковый номер</summary>
        public int ID { get; set; }
        protected List<Science> content;
        public Science this[int index] => content.Find(n => n.ID == index);
        public int Count { get => content.Count; }
        protected Science master;

        public Science() => content = new List<Science>();
        public Science(string name, int id):this()
        {
            master = null;
            Name = name;
            ID = id;
        }
        public string GetCode(int premier = -1)
        {
            List<int> ids = new List<int>();
            ids.Add(ID);
            var sc = master;
            while (sc != null)
            {
                ids.Add(sc.ID);
                sc = sc.master;
            }
            ids.Reverse();
            if (premier != -1)
                ids.RemoveRange(0, premier);
            string code = string.Join(".",ids.ToArray());
            return code;
        }


        public override string ToString()
        {
            return string.Format("{0} {1}", GetCode(1), Name);
        }

        public virtual void Add(Science science)
        {
            if (content.Find(n => n.ID == science.ID)!= null)
                throw new ArgumentException();
            content.Add(science);
            science.master = this;
            content.Sort();            
        }
        public void Add(string name, int id)
        {
            Science section = new Science(name, id);            
            Add(section);
        }

        public int CompareTo(object obj)
        {
            Science sc = (Science)obj;
            return ID.CompareTo(sc.ID);
        }
        public void Load(BinaryReader reader)
        {
            ID = reader.ReadInt32();
            Name = reader.ReadString();
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                Science item = new Science();
                item.Load(reader);
                Add(item);
            }
        }
        public void Save(BinaryWriter writer)
        {
            writer.Write(ID);
            writer.Write(Name);
            writer.Write(content.Count);
            foreach (var item in content)
            {
                item.Save(writer);
            }

        }
    }
    
}
