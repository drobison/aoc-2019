using System;
using System.Collections.Generic;

namespace Day02
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

        private static Opcode GetNextCommand(List<int> program, int currentPosition)
        {
            switch (program[currentPosition])
            {
                case 1:
                    return Opcode.Add;
                case 2:
                    return Opcode.Multiply;
                case 99:
                    return Opcode.Exit;
                default:
                    throw new ArgumentException(program[currentPosition].ToString());
            }
        }
    }
}