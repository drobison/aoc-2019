using System;
using System.Collections.Generic;
using System.Linq;

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
                command = GetNextCommand(program, currentPosition++);

                if (command.Opcode == Opcode.Add)
                {
                    HandleAdd(program, command, ref currentPosition);
                }
                else if (command.Opcode == Opcode.Multiply)
                {
                    HandleMultiply(program, command, ref currentPosition);
                }
                else if (command.Opcode == Opcode.Input)
                {
                    HandleInput(program, command, ref currentPosition, bypassInput);
                }
                else if (command.Opcode == Opcode.Output)
                {
                    HandleOutput(program, command, ref currentPosition, ref lastOutput);
                }

            } while (command.Opcode != Opcode.Exit);

            return lastOutput;
        }

        private static void HandleAdd(List<int> program, Command command, ref int currentPosition)
        {
            var first = GetProgramValue(program, command.ParameterModes[0], ref currentPosition);
            var second = GetProgramValue(program, command.ParameterModes[1], ref currentPosition);
            var destination = program[currentPosition++];
            program[destination] = first + second;
        }

        public static int GetProgramValue(List<int> program, ParameterMode mode, ref int currentPosition)
        {
            int result;
            if (mode == ParameterMode.Immediate)
                result = program[currentPosition];
            else if (mode == ParameterMode.Position)
                result = program[program[currentPosition]];
            else
                throw new Exception(mode.ToString());
            currentPosition++;
            return result;
        }

        private static void HandleMultiply(List<int> program, Command command, ref int currentPosition)
        {
            var first = GetProgramValue(program, command.ParameterModes[0], ref currentPosition);
            var second = GetProgramValue(program, command.ParameterModes[1], ref currentPosition);
            var destination = program[currentPosition++];
            program[destination] = first * second;
        }

        private static void HandleInput(List<int> program, Command command, ref int currentPosition, bool bypassInput)
        {
            int inputResult;
            if (bypassInput)
            {
                inputResult = 1;
            }
            else
            {
                Console.WriteLine("Please enter integer input:");
                var input = Console.ReadLine();
                var validInput = int.TryParse(input, out inputResult);
                if (!validInput)
                {
                    Console.WriteLine("Input was not an integer, exiting");
                    Environment.Exit(-1);
                }
            }

            var destination = program[currentPosition++];
            program[destination] = inputResult;
        }

        public static void HandleOutput(List<int> program, Command command, ref int currentPosition, ref int output)
        {
            var first = GetProgramValue(program, command.ParameterModes[0], ref currentPosition);
            output = first;
            Console.WriteLine(first);
        }

        private static Command GetNextCommand(List<int> program, int currentPosition)
        {
            return GetNextCommand(program[currentPosition]);
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
                case Opcode.Input:
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
            var commandString = command.ToString();
            var result = new Command();
            var opCodeDigit = Convert.ToInt32(commandString.Substring(Math.Max(0, commandString.Length - 2)));
            result.Opcode = ConvertToOpcode(opCodeDigit);
            var numberOfParameters = GetRequiredParameters(result.Opcode);

            var positionString = commandString.Substring(0, Math.Max(0, commandString.Length - 2));
            var positionStringIndex = positionString.Length;

            for (var x = 0; x < numberOfParameters; x++)
            {
                if (positionStringIndex != 0)
                {
                    var parmeterModeValue = Convert.ToInt32(positionString[positionStringIndex - 1].ToString());
                    if (parmeterModeValue == 0)
                        result.ParameterModes.Add(ParameterMode.Position);
                    else if (parmeterModeValue == 1)
                        result.ParameterModes.Add(ParameterMode.Immediate);
                    else 
                        throw new Exception(parmeterModeValue.ToString());
                    positionStringIndex--;

                }
                else
                {
                    result.ParameterModes.Add(ParameterMode.Position);
                }
            }

            return result;
        }
    }

    public class Command
    {
        public Command()
        {
            ParameterModes = new List<ParameterMode>();
        }
        public Opcode Opcode { get; set; }

        public List<ParameterMode> ParameterModes { get; set; }
    }

    public enum ParameterMode
    {
        Position,
        Immediate
    }

}
