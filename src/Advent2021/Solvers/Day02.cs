using Advent.Common.Extensions;
using Advent.Common.Interfaces;
using Advent.Common.Models;

namespace Advent2021.Solvers;

public class Day02 : ISolver
{
    public ISolution Solve(string input)
    {
        var parsed = input.Split("\n");
        var partA = parsed
            .ToSteps()
            .ComputePosition();
        var partB = parsed
            .ComputePositionWithAim();
        
        return new Solution(partA.ToString(), partB.ToString());
    }
}

public static partial class EnumerableExtensions
{
    public static IEnumerable<(int position, int depth)> ToSteps(this IEnumerable<string> rows)
    {
        return rows.Select(row => row.Split(' '))
            .Select(result => new { result, direction = result[0][0] })
            .Select(t => t.direction switch
            {
                'f' => (t.result[1].ToInt(), 0),
                'u' => (0, t.result[1].ToInt() * -1),
                'd' => (0, t.result[1].ToInt()),
                _ => throw new ArgumentOutOfRangeException()
            });
    }

    public static long ComputePosition(this IEnumerable<(int position, int depth)> steps)
    {
        var position = 0;
        var depth = 0;
        foreach (var (p, d) in steps)
        {
            position += p;
            depth += d;
        }

        return position * depth;
    }

    public static long ComputePositionWithAim(this IEnumerable<string> rows)
    {
        var steps = rows.Select(row => row.Split(' '))
            .Select(result => new { result, direction = result[0][0] });

        var position = 0;
        var depth = 0;
        var aim = 0;
        foreach (var step in steps)
        {
            switch (step.direction)
            {
                case 'd':
                    aim += step.result[1].ToInt();
                    break;
                case 'u':
                    aim += step.result[1].ToInt() * -1;
                    break;
                case 'f':
                    var val = step.result[1].ToInt();
                    depth += aim * val;
                    position += val;
                    break;
            }
        }

        return position * depth;
    }
}