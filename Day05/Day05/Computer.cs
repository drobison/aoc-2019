using System;
using System.Collections.Generic;

namespace Day05
{
    public static class Computer
    {
        public static int ProcessInput(List<int> program)
        {
            var currentPosition = 0;
            Opcode command;
            do
            {
                command = GetNextCommand(program, currentPosition++);

                if (command == Opcode.Add)
                {
                    HandleAdd(program, ref currentPosition);
                }
                else if (command == Opcode.Multiply)
                {
                    HandleMultiply(program, ref currentPosition);
                }
                else if (command == Opcode.Save)
                {
                    HandleSave(program, ref currentPosition);
                }
                else if (command == Opcode.Output)
                {
                    HandleOutput(program, ref currentPosition);
                }

            } while (command != Opcode.Exit);

            return program[0];
        }

        private static void HandleAdd(List<int> program, ref int currentPosition)
        {
            var first = program[program[currentPosition++]];
            var second = program[program[currentPosition++]];
            var destination = program[currentPosition++];
            program[destination] = first + second;
        }

        private static void HandleMultiply(List<int> program, ref int currentPosition)
        {
            var first = program[program[currentPosition++]];
            var second = program[program[currentPosition++]];
            var destination = program[currentPosition++];
            program[destination] = first * second;
        }

        private static void HandleSave(List<int> program, ref int currentPosition)
        {
            var interger = program[program[currentPosition++]];
            var destination = program[currentPosition++];
            program[destination] = interger;
        }

        public static void HandleOutput(List<int> program, ref int currentPosition)
        {
            var destination = program[currentPosition++];
            Console.WriteLine(program[destination]);
        }

        private static Opcode GetNextCommand(List<int> program, int currentPosition)
        {
            return ConvertToOpcode(program[currentPosition]);
        }

        public static Opcode ConvertToOpcode(int input)
        {
            switch (input)
            {
                case 1:
                    return Opcode.Add;
                case 2:
                    return Opcode.Multiply;
                case 3:
                    return Opcode.Save;
                case 4:
                    return Opcode.Output;
                case 99:
                    return Opcode.Exit;
                default:
                    throw new ArgumentException(input.ToString());
            }
        }

        public static int GetRequiredParameters(Opcode input)
        {
            switch (input)
            {
                case Opcode.Save:
                case Opcode.Output:
                    return 1;
                case Opcode.Add:
                case Opcode.Multiply:
                    return 3;
                case Opcode.Exit:
                    return 0;
                default:
                    throw new Exception(string.Format("unhandled opcode {0}", input));
            }
        }

        public static Command GetNextCommand(int command)
        {
            var result = new Command();
            var opCodeDigit = Convert.ToInt32(command.ToString().Substring(Math.Max(0, command.ToString().Length)));
            result.Opcode = ConvertToOpcode(opCodeDigit);

            return result;
        }
    }

    public class Command
    {
        public Opcode Opcode { get; set; }

        public List<ParameterMode> ParameterModes { get; set; }
    }

    public enum ParameterMode
    {
        Position,
        Immediate
    }

}
