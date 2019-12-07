using System;
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
    }
}
