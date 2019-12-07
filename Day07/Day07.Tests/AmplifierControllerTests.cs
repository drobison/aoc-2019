using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Day07.Tests
{
    [TestFixture]
    public class AmplifierControllerTests
    {
        [Test]
        public void Test1()
        {
            var program = new List<int>() {3, 15, 3, 16, 1002, 16, 10, 16, 1, 16, 15, 15, 4, 15, 99, 0, 0};
            var permutation = new List<int>(){ 4,3,2,1,0 };
            var result = AmplifierController.ProcessPermutation(program, permutation);
            Assert.AreEqual(43210, result);
        }

        [Test]
        public void Test2()
        {
            var program = new List<int>() { 3,23,3,24,1002,24,10,24,1002,23,-1,23,
                101,5,23,23,1,24,23,23,4,23,99,0,0 };
            var permutation = new List<int>() { 0, 1, 2, 3, 4 };
            var result = AmplifierController.ProcessPermutation(program, permutation);
            Assert.AreEqual(54321, result);
        }

        [Test]
        public void Test3()
        {
            var program = new List<int>() { 3,31,3,32,1002,32,10,32,1001,31,-2,31,1007,31,0,33,
                1002,33,7,33,1,33,31,31,1,32,31,31,4,31,99,0,0,0 };
            var permutation = new List<int>() { 1, 0, 4, 3, 2 };
            var result = AmplifierController.ProcessPermutation(program, permutation);
            Assert.AreEqual(65210, result);
        }
    }
}
