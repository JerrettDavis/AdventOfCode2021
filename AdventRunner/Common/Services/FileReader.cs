using System.IO;
using System.Threading.Tasks;
using AdventRunner.Common.Interfaces;

namespace AdventRunner.Common.Services
{
    public class FileReader : IFileReader
    {
        public Task<string> GetContent(string key)
        {
            return File.ReadAllTextAsync($"Data/{key}.txt");
        }
    }
}