using System;
using System.Collections.Generic;
using System.Linq;
using AdventRunner.Common.Services;
using AdventRunner.Solutions;

var fileReader = new FileReader();
var profiler = new SolutionProfiler();
var executor = new DayOneSolutionExecutor(fileReader, profiler);
var solution = await executor.GetSolutionAsync();

Console.WriteLine("Cold Run");
Console.WriteLine($"Part A: {solution.PartA}");
Console.WriteLine($"Part B: {solution.PartB}");
Console.WriteLine("---------------------------------");
profiler.PrintStats();
Console.WriteLine("---------------------------------");
var profiles = Enumerable.Range(1, 1000)
    .Select(async _ =>
    {
        await executor.GetSolutionAsync();
        return profiler.GetStats();
    })
    .Select(r => r.Result)
    .SelectMany(r => r)
    .GroupBy(kvp => kvp.Key,
        (key, kvps) =>
        {
            var pairs = kvps as KeyValuePair<string, double>[] ?? kvps.ToArray();
            return new {Key = key, Value = pairs.Sum(kvp => kvp.Value) / pairs.Length };
        })
    .ToDictionary(x => x.Key, x => x.Value);

var prev = 0.0;
Console.WriteLine("Averages of 1000 runs (Times in Milliseconds).");
foreach (var (key, value) in profiles)
{
    Console.WriteLine($"{key} - Step: {value - prev:F4} | Total: {value:F4}");
    prev = value;
}