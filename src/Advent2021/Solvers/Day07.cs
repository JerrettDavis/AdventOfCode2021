using Advent.Common.Extensions;
using Advent.Common.Interfaces;
using Advent.Common.Models;

namespace Advent2021.Solvers;

public class Day07 : ISolver
{
    public ISolution Solve(string input)
    {
        var crabs = input
            .Split(',')
            .Select(r => r.ToInt())
            .ToArray();
        var partA = PartA(crabs);
        var partB = PartB(crabs);
        
        return new Solution(partA.ToString(), partB.ToString());
    }

    private static int PartA(IList<int> crabs)
    {
        return Enumerable.Range(crabs.Min(), crabs.Max())
            .Select(n => crabs.Sum(crab => Math.Abs(crab - n)))
            .Min();
    }

    private static int PartB(IList<int> crabs) {
        return Enumerable.Range(crabs.Min(), crabs.Max())
            .Select(position => crabs.Select(crab => Math.Abs(crab - position))
                .Select(steps => steps * (steps + 1) / 2).Sum())
            .Min();
    }
}