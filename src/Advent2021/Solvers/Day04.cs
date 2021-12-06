using System.Collections.Immutable;
using System.Diagnostics;
using System.Text;
using Advent.Common.Extensions;
using Advent.Common.Interfaces;
using Advent.Common.Models;

namespace Advent2021.Solvers;

public class Day04 : ISolver
{
    public ISolution Solve(string input)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var lines = input.Split(Environment.NewLine);
        var moves = lines[0]
            .Split(',')
            .Select(r => r.ToInt())
            .ToArray();
        var boards = lines
            .Skip(1)
            .ToBoards()
            .ToArray();
        var games = moves
            .GetBingosInOrder(boards)
            .ToList();
        var partA = games.First().score;
        var partB = games.Last().score;
        Console.WriteLine($"Took: {stopwatch.Elapsed.TotalMilliseconds}");
        return new Solution(partA.ToString(), partB.ToString());
    }
}

public class Board
{
    private int[][] Numbers { get; }
    public IEnumerable<int> UncalledNumbers => _allNumbers.Except(_moves);
    private int Rows { get; }
    private int Columns { get; }
    
    private readonly HashSet<int> _allNumbers;
    private readonly HashSet<int> _moves;
    private readonly int _minMoves;

    public Board(IEnumerable<string> input)
    {
        _allNumbers = new HashSet<int>();
        _moves = new HashSet<int>();
        Numbers = input
            .Select(r => r.Split(' ')
                .Where(v => !string.IsNullOrWhiteSpace(v))
                .Select(n =>
                {
                    var val = n.ToInt();
                    _allNumbers.Add(val);
                    return val;
                })
                .ToArray())
            .ToArray();
        Rows = Numbers.Length;
        Columns = Numbers[0].Length;
        _minMoves = Math.Min(Rows, Columns);
    }

    public bool TryMakeMove(int number)
    {
        if (!_allNumbers.Contains(number))
            return false;

        _moves.Add(number);
        return true;
    }

    //TODO: Refactor this mess
    public bool HasBingo()
    {
        if (_moves.Count < _minMoves) return false;
        
        var columns = new Dictionary<int,List<int?>>();

        for (var x = 0; x < Rows; x++)
        {
            var row = new List<int?>();
            for (var y = 0; y < Columns; y++)
            {
                var contains = _moves.Contains(Numbers[x][y]) ?
                        Numbers[x][y] : default(int?);
                row.Add(contains);
                
                var column = x == 0 ? new List<int?>() : columns[y];
                column.Add(contains);
                
                if (x == 0)
                    columns.Add(y, column);

                // Is this necessary?
                columns[y] = column;

                if (x == Rows - 1 && column.Count == columns.Count && column.All(r => r.HasValue))
                    return true;
            }

            if (row.All(r => r.HasValue))
                return true;
        }

        return false;
    }

    public string ToStringWithCalled()
    {
        var sb = new StringBuilder();
        foreach (var row in Numbers)
        {
            sb.AppendLine(string.Join(' ',
                row.Select(r =>
                    _moves.Contains(r) ?
                        " x" :
                    $"{r,2}")));
        }

        return sb.ToString();
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach (var row in Numbers)
        {
            sb.AppendLine(string.Join(' ', row.Select(r => $"{r,2}")));
        }

        return sb.ToString();
    }
}

public static partial class EnumerableExtensions
{
    public static IEnumerable<Board> ToBoards(this IEnumerable<string> rows)
    {
        var boardRows = new List<string>();
        foreach (var row in rows)
        {
            if (string.IsNullOrWhiteSpace(row))
            {
                if (boardRows.Count <= 0) continue;
                
                var board = new Board(boardRows);
                boardRows.Clear();
                yield return board;
            }
            else
                boardRows.Add(row);
        }

        if (boardRows.Count > 0)
            yield return new Board(boardRows);
    }

    public static IEnumerable<(Board board, int score)> GetBingosInOrder(
        this IEnumerable<int> moves,
        IEnumerable<Board> boards)
    {
        var winningBoards = new HashSet<Board>();
        var boardList = boards.ToArray();
        foreach (var move in moves)
        {
            if (winningBoards.Count == boardList.Length) yield break;

            var result = boardList
                .Except(winningBoards)
                .PlayMove(move)
                .ToArray();
            var (board, score) = result.FirstOrDefault(r => r.score.HasValue);
            if (score == null) continue;
            
            var allWins = result
                .Where(r => r.score.HasValue)
                .Select(r => r.board);
            foreach(var win in allWins)
                winningBoards.Add(win);
            
            yield return (board, score.Value);
        }
    }

    private static IEnumerable<(Board board, int? score)> PlayMove(
        this IEnumerable<Board> boards, int move)
    {
        return boards.Select(board => new {board, madeMove = board.TryMakeMove(move)})
            .Where(t => t.madeMove && t.board.HasBingo())
            .Select(t => (t.board, t.board.UncalledNumbers.Sum() * move))
            .Select(dummy => ((Board board, int? score)) dummy);
    }
}