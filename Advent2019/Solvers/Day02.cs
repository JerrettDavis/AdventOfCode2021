using Advent.Common.Extensions;
using Advent.Common.Interfaces;
using Advent.Common.Models;

namespace Advent2019.Solvers;

public class Day02 : ISolver
{
    public ISolution Solve(string input)
    {
        var opCode = input.Split(',').Select(r => r.ToInt()).ToArray();
        var partA = CalculatePartA(opCode);

        return new Solution(partA.ToString(), null!);
    }

    private int CalculatePartA(IEnumerable<int> opCode)
    {
        var output = opCode.ToArray();
        // Set state
        output[1] = 12;
        output[2] = 2;

        return RunOpCode(output)[0];
    }

    private int[] RunOpCode(IEnumerable<int> input)
    {
        var program = input.ToArray();
        var index = 0;
        while ((OpCode)program[index] != OpCode.Halt)
        {
            switch ((OpCode)program[index])
            {
                case OpCode.Add:
                    program[program[index + 3]] = program[program[index + 1]] + program[program[index + 2]]; 
                    break;
                case OpCode.Multiply:
                    program[program[index + 3]] = program[program[index + 1]] * program[program[index + 2]];
                    break;
                case OpCode.Halt:
                    return program;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            index += 4;
        }

        return program;
    }
}

public enum OpCode
{
    Add = 1,
    Multiply = 2,
    Halt = 99
}