using System;
using System.Collections.Generic;
using htyWEBlib.data;

namespace htyWEBlib.eduDisciplines
{
    public abstract class ScienceNameID: HData, IComparable
    {
        public ScienceNameID(int cI = 1, int cD = 0, int cB = 0, int cS = 1) : base(cI, cD, cB, cS)
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

    }
    public class Science : ScienceNameID
    {
        public Section this[int index] { get => (Section)content[index]; }


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
    }

    public class Section : ScienceNameID
    {
        public Section(): base(2,0,0,1)
        {
        }

        public Subsection this[int index] { get => (Subsection)content[index]; }
        public int ScienceID { get => data_I[1]; set => data_I[1] = value; }

        public override string GetCode() => string.Format("{0}", ID + 1);
    }

    public class Subsection : ScienceNameID
    {
        public Theme this[int index] { get => (Theme)content[index]; }

        public Subsection() : base(2, 0, 0, 1)
        {
        }
        public int SectionID { get => data_I[1]; }

        public override string GetCode()
        {
            string.Format("{0}.{1}", SectionID ,ID + 1);
        }

        public void Add ()


    }

    public class Theme : ScienceNameID
    {
        public int SectionID { get => data_I[1]; }
        public int SubsectionID { get => data_I[2]; }

        public Theme() : base(3, 0, 0, 1)
        {
        }

        public override string GetCode()
        {
            string.Format("{0}.{1}.{2}", SectionID, SubsectionID,  ID + 1);
        }
    }
}
