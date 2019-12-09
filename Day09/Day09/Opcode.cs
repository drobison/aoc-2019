namespace Day09
{
    public enum Opcode
    {
        Add = 1,
        Multiply = 2,
        Exit = 99,
        Input = 3,
        Output = 4,
        JumpIfTrue = 5,
        JumpIfFalse = 6,
        LessThan = 7,
        Equals = 8,
        RelativeBase = 9
    }
}