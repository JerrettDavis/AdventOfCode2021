using System.Threading.Tasks;

namespace AdventRunner.Common.Interfaces
{
    public interface IFileReader
    {
        Task<string> GetContent(string key);
    }
}