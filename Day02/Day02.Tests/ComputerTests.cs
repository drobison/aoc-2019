using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Day02.Tests
{
    [TestFixture]
    public class ComputerTests
    {
        [Test]
        public void Test()
        {
            var input = new List<int>(){1,0,0,0,99};
            Computer.ProcessInput(input);
            Assert.AreEqual(2, input[0]);
        }
    }
}
