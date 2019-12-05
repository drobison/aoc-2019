using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day05
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
        public static List<int> LoadInput(string fileName)
        {
            using (TextReader reader = File.OpenText(fileName))
            {
                string currentLine = reader.ReadLine();
                if (currentLine == null) throw new ArgumentException();

                return currentLine.Split(",").Select(x => Convert.ToInt32(x)).ToList();
            }
        }
    }

}
