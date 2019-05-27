using System.IO;

namespace htyWEBlib.data
{
    public interface IHData
    {
        void Load(BinaryReader reader);
        void Save(BinaryWriter writer);
    }
    public interface ICoder
    {
        string Code();
        void Decode(string code);
    }
}
