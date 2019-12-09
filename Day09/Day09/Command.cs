using System;
using System.Collections.Generic;

namespace Day09
{
    public class Command
    {
        public Command()
        {
            Parameters = new List<Parameter>();
        }
        public Opcode Opcode { get; set; }

        public List<Parameter> Parameters { get; set; }

        public int RequiredParameters
        {
            get
            {

                switch (Opcode)
                {
                    case Opcode.Input:
                    case Opcode.Output:
                    case Opcode.RelativeBase:
                        return 1;
                    case Opcode.JumpIfTrue:
                    case Opcode.JumpIfFalse:
                        return 2;
                    case Opcode.Add:
                    case Opcode.Multiply:
                    case Opcode.LessThan:
                    case Opcode.Equals:
                        return 3;
                    case Opcode.Exit:
                        return 0;
                    default:
                        throw new Exception(string.Format("unhandled opcode {0}", Opcode));
                }

            }

        }
    }
}