using System.IO;
using htyWEBlib.data;
using htyWEBlib.Tag;

namespace htyWEBlib.eduDisciplines
{
    public abstract class Fragment:IHData
    {
        /// <summary>Название </summary>
        public string Name { get; set; }
        /// <summary>Индивидуальный порядковый номер</summary>
        public int ID { get; set; }

        public abstract void Load(BinaryReader reader);
        public abstract void Save(BinaryWriter writer);
        public abstract HTag ToTag();

    }

}
