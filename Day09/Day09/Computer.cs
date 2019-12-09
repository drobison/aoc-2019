using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace Day09
{
    public static class Computer
    {
        private static Int64 _relativeBase = 0;
        public static Int64 ProcessInput(List<Int64> program, ConcurrentQueue<Int64> input = null, ConcurrentQueue<Int64> output = null, bool bypassInput = false)
        {
            var currentPosition = 0;
            var lastOutput = 0;
            var currentInput = 0;
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
                    HandleInput(program, command, ref currentPosition, input, currentInput++, bypassInput);
                }
                else if (command.Opcode == Opcode.Output)
                {
                    HandleOutput(program, command, ref lastOutput, output);
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
                else if (command.Opcode == Opcode.RelativeBase)
                {
                    HandleRelativeBase(program, command);
                }


            } while (command.Opcode != Opcode.Exit);

            return lastOutput;
        }

        private static void HandleRelativeBase(List<Int64> program, Command command)
        {
            var first = GetProgramValue(program, command.Parameters[0]);
            _relativeBase += first;
        }

        private static void HandleJumpIfTrue(List<Int64> program, Command command, ref Int64 currentPosition)
        {
            var first = GetProgramValue(program, command.Parameters[0]);
            var second = GetProgramValue(program, command.Parameters[1]);
            if (first != 0)
            {
                currentPosition = second;
            }
        }

        private static void HandleJumpIfFalse(List<Int64> program, Command command, ref Int64 currentPosition)
        {
            var first = GetProgramValue(program, command.Parameters[0]);
            var second = GetProgramValue(program, command.Parameters[1]);
            if (first == 0)
            {
                currentPosition = second;
            }
        }

        private static void HandleLessThan(List<Int64> program, Command command)
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

        private static void HandleEquals(List<Int64> program, Command command)
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

        private static void HandleAdd(List<Int64> program, Command command)
        {
            var first = GetProgramValue(program, command.Parameters[0]);
            var second = GetProgramValue(program, command.Parameters[1]);
            var destination = command.Parameters[2].Value;
            program[destination] = first + second;
        }

        private static void HandleMultiply(List<Int64> program, Command command)
        {
            var first = GetProgramValue(program, command.Parameters[0]);
            var second = GetProgramValue(program, command.Parameters[1]);
            var destination = command.Parameters[2].Value;
            program[destination] = first * second;
        }

        private static void HandleInput(List<Int64> program, Command command, ref Int64 currentPosition, ConcurrentQueue<Int64> input, Int64 currentInput, bool bypassInput)
        {
            int inputResult;
            if (!bypassInput)
            {
                inputResult = ReadFromConsole();
            }
            else
            {
                while (input.IsEmpty)
                {
                    Thread.Sleep(50);
                }

                input.TryDequeue(out inputResult);
            }

            var destination = command.Parameters[0].Value;
            program[destination] = inputResult;
        }

        private static Int64 ReadFromConsole()
        {
            Console.WriteLine("Please enter integer input:");
            var input = Console.ReadLine();
            var validInput = Int64.TryParse(input, out var inputResult);
            if (!validInput)
            {
                Console.WriteLine("Input was not an integer, exiting");
                Environment.Exit(-1);
            }

            return inputResult;
        }

        public static Int64 GetProgramValue(List<Int64> program, Parameter parameter)
        {
            int result;
            if (parameter.ParameterMode == ParameterMode.Immediate)
                result = parameter.Value;
            else if (parameter.ParameterMode == ParameterMode.Position)
                result = program[parameter.Value];
            else if (parameter.ParameterMode == ParameterMode.Relative)
                result = program[parameter.Value + _relativeBase];
            else
                throw new Exception(parameter.ParameterMode.ToString());
            return result;
        }

        public static void HandleOutput(List<Int64> program, Command command, ref Int64 output, ConcurrentQueue<Int64> outputList)
        {
            var first = GetProgramValue(program, command.Parameters[0]);
            output = first;
            if (outputList != null)
            {
                outputList.Enqueue(first);
            }
            else
            {
                Console.WriteLine(first);
            }
        }

        public static Command GetNextCommand(List<Int64> program, ref int currentPosition)
        {
            var commandString = program[currentPosition++].ToString();
            var result = new Command();
            var opCodeDigit = Convert.ToInt32(commandString.Substring(Math.Max(0, commandString.Length - 2)));
            result.Opcode = (Opcode)opCodeDigit;

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
                    else if (parameterModeValue == 2)
                        parameter.ParameterMode = ParameterMode.Relative;
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