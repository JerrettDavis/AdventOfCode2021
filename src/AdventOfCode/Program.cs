using System.CommandLine;
using System.CommandLine.Invocation;
using AdventOfCode;
using AdventOfCode.Common.Exceptions;

var rootCommand = new RootCommand
{
    new Option<int>(
        "--year",
        getDefaultValue: () => DateTime.Now.Year,
        description: "The year for which the solution is being ran"),
    new Option<int>(
        "--day",
        getDefaultValue: () => 1,
        description: "The day's solution to run"),
    new Option<string>(
        "--input-store",
        getDefaultValue: () => "Data",
        description: "The directory containing the input data")
};

rootCommand.Description = "A simple app to solve the yearly Advent of Code";

rootCommand.Handler = CommandHandler.Create<int, int, string>(async (year, day, inputStore) =>
{
    try
    {
        var solution = await AdventRunner.GetSolutionAsync(year, day, inputStore);
        Console.WriteLine($"Part A: {solution.PartA}");
        Console.WriteLine($"Part B: {solution.PartB}");
    }
    catch (MissingYearException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (MissingDayException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (DirectoryNotFoundException)
    {
        Console.WriteLine("The data directory provided was not found");
    }
    catch (FileNotFoundException)
    {
        Console.WriteLine("The input data was not found for the provided day. Ensure it's in the data directory.");
    }
});

await rootCommand.InvokeAsync(args);