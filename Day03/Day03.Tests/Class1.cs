using System;
using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;

namespace Day03.Tests
{
    [TestFixture]
    public class Class1
    {
        [Test]
        public void LineLength()
        {
            var result = Program.LineLength(new PointF(0f, 0f), new PointF(2f, 2f));
            Assert.AreEqual(3, result);
        }

        [Test]
        public void LineLengthManhattanDistance()
        {
            var result = Program.ManhattanDistanceFromCenter(new PointF(3f, 3f));
            Assert.AreEqual(6, result);
        }

        [Test]
        public void MinMan()
        {
            var path1 = Program.ParsePoints(new List<string>(){"R8", "U5", "L5", "D3"});
            var path2 = Program.ParsePoints(new List<string>() { "U7", "R6", "D4", "L4" });
            var result = Program.FindMinManPath(path1, path2);
            Assert.AreEqual(30, result);
        }
    }
}
