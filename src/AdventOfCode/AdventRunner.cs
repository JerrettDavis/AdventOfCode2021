using System.Reflection;
using Advent.Common.Interfaces;
using Advent2021.Solvers;
using AdventOfCode.Common.Exceptions;

namespace AdventOfCode;

public static class AdventRunner
{
    public static async Task<ISolution> GetSolutionAsync(
        int year, 
        int day, 
        string inputStore)
    {
        var assembly = GetAssembly(year);
        var solver = CreateSolver(assembly, year, day);
        var input = await File.ReadAllTextAsync($"{inputStore}/{year}/{day:D2}.txt");

        return solver.Solve(input);
    }

    private static Assembly GetAssembly(int year)
    {
        return year switch
        {
            2021 => typeof(Day01).Assembly,
            _ => throw new MissingYearException($"No solvers for the year '{year}'")
        };
    }

    private static ISolver CreateSolver(Assembly assembly, int year, int day)
    {
        var solver = (ISolver?) assembly.CreateInstance($"Advent{year}.Solvers.Day{day:D2}");

        if (solver == null)
            throw new MissingDayException($"No solver found for the day '{day}'");

        return solver;
    }
}