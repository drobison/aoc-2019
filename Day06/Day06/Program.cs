using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day06
{
    class Program
    {
        private static List<Object> result = new List<Object>();
        private static HashSet<string> visited = new HashSet<string>();

        static void Main(string[] args)
        {
            var input = LoadFile("input.txt");
            ParseInput(input);
            var count = CountOrbits();
            var start = result.First(x => x.Name == "YOU");
            var end = result.First(x => x.Name == "SAN");
            var pathLength = BreadthSearch(start, end);
        }

        private static int BreadthSearch(Object start, Object end)
        {
            var current = new List<Object>();
            var next = new List<Object>();
            var length = -1;
            current.AddRange(start.Orbiters);
            current.Add(start.Orbits);

            while (current.Any()) 
            {
                foreach (var o in current)
                {
                    if (o == null || visited.Contains(o.Name)) continue;
                    visited.Add(o.Name);
                    if (o == end) return length;
                    next.AddRange(o.Orbiters);
                    next.Add(o.Orbits);

                }

                current = next;
                next = new List<Object>();
                length++;
            }

            return -1;
        }

        private static List<Object> ParseInput(List<string> input)
        {
            var objects = new List<Object>();
            foreach (var record in input)
            {
                ParseLine(record);
            }

            return objects;
        }

        private static int CountOrbits()
        {
            int count = 0;
            foreach (var o in result)
            {
                var next = o.Orbits;
                while (next != null)
                {
                    count++;
                    next = next.Orbits;
                }
            }

            return count;
        }

        private static void ParseLine(string record)
        {
            var names = record.Split(')');
            var object1 = GetByName(names[0]);
            var object2 = GetByName(names[1]);
            object2.Orbits = object1;
            object1.Orbiters.Add(object2);
        }

        private static Object GetByName(string name)
        {
            var item = result.FirstOrDefault(x => x.Name == name);
            if (item == null)
            {
                item = new Object(){Name = name};
                result.Add(item);
            }

            return item;
        }

        public static List<string> LoadFile(string fileName)
        {
            var input = new List<string>();
            string currentLine;
            using (TextReader reader = File.OpenText(fileName))
            {
                while ((currentLine = reader.ReadLine()) != null)
                {
                    input.Add(currentLine);
                }
            }

            return input;
        }
    }

    public class Object
    {
        public Object()
        {
            Orbiters = new List<Object>();
        }

        public string Name { get; set; }
        public Object Orbits { get; set; }
        public List<Object> Orbiters { get; set; }
    }
}
