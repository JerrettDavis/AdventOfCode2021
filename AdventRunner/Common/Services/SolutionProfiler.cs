using System;
using System.Collections.Generic;
using System.Diagnostics;
using AdventRunner.Common.Interfaces;

namespace AdventRunner.Common.Services
{
    public class SolutionProfiler : ISolutionProfiler
    {
        private readonly Stopwatch _stopwatch;
        private readonly Dictionary<string, double> _results;

        public SolutionProfiler()
        {
            _stopwatch = new Stopwatch();
            _results = new Dictionary<string, double>();
        }

        public void Start()
        {
            _results.Clear();
            _stopwatch.Restart();
        }

        public void MarkFileReadComplete()
        {
            _results.Add("File Read", _stopwatch.Elapsed.TotalMilliseconds);
        }

        public void MarkFileParseComplete()
        {
            _results.Add("File Parse", _stopwatch.Elapsed.TotalMilliseconds);
        }

        public void MarkPartAComplete()
        {
            _results.Add("Part A", _stopwatch.Elapsed.TotalMilliseconds);
        }

        public void MarkParkBComplete()
        {
            _results.Add("Part B", _stopwatch.Elapsed.TotalMilliseconds);
        }

        public void PrintStats()
        {
            var prev = 0.0;
            foreach (var (key, value) in _results)
            {
                Console.WriteLine($"{key} - Step: {value - prev:F4} | Total: {value:F4}");
                prev = value;
            }
        }

        public Dictionary<string, double> GetStats()
        {
            return _results;
        }
    }
}