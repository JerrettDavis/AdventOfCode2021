using Advent.Common.Extensions;
using Advent.Common.Interfaces;
using Advent.Common.Models;

namespace Advent2019.Solvers;

public class Day01 : ISolver
{
    public ISolution Solve(string input)
    {
        var partA = input.Split(Environment.NewLine)
            .Sum(r => CalculateFuel(r.ToInt()));
        var partB = input.Split(Environment.NewLine)
            .Sum(r => CalculateFuelWithFuel(r.ToInt()));
        
        return new Solution(partA.ToString(), partB.ToString());
    }

    private int CalculateFuel(int mass)
    {
        return (int)Math.Floor((double)mass / 3) - 2;
    }

    private int CalculateFuelWithFuel(int mass)
    {
        var fuel = CalculateFuel(mass);
        return fuel <= 0 ? 0 : CalculateFuelWithFuel(fuel) + fuel;
    }
}