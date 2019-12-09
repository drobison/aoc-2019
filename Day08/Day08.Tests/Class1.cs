using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Day08.Tests
{
    [TestFixture]
    public class Class1
    {
        [Test]
        public void BuildArray()
        {
            var input = new List<int>()
            {
                1,2,3,
                4,5,6,

                7,8,9,
                0,1,2
            };
            var width = 3;
            var height = 2;
            var result = Program.BuildArray(input, height, width);
            // rows / height
            Assert.AreEqual(2, result.GetLength(1));
            // columns / width
            Assert.AreEqual(3, result.GetLength(0));
            // depth
            Assert.AreEqual(2, result.GetLength(2));
            // Data
            Assert.AreEqual(1, result[0,0,0]);
        }

        [Test]
        public void DecodeImage()
        {
            var input = new List<int>()
            {
                0,2,
                2,2,
                
                1,1,
                2,2,
                
                2,2,
                1,2,

                0,0,
                0,0
            };
            var width = 2;
            var height = 2;
            var result = Program.BuildArray(input, height, width);
            var decoded = Program.DecodeImage(result);

            Assert.AreEqual(0, decoded[0,0]);
            Assert.AreEqual(1, decoded[0, 1]);
            Assert.AreEqual(1, decoded[1, 0]);
            Assert.AreEqual(0, decoded[1, 1]);
            Program.Print(decoded);
        }
    }
}
