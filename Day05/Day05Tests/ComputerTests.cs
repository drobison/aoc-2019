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
        }
    }
}
