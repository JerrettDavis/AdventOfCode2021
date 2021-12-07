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

    private int PartA(IList<int> crabs)
    {
        
        return Enumerable.Range(crabs.Min(), crabs.Max())
            .Select(n => crabs.Sum(crab => Math.Abs(crab - n)))
            .Min();
    }

    private int PartB(IList<int> crabs) {
        return Enumerable.Range(crabs.Min(), crabs.Max())
            .Select(n => crabs.Select(crab => Math.Abs(crab - n))
                .Select(v => v * (v + 1) / 2).Sum())
            .Min();
    }
}