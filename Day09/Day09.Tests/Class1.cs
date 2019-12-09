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
            Assert.AreEqual("109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99", result);
        }

        [Test]
        public void Second()
        {
            var program = new List<Int64>() { 1102, 34915192, 34915192, 7, 4, 7, 99, 0 };
            var result = Computer.ProcessInput(program);
        }
    }
}
