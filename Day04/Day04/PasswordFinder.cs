using System;
using System.Collections.Generic;

namespace Day04
{
    public class PasswordFinder
    {
        public static int CheckRange(int start, int end)
        {
            var possiblePasswords = new List<int>();

            for (int index = start; index <= end; index++)
            {
                if (IsValid(index))
                    possiblePasswords.Add(index);
            }

            return possiblePasswords.Count;
        }

        public static bool IsValid(int input)
        {
            var inputString = input.ToString();
            int lastDigit = Convert.ToInt32(inputString[0]);
            bool hasConsectiveMatching = false;
            int inRepeatingRun = 1;
            for (var index = 1; index < inputString.Length; index++)
            {
                var currentDigit = Convert.ToInt32(inputString[index]);
                if (currentDigit < lastDigit) return false;
                if (currentDigit == lastDigit)
                {
                    inRepeatingRun++;
                }
                else if (currentDigit != lastDigit && inRepeatingRun == 2)
                {
                    hasConsectiveMatching = true;
                    inRepeatingRun = 1;
                }
                else
                {
                    inRepeatingRun = 1;
                }
                lastDigit = currentDigit;
            }

            if (inRepeatingRun == 2) hasConsectiveMatching = true;

            return hasConsectiveMatching;
        }
    }
}