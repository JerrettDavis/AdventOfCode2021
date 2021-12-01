using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using AdventRunner.Common.Extensions;
using AdventRunner.Common.Interfaces;
using AdventRunner.Common.Models;

namespace AdventRunner.Solutions
{
    public class DayOneSolutionExecutor : ISolutionExecutor
    {
        private readonly IFileReader _fileReader;
        private readonly ISolutionProfiler _profiler;
        private string? _fileContents;

        public DayOneSolutionExecutor(
            IFileReader fileReader, 
            ISolutionProfiler profiler)
        {
            _fileReader = fileReader;
            _profiler = profiler;
        }

        public string Key => "01";
        
        public async Task<ISolution> GetSolutionAsync()
        {
            var content = await GetFileContentsAsync();

            _profiler.Start();
            
            var parsed = ParseData(content);
            
            _profiler.MarkFileParseComplete();
            
            var partOne = parsed.GreaterThanPrevious()
                .Count(r => r) - 1; // Only need the counts of the trues and we need to discard the first   
            
            _profiler.MarkPartAComplete();
            
            var partTwo = parsed.SlidingWindow(int.MinValue)
                .Where(r => r.PrevItem > int.MinValue && r.NextItem > int.MinValue) // Ignore first and last items
                .Select(r => r.PrevItem + r.CurrentItem + r.NextItem) // Add everything up
                .GreaterThanPrevious()
                .Count(r => r) - 1; // Only need the counts of the trues and we need to discard the first as it'll always be true
            
            _profiler.MarkParkBComplete();

            return new Solution(partOne.ToString(), partTwo.ToString());
        }

        private async Task<string> GetFileContentsAsync()
        {
            if (!string.IsNullOrWhiteSpace(_fileContents))
                return _fileContents;
            
            _fileContents = await _fileReader.GetContent(Key);
            
            return _fileContents;
        }

        private static IReadOnlyCollection<int> ParseData(string input) =>
            input
                .Split(Environment.NewLine)
                .Select(r =>
                {
                    var result = 0;
                    // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
                    foreach (var c in r) result = result * 10 + (c - '0');
                    return result;
                })
                .ToArray();
    }
}