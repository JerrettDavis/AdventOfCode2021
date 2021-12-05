using Advent.Common.Extensions;
using Advent.Common.Interfaces;
using Advent.Common.Models;

namespace Advent2021.Solvers;

public class Day01: ISolver
{
        public ISolution Solve(string input)
        {
            var parsed = ParseData(input);
            var partOne = parsed.GreaterThanPrevious()
                .Count(r => r) - 1; // Only need the counts of the trues and we need to discard the first
            var partTwo = parsed.SlidingWindow(int.MinValue)
                .Where(r => r.PrevItem > int.MinValue && r.NextItem > int.MinValue) // Ignore first and last items
                .Select(r => r.PrevItem + r.CurrentItem + r.NextItem) // Add everything up
                .GreaterThanPrevious()
                .Count(r => r) - 1; // Only need the counts of the trues and we need to discard the first as it'll always be true

            return new Solution(partOne.ToString(), partTwo.ToString());
        }

        private static IReadOnlyCollection<int> ParseData(string input) =>
            input
                .Split(Environment.NewLine)
                .Select(r => r.ToInt())
                .ToArray();
    }