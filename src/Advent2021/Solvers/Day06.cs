using Advent.Common.Extensions;
using Advent.Common.Interfaces;
using Advent.Common.Models;

namespace Advent2021.Solvers;

public class Day06 : ISolver
{
    public ISolution Solve(string input)
    {
        var fish = input.Split(',').Select(r => r.ToInt()).ToList();
        var partA = GetPartA(fish);
        var partB = GetPartB(fish);

        return new Solution(partA.ToString(), partB.ToString());
    }

    private long GetPartA(IEnumerable<int> input)
    {
        var days = new long[9];
        foreach (var f in input) days[f]++;

        for (var i = 0; i < 80; i++)
        {
            var fishToAdd = days[0];
            for (var x = 1; x < days.Length; x++)
            {
                days[x - 1] = days[x];
            }

            days[6] += fishToAdd;
            days[8] = fishToAdd;
        }

        return days.Sum();
    }

    private long GetPartB(IEnumerable<int> input)
    {
        var days = new long[9];
        foreach (var f in input) days[f]++;
        
        for (var i = 0; i < 256; i++)
        {
            var fishToAdd = days[0];
            for (var x = 1; x < days.Length; x++)
            {
                days[x - 1] = days[x];
            }

            days[6] += fishToAdd;
            days[8] = fishToAdd;
        }

        return days.Sum();
    }

}