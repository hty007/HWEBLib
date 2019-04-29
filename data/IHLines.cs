using System.IO;

namespace htyWEBlib.data
{
    public interface IHLines
    {
         HLines ToLines();
         void InLines(HLines lines);
    }
}
