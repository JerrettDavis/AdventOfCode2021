namespace AdventOfCode.Common.Exceptions;

public class MissingDayException : Exception
{
    public MissingDayException(string message): base(message) {}
}