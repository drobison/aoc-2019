using System.Collections.Generic;
using System.IO;

namespace Day01
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = LoadInput("input.txt");
            var result = Fuel.CalculateRocketShipFuel(input);
        }

        public static List<int> LoadInput(string fileName)
        {
            var input = new List<int>();
            using (TextReader reader = File.OpenText(fileName))
            {
                string currentLine;
                while ((currentLine = reader.ReadLine()) != null)
                {
                    var current = int.Parse(currentLine);
                    input.Add(current);
                }
            }

            return input;
        }
    }
}
