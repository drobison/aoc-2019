using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace Day09
{
    public static class Computer
    {
        private static Int64 _relativeBase = 0;

        public static void SetRelativeBase(int input)
        {
            _relativeBase = input;
        }

        public static Int64 GetRelativeBase()
        {
            return _relativeBase;
        }
        public static Int64 ProcessInput(List<Int64> program, ConcurrentQueue<Int64> input = null, ConcurrentQueue<Int64> output = null, bool bypassInput = false)
        {
            var currentPosition = 0;
            Int64 lastOutput = 0;
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

        private static void HandleJumpIfTrue(List<Int64> program, Command command, ref int currentPosition)
        {
            var first = GetProgramValue(program, command.Parameters[0]);
            var second = Convert.ToInt32(GetProgramValue(program, command.Parameters[1]));
            if (first != 0)
            {
                EnsureSize(program, second);
                currentPosition = second;
            }
        }

        private static void HandleJumpIfFalse(List<Int64> program, Command command, ref int currentPosition)
        {
            var first = GetProgramValue(program, command.Parameters[0]);
            var second = Convert.ToInt32(GetProgramValue(program, command.Parameters[1]));
            if (first == 0)
            {
                EnsureSize(program, second);
                currentPosition = second;
            }
        }

        private static void HandleLessThan(List<Int64> program, Command command)
        {
            var first = GetProgramValue(program, command.Parameters[0]);
            var second = GetProgramValue(program, command.Parameters[1]);
            var destination = GetWriteIndex(command.Parameters[2]);
            EnsureSize(program, destination);

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
            var destination = GetWriteIndex(command.Parameters[2]);
            EnsureSize(program, destination);

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
            var destination = GetWriteIndex(command.Parameters[2]);
            EnsureSize(program, destination);
            program[destination] = first + second;
        }

        private static void HandleMultiply(List<Int64> program, Command command)
        {
            var first = GetProgramValue(program, command.Parameters[0]);
            var second = GetProgramValue(program, command.Parameters[1]);
            var destination = GetWriteIndex(command.Parameters[2]);

            EnsureSize(program, destination);
            program[destination] = first * second;
        }

        private static void HandleInput(List<Int64> program, Command command, ref int currentPosition, ConcurrentQueue<Int64> input, int currentInput, bool bypassInput)
        {
            Int64 inputResult;
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

            var destinationPosition = GetWriteIndex(command.Parameters[0]);
            program[destinationPosition] = inputResult;
        }

        public static int GetWriteIndex(Parameter parameter)
        {
            return Convert.ToInt32(parameter.ParameterMode == ParameterMode.Position ? parameter.Value : parameter.Value + _relativeBase);
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
            Int64 result;
            if (parameter.ParameterMode == ParameterMode.Immediate)
                result = parameter.Value;
            else if (parameter.ParameterMode == ParameterMode.Position)
            {
                EnsureSize(program, parameter.Value);
                result = program[Convert.ToInt32(parameter.Value)];
            }
            else if (parameter.ParameterMode == ParameterMode.Relative)
            {
                EnsureSize(program, parameter.Value + _relativeBase);
                result = program[Convert.ToInt32(parameter.Value + _relativeBase)];
            }
            else
                throw new Exception(parameter.ParameterMode.ToString());
            return result;
        }

        private static void EnsureSize(List<long> program, in long size)
        {
            var missingCapacity = size + 1 - program.Count;
            if(missingCapacity > 0)
            {
                for(int x = 0; x < missingCapacity; x++)
                {
                    program.Add(0);
                }
            }

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