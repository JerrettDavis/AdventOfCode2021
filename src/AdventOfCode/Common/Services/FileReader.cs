using AdventOfCode.Common.Interfaces;

namespace AdventOfCode.Common.Services;

public class FileReader : IFileReader
{
    public Task<string> GetContent(string key)
    {
        return File.ReadAllTextAsync($"Data/{key}.txt");
    }
}