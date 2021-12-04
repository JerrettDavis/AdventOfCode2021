using System.Threading.Tasks;

namespace AdventOfCode.Common.Interfaces;

public interface IFileReader
{
    Task<string> GetContent(string key);
}