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
        var range = Enumerable.Range(crabs.Min(), crabs.Max());
        var count = range
            .Select(n => crabs.Sum(crab => Math.Abs(crab - n)));

        return count.Min();
    }

    private int PartB(IList<int> crabs) {
        var range = Enumerable.Range(crabs.Min(), crabs.Max());
        var count = new List<int>();
        foreach (var n in range)
        {
            var c = 0;
            foreach (var crab in crabs)
            {
                var v = Math.Abs(crab - n);
                var steps = 0;
                for (var s = 1; s <= v; s++)
                {
                    steps += s;
                }
                c += steps;
            }

            count.Add(c);
        }

        return count.Min();
    }
}