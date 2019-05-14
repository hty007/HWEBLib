using System.IO;

namespace htyWEBlib.data
{
    public interface IHStringData
    {
        void Load(StreamReader sr);
        void Save(StreamWriter sw);
    }
}