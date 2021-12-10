using Advent.Common.Extensions;
using Advent.Common.Interfaces;
using Advent.Common.Models;

namespace Advent2021.Solvers;

public class Day08 : ISolver
{
    public ISolution Solve(string input)
    {
        var lines = input.Split(Environment.NewLine).ToList();
        var partA = PartA(lines);
        var partB = PartB(lines);

        return new Solution(partA.ToString(), partB.ToString());
    }

    public int PartA(IEnumerable<string> lines)
    {
        var digits = new Dictionary<int, int>
        {
            [2] = 1,
            [4] = 4,
            [3] = 7,
            [7] = 8
        };
        return lines
            .SelectMany(line => line.Split('|')[1]
                .Trim()
                .Split(" ")
                .Select(r => r.Length))
            .Count(word => digits.ContainsKey(word));
    }

    public int PartB(IEnumerable<string> lines)
    {
        var line = lines.Select(line => line.Split('|'))
            .Select(v => new { Input = v[0].Trim().Split(" "), Output = v[1].Trim().Split(" ").ToList()})
            .Select(v =>
            {
                var digits = GetDigits(v.Input);
                var output = string.Join("", v.Output.Select(d => GetDigit(d.ToHashSet(), digits))).ToInt();
                return output;
            })
            .ToList();

        return line.Sum();
    }

    private int GetDigit(HashSet<char> digit, Dictionary<int,HashSet<char>> map)
    {
        return map.Single(r => r.Value.SetEquals(digit)).Key;
    }

    public Dictionary<int, HashSet<char>> GetDigits(IList<string> words)
    {
        var map = new Dictionary<int, int>
        {
            [2] = 1,
            [4] = 4,
            [3] = 7,
            [7] = 8
        };
        var charMap = words
            .Where(w => map.ContainsKey(w.Length))
            .ToDictionary(w => map[w.Length], w => w.ToHashSet());
        var unsolved = words
            .Where(w => !map.ContainsKey(w.Length))
            .Select(r => r.ToHashSet())
            .ToHashSet();
        var leftTopAndMiddle = charMap[4].Except(charMap[7]).ToHashSet();
        var six = unsolved.Single(v => v.IsSupersetOf(charMap[8].Except(charMap[1])));

        charMap.Add(6, six);
        unsolved.Remove(six);

        var rightTop = charMap[8].Except(charMap[6]);
        var rightBottom = charMap[1].Except(rightTop);
        var twoAndNine = unsolved.Where(v => v.IsSupersetOf(charMap[8].Except(rightBottom).Except(leftTopAndMiddle))).ToList();
        var two = twoAndNine.Single(r => r.Count == 5);
        var nine = twoAndNine.Single(r => r.Count == 6);

        charMap.Add(2, two);
        charMap.Add(9, nine);
        unsolved.Remove(two);
        unsolved.Remove(nine);

        var zero = unsolved.Single(r => r.Count == 6);
        
        charMap.Add(0, zero);
        unsolved.Remove(zero);

        var five = unsolved.Single(v => leftTopAndMiddle.IsSubsetOf(v));
        
        charMap.Add(5, five);
        unsolved.Remove(five);

        var three = unsolved.Single();
        
        charMap.Add(3, three);

        return charMap;
    }
    
    /*
     *
  0:      1:      2:      3:      4:
 aaaa    ....    aaaa    aaaa    ....
b    c  .    c  .    c  .    c  b    c
b    c  .    c  .    c  .    c  b    c
 ....    ....    dddd    dddd    dddd
e    f  .    f  e    .  .    f  .    f
e    f  .    f  e    .  .    f  .    f
 gggg    ....    gggg    gggg    ....

  5:      6:      7:      8:      9:
 aaaa    aaaa    aaaa    aaaa    aaaa
b    .  b    .  .    c  b    c  b    c
b    .  b    .  .    c  b    c  b    c
 dddd    dddd    ....    dddd    dddd
.    f  e    f  .    f  e    f  .    f
.    f  e    f  .    f  e    f  .    f
 gggg    gggg    ....    gggg    gggg
     * 
     */

}