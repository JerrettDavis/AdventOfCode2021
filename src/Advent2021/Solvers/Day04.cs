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
        var lines = input.Split("\n");
        var moves = lines[0]
            .Split(',')
            .Select(r => r.ToInt())
            .ToArray();
        var boards = lines
            .Skip(1)
            .ToBoards()
            .ToArray();
        var games = moves.GetBingosInOrder(boards).ToList();
        var partA = games.First().score;
        var partB = games.Last().score;
        Console.WriteLine($"Took: {stopwatch.Elapsed.TotalMilliseconds}");
        return new Solution(partA.ToString(), partB.ToString());
    }


}

public class Board
{
    public int[][] Numbers { get; }
    public int[] UncalledNumbers => _allNumbers.Except(_moves).ToArray();
    public int Rows { get; }
    public int Columns { get; }
    private readonly HashSet<int> _allNumbers;
    private readonly HashSet<int> _moves;
    private int _minMoves;

    public Board(IEnumerable<string> input)
    {
        _allNumbers = new HashSet<int>();
        _moves = new HashSet<int>();
        Numbers = input
            .Select(r => r.Split(' ')
                .Where(r => !string.IsNullOrWhiteSpace(r))
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
            {
                boardRows.Add(row);
            }
        }

        if (boardRows.Count > 0)
            yield return new Board(boardRows);
    }

    public static IEnumerable<(Board board, int score)> GetBingosInOrder(
        this IEnumerable<int> moves,
        IReadOnlyCollection<Board> boards)
    {
        var winningBoards = new HashSet<Board>();
        var m = 0;
        foreach (var move in moves)
        {
            if (winningBoards.Count == boards.Count) yield break;
            
            var b = boards.Except(winningBoards);
            var c = 0;
            foreach (var b1 in boards)
            {
                Console.WriteLine($"---------------- BOARD {c++} | MOVE {m} ({move}) -------------------");
                Console.WriteLine(b1.ToStringWithCalled());
            }
            m++;
            var result = b.PlayMove(move);
            var (board, score) = result.FirstOrDefault(r => r.score.HasValue);
            if (score == null) continue;
            winningBoards.Add(board);
            yield return (board, score.Value);
        }
    }

    public static int PlayBingo(
        this IEnumerable<int> moves, 
        IReadOnlyCollection<Board> boards)
    {
        foreach (var move in moves)
        {
            var result = boards.PlayMove(move);
            var (_, score) = result.FirstOrDefault(r => r.score.HasValue);
            if (score != null) return score.Value;
        }

        return -1;
    }

    public static IEnumerable<(Board board, int? score)> PlayMove(
        this IEnumerable<Board> boards, int move)
    {
        foreach (var board in boards)
        {
            var madeMove = board.TryMakeMove(move);
            if (madeMove && board.HasBingo())
                yield return (board, board.UncalledNumbers.Sum() * move);
        }
    }
}