using System;
using NUnit.Framework;

namespace Day04.Tests
{
    [TestFixture]
    public class PasswordFinderTests
    {
        [TestCase(112233, true)] // valid
        [TestCase(223450, false)] // not increasing
        [TestCase(123789, false)] // no repeating
        [TestCase(123444, false)] // repeating group, no pair
        [TestCase(111122, true)] // repeating group
        public void Validate(int input, bool isValid)
        {
            var result = PasswordFinder.IsValid(input);
            Assert.AreEqual(isValid, result);
        }

        [Test]
        public void ValidRange()
        {
            int start = 264793;
            int end = 803935;
            var result = PasswordFinder.CheckRange(start, end);
        }
    }
}
