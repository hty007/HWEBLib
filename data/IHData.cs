using System.IO;

namespace htyWEBlib.data
{
    public interface IHData
    {
        void Load(BinaryReader reader);
        void Save(BinaryWriter writer);
    }
}
