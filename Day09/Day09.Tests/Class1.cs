using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Day09.Tests
{ 

    [TestFixture]
    public class Class1
    {
        [Test]
        public void First()
        {
            var program = new List<Int64>() { 109, 1, 204, -1, 1001, 100, 1, 100, 1008, 100, 16, 101, 1006, 101, 0, 99 };
            var result = Computer.ProcessInput(program);
            Assert.AreEqual(99, result); // should create output queue and validate it matches with input
        }

        [Test]
        public void Second()
        {
            var program = new List<Int64>() { 1102, 34915192, 34915192, 7, 4, 7, 99, 0 };
            var result = Computer.ProcessInput(program);
        }

        [Test]
        public void Third()
        {
            var program = new List<Int64>() { 104, 1125899906842624, 99 };
            var result = Computer.ProcessInput(program);
            Assert.AreEqual(1125899906842624, result);
        }

        [Test]
        public void ErrorAdd()
        {
            var currentPosition = 0;
            var program = new List<Int64>() {1001,100,1,100}; // 1 0 01 
            //var result = Computer.GetNextCommand(program, ref currentPosition);
            //Assert.AreEqual(ParameterMode.Position, result.Parameters[0].ParameterMode);
            //Assert.AreEqual(ParameterMode.Immediate, result.Parameters[1].ParameterMode);
            //Assert.AreEqual(ParameterMode.Position, result.Parameters[2].ParameterMode);
        }

        [Test]
        public void IsItRelative()
        {
            Computer.SetRelativeBase(2000);
            var program = new List<Int64>() { };
            var size = 2000;
            for (int x = 0; x < size; x++)
            {
                program.Add(0);
            }

            program[0] = 109;
            program[1] = 19;
            program[2] = 204;
            program[3] = -34;
            program[1985] = -1;
            Computer.ProcessInput(program);
            Assert.AreEqual(2019, Computer.GetRelativeBase());

        }

        [Test]
        public void ParameterMode()
        {
            var program = new List<Int64>() { 109, 1, 3, 3, 204, 2, 99};
            var result = Computer.ProcessInput(program);
            Assert.AreEqual(204, result);
        }
    }
}
