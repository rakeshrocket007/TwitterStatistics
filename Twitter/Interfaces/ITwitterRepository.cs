using System.IO;
using System.Threading.Tasks;

namespace JH.Twitter
{
    public interface ITwitterRepository
    {
        StreamReader GetTwitterStream(string endpoint);
    }
}