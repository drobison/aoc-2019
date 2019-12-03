using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day02
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = LoadInput("input.txt");
            var noun = 0;
            var verb = 0;
            var answer = 0;
            for (noun = 0; noun <= 99; noun++)
            {
                for (verb = 0; verb <= 99; verb++)
                {
                    var run = new List<int>(program);
                    run[1] = noun;
                    run[2] = verb;
                    var result = Computer.ProcessInput(run);
                    if (result == 19690720)
                    {
                        answer = 100 * noun + verb;
                    }

                }
            }

            program[1] = 12;
            program[2] = 2;
            Computer.ProcessInput(program);
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
