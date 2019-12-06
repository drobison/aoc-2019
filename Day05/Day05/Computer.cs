using System;
using System.Collections.Generic;

namespace Day05
{
    public static class Computer
    {
        public static int ProcessInput(List<int> program, bool bypassInput = false)
        {
            var currentPosition = 0;
            var lastOutput = 0;
            Command command;
            do
            {
                command = GetNextCommand(program, ref currentPosition);

                if (command.Opcode == Opcode.Add)
                {
                    HandleAdd(program, command);
                }
                else if (command.Opcode == Opcode.Multiply)
                {
                    HandleMultiply(program, command);
                }
                else if (command.Opcode == Opcode.Input)
                {
                    HandleInput(program, command, ref currentPosition, bypassInput);
                }
                else if (command.Opcode == Opcode.Output)
                {
                    HandleOutput(program, command, ref lastOutput);
                }
                else if (command.Opcode == Opcode.JumpIfTrue)
                {
                    HandleJumpIfTrue(program, command, ref currentPosition);
                }
                else if (command.Opcode == Opcode.JumpIfFalse)
                {
                    HandleJumpIfFalse(program, command, ref currentPosition);
                }
                else if (command.Opcode == Opcode.LessThan)
                {
                    HandleLessThan(program, command);
                }
                else if (command.Opcode == Opcode.Equals)
                {
                    HandleEquals(program, command);
                }

            } while (command.Opcode != Opcode.Exit);

            return lastOutput;
        }

        private static void HandleJumpIfTrue(List<int> program, Command command, ref int currentPosition)
        {
            var first = GetProgramValue(program, command.Parameters[0]);
            var second = GetProgramValue(program, command.Parameters[1]);
            if (first != 0)
            {
                currentPosition = second;
            }
        }

        private static void HandleJumpIfFalse(List<int> program, Command command, ref int currentPosition)
        {
            var first = GetProgramValue(program, command.Parameters[0]);
            var second = GetProgramValue(program, command.Parameters[1]);
            if (first == 0)
            {
                currentPosition = second;
            }
        }

        private static void HandleLessThan(List<int> program, Command command)
        {
            var first = GetProgramValue(program, command.Parameters[0]);
            var second = GetProgramValue(program, command.Parameters[1]);
            var destination = command.Parameters[2].Value;
            if (first < second)
            {
                program[destination] = 1;
            }
            else
            {
                program[destination] = 0;
            }
        }

        private static void HandleEquals(List<int> program, Command command)
        {
            var first = GetProgramValue(program, command.Parameters[0]);
            var second = GetProgramValue(program, command.Parameters[1]);
            var destination = command.Parameters[2].Value;
            if (first == second)
            {
                program[destination] = 1;
            }
            else
            {
                program[destination] = 0;
            }
        }

        private static void HandleAdd(List<int> program, Command command)
        {
            var first = GetProgramValue(program, command.Parameters[0]);
            var second = GetProgramValue(program, command.Parameters[1]);
            var destination = command.Parameters[2].Value;
            program[destination] = first + second;
        }

        private static void HandleMultiply(List<int> program, Command command)
        {
            var first = GetProgramValue(program, command.Parameters[0]);
            var second = GetProgramValue(program, command.Parameters[1]);
            var destination = command.Parameters[2].Value;
            program[destination] = first * second;
        }

        private static void HandleInput(List<int> program, Command command, ref int currentPosition, bool bypassInput)
        {
            var inputResult = bypassInput ? 1 : ReadFromConsole();

            var destination = command.Parameters[0].Value;
            program[destination] = inputResult;
        }

        private static int ReadFromConsole()
        {
            Console.WriteLine("Please enter integer input:");
            var input = Console.ReadLine();
            var validInput = int.TryParse(input, out var inputResult);
            if (!validInput)
            {
                Console.WriteLine("Input was not an integer, exiting");
                Environment.Exit(-1);
            }

            return inputResult;
        }

        public static int GetProgramValue(List<int> program, Parameter parameter)
        {
            int result;
            if (parameter.ParameterMode == ParameterMode.Immediate)
                result = parameter.Value;
            else if (parameter.ParameterMode == ParameterMode.Position)
                result = program[parameter.Value];
            else
                throw new Exception(parameter.ParameterMode.ToString());
            return result;
        }

        public static void HandleOutput(List<int> program, Command command, ref int output)
        {
            var first = GetProgramValue(program, command.Parameters[0]);
            output = first;
            Console.WriteLine(first);
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
                    return Opcode.Input;
                case 4:
                    return Opcode.Output;
                case 5:
                    return Opcode.JumpIfTrue;
                case 6:
                    return Opcode.JumpIfFalse;
                case 7:
                    return Opcode.LessThan;
                case 8:
                    return Opcode.Equals;
                case 99:
                    return Opcode.Exit;
                default:
                    throw new ArgumentException(input.ToString());
            }
        }

        public static Command GetNextCommand(List<int> program, ref int currentPosition)
        {
            var commandString = program[currentPosition++].ToString();
            var result = new Command();
            var opCodeDigit = Convert.ToInt32(commandString.Substring(Math.Max(0, commandString.Length - 2)));
            result.Opcode = ConvertToOpcode(opCodeDigit);

            var positionString = commandString.Substring(0, Math.Max(0, commandString.Length - 2));
            var positionStringIndex = positionString.Length;

            for (var x = 0; x < result.RequiredParameters; x++)
            {
                var parameter = new Parameter();
                if (positionStringIndex != 0)
                {
                    var parameterModeValue = Convert.ToInt32(positionString[positionStringIndex - 1].ToString());
                    if (parameterModeValue == 0)
                        parameter.ParameterMode = ParameterMode.Position;
                    else if (parameterModeValue == 1)
                        parameter.ParameterMode = ParameterMode.Immediate;
                    else 
                        throw new Exception(parameterModeValue.ToString());
                    positionStringIndex--;

                }
                else
                {
                    parameter.ParameterMode = ParameterMode.Position;
                }

                parameter.Value = program[currentPosition++];
                result.Parameters.Add(parameter);
            }

            return result;
        }
    }
}
