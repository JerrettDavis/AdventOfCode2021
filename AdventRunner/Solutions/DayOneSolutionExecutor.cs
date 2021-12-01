using System;
using System.Collections.Generic;
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
            _profiler.Start();
            
            var content = await GetFileContentsAsync();
            
            _profiler.MarkFileReadComplete();
            
            var parsed = ParseData(content);
            
            _profiler.MarkFileParseComplete();
            
            var partOne = parsed.GreaterThanPrevious()
                .Skip(1) // Skip the first as it'll always be true since there's no previous
                .Count(r => r); // Only need the counts of the trues   
            
            _profiler.MarkPartAComplete();
            
            var partTwo = parsed.SlidingWindow(int.MinValue)
                .Where(r => r.PrevItem > int.MinValue && r.NextItem > int.MinValue) // Ignore first and last items
                .Select(r => r.PrevItem + r.CurrentItem + r.NextItem) // Add everything up
                .GreaterThanPrevious()
                .Skip(1) // Skip the first as it'll always be true
                .Count(r => r); // Only need the counts of the trues
            
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

        private static IList<int> ParseData(string input) =>
            input
                .Split(Environment.NewLine)
                .Where(r => !string.IsNullOrWhiteSpace(r))
                .Select(int.Parse)
                .ToList();
    }
}