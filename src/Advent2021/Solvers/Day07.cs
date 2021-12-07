using Advent.Common.Extensions;
using Advent.Common.Interfaces;
using Advent.Common.Models;

namespace Advent2021.Solvers;

public class Day07 : ISolver
{
    public ISolution Solve(string input)
    {
        var crabs = input.Split(',').Select(r => r.ToInt()).ToList();
        
        return new Solution(PartA(crabs).ToString(), PartB(crabs).ToString());
    }

    private long PartA(IList<int> crabs)
    {
        return Enumerable.Range(crabs.Min(), crabs.Max())
            .Select(n => crabs.Sum(crab => Math.Abs(crab - n)))
            .Min();
    }

    private long PartB(IList<int> crabs) {
        return Enumerable.Range(crabs.Min(), crabs.Max())
            .Select(position => crabs.Select(crab => Math.Abs(crab - position))
                .Select(steps => (long)steps * (steps + 1)).Sum() / 2)
            .Min();
    }
}