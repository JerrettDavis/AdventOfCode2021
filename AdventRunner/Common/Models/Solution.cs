using AdventRunner.Common.Interfaces;

namespace AdventRunner.Common.Models
{
    public class Solution : ISolution
    {
        public string PartA { get; }
        public string PartB { get; }

        public Solution(string partA, string partB)
        {
            PartA = partA;
            PartB = partB;
        }
    }
}