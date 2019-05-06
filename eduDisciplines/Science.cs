using System;
using System.Collections.Generic;
using htyWEBlib.data;

namespace htyWEBlib.eduDisciplines
{
    public abstract class ScienceNameID: HData, IComparable
    {
        public ScienceNameID(int cI = 4, int cD = 0, int cB = 0, int cS = 1) : base(cI, cD, cB, cS)
        {
        content = new List<ScienceNameID>(); 
        }

               /// <summary>Название </summary>
        public string Name { get => data_S[0]; set => data_S[0] = value; }
        /// <summary>Индивидуальный порядковый номер</summary>
        public int ID { get => data_I[0]; set => data_I[0] = value; }
        protected List<ScienceNameID> content;


        public int Count { get => content.Count; }
        public string Code { get => GetCode(); }

        public abstract string GetCode();

        public int CompareTo(object obj)
        {
            Science sc = (Science)obj;
            return ID.CompareTo(sc.ID);
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", Code, Name);
        }

        public virtual void Add(ScienceNameID science)
        {
            if (content.Find(n => n.ID == science.ID)!= null)
                throw new ArgumentException();
            content.Add(science);
            content.Sort();            
        }

    }
    public class Science : ScienceNameID
    {
        public Section this[int index]
        {
            get
            {
                return (Section)content.Find(n=> n.ID==index);
            }
        }

        public Science(string name, int id):base(1,0,0,1)
        {
            Name = name;
            ID = id;
        }

        public override string ToString() => Name;

        public override string GetCode()
        {
            return "";
        }

        public void Add(string name, int id)
        {
            Section section = new Section(name, id);
            section.ScienceID = ID;
            Add(section);
        }
    }
    public class Section : ScienceNameID
    {
        public Section(): base(2,0,0,1)
        {
        }

        public Section(string name, int id):this()
        {
            Name = name;
            ID = id;
        }

        public Subsection this[int index] { get => (Subsection)content[index]; }
        public int ScienceID { get => data_I[1]; set => data_I[1] = value; }

        public override string GetCode() => string.Format("{0}", ID + 1);

        public void Add(string name, int id)
        {
            Subsection subsection = new Subsection(name, id);
            subsection.SectionID = ID;
            Add(subsection);
        }
    }

    public class Subsection : ScienceNameID
    {
        public Theme this[int index] { get => (Theme)content[index]; }

        public Subsection() : base(2, 0, 0, 1)
        {
        }

        public Subsection(string name, int id) : this()
        {
            Name = name;
            ID = id;
        }

        public int SectionID { get => data_I[1]; set => data_I[1] = value; }

        public override string GetCode()
        {
            return string.Format("{0}.{1}", SectionID+1 ,ID + 1);
        }

        public void Add(string name, int id)
        {
            Theme theme = new Theme(name, id);
            theme.SectionID = SectionID;
            theme.SubsectionID = ID;
            Add(theme);
        }


    }

    public class Theme : ScienceNameID
    {
        public int SectionID { get => data_I[1]; set => data_I[1] = value; }
        public int SubsectionID { get => data_I[2]; set => data_I[2] = value; }

        public Theme() : base(3, 0, 0, 1)
        {
        }

        public Theme(string name, int id):this()
        {
            Name = name;
            ID = id;
        }

        public override string GetCode()
        {
            return string.Format("{0}.{1}.{2}", SectionID+1, SubsectionID+1,  ID + 1);
        }
    }
}
