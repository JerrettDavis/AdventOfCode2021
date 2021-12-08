using Advent.Common.Extensions;
using Advent.Common.Interfaces;
using Advent.Common.Models;
using Advent2019.Utilities;

namespace Advent2019.Solvers;

public class Day02 : ISolver
{
    public ISolution Solve(string input)
    {
        var opCode = input.Split(',').Select(r => r.ToInt()).ToArray();
        var partA = CalculatePartA(opCode);
        var partB = CalculatePartB(opCode);

        return new Solution(partA.ToString(), partB.ToString());
    }

    private int CalculatePartA(IEnumerable<int> opCode)
    {
        var computer = new OpCodeRunner(opCode);
        
        computer.SetState(1, 12);
        computer.SetState(2, 2);
        computer.RunOpCode();
        
        return computer.Program.First();
    }

    private int CalculatePartB(IEnumerable<int> opCode)
    {
        const int target = 19690720;
        var computer = new OpCodeRunner(opCode);

        var first = Enumerable.Range(0, 100).ToList();
        var second = first.ToList();

        foreach (var x in first)
        {
            foreach (var y in second)
            {
                computer.Reset();
                computer.SetState(1, x);
                computer.SetState(2, y);
                if (computer.RunOpCode() == target)
                {
                    return x * 100 + y;
                }
            }
        }

        return 0;
    }
}