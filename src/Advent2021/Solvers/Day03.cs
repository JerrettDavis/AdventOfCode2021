using Advent.Common.Extensions;
using Advent.Common.Interfaces;
using Advent.Common.Models;

namespace Advent2021.Solvers;

public class Day03 : ISolver
{
    public ISolution Solve(string input)
    {
        /*
         * idea:
         * Each row consists of 5 characters, with each being either zero or one.
         * If we sum the value of each column as we descend, and also count the number of rows
         * we can determine the average value of each row. If we sum..
         */

        var parsed = input.Split('\n');

        return new Solution(SolvePartA(parsed).ToString(), SolvePartB(parsed).ToString());
    }

    private static int SolvePartA(IReadOnlyList<string> input)
    {
        var results = new int[input[0].Length];
        var count = input.Count;
        foreach (var row in input)
        {
            for (var i = 0; i < row.Length; i++)
            {
                results[i] += row[i].ToInt();
            }
        }

        var gamma = new int[results.Length];
        var epsilon = new int[results.Length];
        for (var i = 0; i < results.Length; i++)
        {
            gamma[i] = (int)Math.Round(results[i] / (double)count);
            epsilon[i] = gamma[i] ^ 1;
        }

        return BinaryToInt(gamma) * BinaryToInt(epsilon);
    }

    private static int SolvePartB(IEnumerable<string> input)
    {
        var ones = new List<int[]>();
        var zeros = new List<int[]>();
        foreach (var row in input)
        {
            var data = row
                .Select(r => r.ToInt())
                .ToArray();

            if (data[0] == 0) 
                zeros.Add(data);
            else
                ones.Add(data);
        }

        int[] oxygen;
        int[] co2;
        if (ones.Count > zeros.Count)
        {
            oxygen = GetOxygenRating(ones, 1).Single();
            co2 = GetCo2Rating(zeros, 1).Single();
        }
        else
        {
            oxygen = GetOxygenRating(zeros, 1).Single();
            co2 = GetCo2Rating(ones, 1).Single();
        }

        return BinaryToInt(oxygen) * BinaryToInt(co2);
    }

    private static (IReadOnlyCollection<int[]> ones, IReadOnlyCollection<int[]> zeros)
        SplitCollectionsByBit(IReadOnlyCollection<int[]> input, int bit)
    {
        var ones = new List<int[]>();
        var zeros = new List<int[]>();
        foreach (var row in input)
        {
            if (row[bit] == 0)
                zeros.Add(row);
            else
                ones.Add(row);
        }

        return (ones, zeros);
    }

    private static IReadOnlyCollection<int[]> GetOxygenRating(IReadOnlyCollection<int[]> input, int bit)
    {
        while (true)
        {
            if (input.Count == 1) return input;

            var (ones, zeros) = SplitCollectionsByBit(input, bit);

            // if there are more ones OR there are equal amounts, take ones
            if (ones.Count > zeros.Count || ones.Count == zeros.Count)
            {
                input = ones;
                bit++;
                continue;
            }

            input = zeros;
            bit++;
        }
    }
    
    private static IReadOnlyCollection<int[]> GetCo2Rating(IReadOnlyCollection<int[]> input, int bit)
    {
        while (true)
        {
            if (input.Count == 1) return input;

            var (ones, zeros) = SplitCollectionsByBit(input, bit);

            // If there are more ones OR there are an equal amount, take zeros
            if (ones.Count > zeros.Count || zeros.Count == ones.Count)
            {
                input = zeros;
                bit++;
                continue;
            }

            input = ones;
            bit++;
        }
    }


    private static int BinaryToInt(IEnumerable<int> input)
    {
        var exp = 0;
        return input.Reverse().Sum(item => (int)(item * Math.Pow(2, exp++)));
    }
}