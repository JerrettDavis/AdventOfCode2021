using Advent.Common.Extensions;
using Advent.Common.Interfaces;
using Advent.Common.Models;

namespace Advent2019.Solvers;

public class Day03 : ISolver
{
    public ISolution Solve(string input)
    {
        var rows = input.Split(Environment.NewLine);
        var wireOnePoints = ToPoint(rows[0].Split(',')).ToList();
        var wireTwoPoints = ToPoint(rows[1].Split(',')).ToList();
        var tmp = wireOnePoints.Intersect(wireTwoPoints)
            .Select(p => Math.Abs(p.X) + Math.Abs(p.Y));
        var tmp2 = wireOnePoints.Intersect(wireTwoPoints).ToList();
        var partA = wireOnePoints.Intersect(wireTwoPoints)
            .Where(p => p.X > 0 && p.Y > 0)
            .Min(p => Math.Abs(p.X) + Math.Abs(p.Y));
        
        return new Solution(partA.ToString(), null!);
    }

    private int GetDistance(string move)
    {
        return move.Substring(1).ToInt();
    }
    private IEnumerable<Point> ToPoint(IEnumerable<string> moves)
    {
        var position = new Point { X = 0, Y = 0 };
        var allMoves = new List<Point>();
        foreach (var move in moves)
        {
            var distance = GetDistance(move);
            var pos = position;
            allMoves.AddRange(move[0] switch
            {
                'U' => Enumerable.Range(position.Y, distance).Select(p => pos with { Y = p }),
                'D' => Enumerable.Range(position.Y - distance, distance).Reverse().Select(p => pos with { Y = p }),
                'L' => Enumerable.Range(position.X - distance, distance).Reverse().Select(p => pos with { X = p }),
                'R' => Enumerable.Range(position.X, distance).Select(p => pos with { X = p }),
                _ => throw new InvalidOperationException()
            });
            position = allMoves.Last();
        }

        return allMoves;
    }
}

internal record Point
{
    public int X { get; init; }
    public int Y { get; init; }
}