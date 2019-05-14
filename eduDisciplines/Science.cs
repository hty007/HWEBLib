 using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using htyWEBlib.data;

namespace htyWEBlib.eduDisciplines
{
    public class Science: IHData, IComparable, IEnumerable
    {        
        /// <summary>Название </summary>
        public string Name { get; set; }
        /// <summary>Индивидуальный порядковый номер</summary>
        public int ID { get; set; }
        /// <summary>Тип отрывка</summary>
        public ScienceType Type { get; set; }

        protected List<Science> content;
        protected Science master;
        
        public Science this[int index] => content.Find(n => n.ID == index);
        /// <summary>Количество состовных частей</summary>
        public int Count { get => content.Count; }
        /// <summary>Получить на уровень выше</summary>
        public Science GetMaster() => master;

        public Science() => content = new List<Science>();
        public Science(string name, int id, ScienceType type = ScienceType.theme):this()
        {
            master = null;
            Name = name;
            ID = id;
            Type = type;
        }
        /// <summary>
        /// Получить код 
        /// </summary>
        /// <param name="premier">Сколько уровень срезать</param>
        /// <returns>Строка с кодом</returns>
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

        public bool ContainIn(int id)
        {
            return null != content.Find(n => n.ID == id);
        }

        /// <summary>
        /// Найти нужную тему 
        /// </summary>
        /// <returns>The science.</returns>
        /// <param name="code">Нет проверки на null</param>
        public Science GetScience(string code)
        {
            if (GetCode() == code)
                return this;
            Science result = null;
            foreach (Science s in content)
            {                
                result = s.GetScience(code);
                if (result != null)
                    break;
            }
            return result;
            
            //throw new NotImplementedException();
        }

        public void SetMaster(Science master)
        {
            this.master = master;            
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
        public void Add(string name, int id, ScienceType type = ScienceType.theme)
        {
            Science section = new Science(name, id, type);            
            Add(section);
        }

        public int CompareTo(object obj)
        {
            Science sc = (Science)obj;
            return ID.CompareTo(sc.ID);
        }
        public void Load(BinaryReader reader)
        {
            Type = (ScienceType)reader.ReadInt32();
            switch (Type)
            {
                case ScienceType.formyle:
                case ScienceType.text:
                case ScienceType.theme:
                    ID = reader.ReadInt32();
                    Name = reader.ReadString();
                    break;
                default:
                    throw new ArgumentException($"Не предусмотрено: {Type}.");
            }
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
            writer.Write((int)Type);
            switch (Type)
            {
                case ScienceType.formyle:
                case ScienceType.text:
                case ScienceType.theme:
                    writer.Write(ID);
                    writer.Write(Name);
                    break;
                default:
                    throw new ArgumentException($"Не предусмотрено: {Type}.");
            }                
            writer.Write(content.Count);
            foreach (var item in content)
            {
                item.Save(writer);
            }
        }
        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)content).GetEnumerator();
        }
    }

    public enum ScienceType
    {
        theme,
        text,
        formyle
    }

    public static class ScienceTypeHelper
    {
        public static string ToRus(this ScienceType type)
        {
            switch (type)
            {
                case ScienceType.formyle:return "Формула";
                case ScienceType.text: return "Текст"; 
                case ScienceType.theme: return "Тема";
                default:
                    throw new ArgumentException($"Не предусмотрено: {type}.");
            }
        }
        public static ScienceType ToScienceType(this string text)
        {
            return (ScienceType)Enum.Parse(typeof(ScienceType), text);
        }
    }
    
}
