namespace AdventOfCode.Common.Exceptions;

public class MissingYearException : Exception
{
    public MissingYearException(string message) : base(message)
    {
    }
}