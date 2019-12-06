using Day05;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Day05Tests
{
    [TestFixture]
    public class ComputerTests
    {
        [Test]
        public void Test()
        {
            var input = new List<int>() { 1, 0, 0, 0, 99 };
            Computer.ProcessInput(input);
            Assert.AreEqual(2, input[0]);
        }

        [Test]
        public void Test2()
        {
            var input = new List<int>() {1002, 4, 3, 4, 33};
            Computer.ProcessInput(input);

        }

        [Test]
        public void GetCommand()
        {
            var input = 1002;
            var result = Computer.GetNextCommand(input);
            Assert.AreEqual(Opcode.Multiply, result.Opcode);
            Assert.AreEqual(3, result.ParameterModes.Count);
            Assert.AreEqual(ParameterMode.Position, result.ParameterModes[0]);
            Assert.AreEqual(ParameterMode.Immediate, result.ParameterModes[1]);
            Assert.AreEqual(ParameterMode.Position, result.ParameterModes[2]);
        }

        [Test]
        public void ProcessInput()
        {
            var input = new List<int>() { 1002, 4, 3, 4, 33 };
            Computer.ProcessInput(input);
            Assert.AreEqual(99, input[4]);
        }

        [Test]
        public void ProcessInput2()
        {
            var input = new List<int>() { 1101, 100, -1, 4, 0 };
            Computer.ProcessInput(input);
            Assert.AreEqual(99, input[4]);
        }

        [Test]
        public void ProcessInput3()
        {
            var input = new List<int>() { 101, -760, 224, 224 }; 
            // 1 01
            // add, 1 - immediate, positional, positional
            // -760, program[224], program[224]
            
            Computer.ProcessInput(input);
            Assert.AreEqual(99, input[4]);
        }

        [Test]
        public void FullProgram()
        {
            var program = Program.LoadInput("input.txt");

            var result = Computer.ProcessInput(program, true);

            Assert.AreEqual(2845163, result);
        }
    }
}
