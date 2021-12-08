namespace Advent2019.Utilities;

public class OpCodeRunner
{
    private IList<int> _program;
    private IList<int> _initialState;

    public OpCodeRunner(IEnumerable<int> program)
    {
        _program = program.ToList();
        _initialState = _program.ToList();
    }

    public IEnumerable<int> Program => _program;

    public void Reset()
    {
        _program = _initialState.ToList();
    }
    
    public void SetState(int position, int value)
    {
        _program[position] = value;
    }
    
    public int RunOpCode()
    {
        var index = 0;
        while ((OpCode)_program[index] != OpCode.Halt)
        {
            switch ((OpCode)_program[index])
            {
                case OpCode.Add:
                    _program[_program[index + 3]] = _program[_program[index + 1]] + _program[_program[index + 2]]; 
                    index += 4;
                    break;
                case OpCode.Multiply:
                    _program[_program[index + 3]] = _program[_program[index + 1]] * _program[_program[index + 2]];
                    index += 4;
                    break;
                case OpCode.Halt:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return _program[0];
    }
}

public enum OpCode
{
    Add = 1,
    Multiply = 2,
    Halt = 99
}