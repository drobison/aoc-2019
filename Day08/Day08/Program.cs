using System;
using System.Collections.Generic;
using System.IO;

namespace Day08
{
    public class Program
    {
        static void Main(string[] args)
        {
            var input = LoadData("input.txt");
            var rows = 6;
            var columns = 25;
            var image = BuildArray(input, rows, columns);
            var layerNumber = LayerWithFewest0(image);
            var numberOfOnes = CountNumbers(image, layerNumber, 1);
            var numberOfTwos = CountNumbers(image, layerNumber, 2);
            var result = numberOfOnes * numberOfTwos;
            var decoded = DecodeImage(image);
            Print(decoded);
        }

        public static void Print(int[,] image)
        {
            for (int row = 0; row < image.GetLength(0); row++)
            {
                for (int column = 0; column < image.GetLength(1); column++)
                {
                    Console.Write(image[row, column]);
                }
                Console.WriteLine();
            }
        }

        public static int[,] DecodeImage(int[,,] image)
        {
            int rowCount = image.GetLength(1);
            int columnCount = image.GetLength(0);
            var result = new int[rowCount, columnCount];

            for (int row = 0; row < result.GetLength(0); row++)
            {
                for (int column = 0; column < result.GetLength(1); column++)
                {
                    var depth = 0;
                    while (image[column, row, depth] == 2)
                    {
                        depth++;
                    }

                    result[row, column] = image[column, row, depth];
                }
            }

            return result;
        }

        private static int CountNumbers(int[,,] image, int depth, int target)
        {
            var count = 0;
            for (int row = 0; row < image.GetLength(1); row++)
            {
                for (int column = 0; column < image.GetLength(0); column++)
                {
                    if (image[column, row, depth] == target) count++;
                }
            }

            return count;
        }

        private static int LayerWithFewest0(int[,,] image)
        {
            var layerNumber = 0;
            var min = Int32.MaxValue;

            for (int depth = 0; depth < image.GetLength(2); depth++)
            {
                var numberOfZeros = CountNumbers(image, depth, 0);

                if (numberOfZeros < min)
                {
                    min = numberOfZeros;
                    layerNumber = depth;
                }
            }

            return layerNumber;
        }

        public static int[,,] BuildArray(List<int> input, int rows, int columns)
        {
            int depth = input.Count / (rows) / (columns);
            var result = new int[columns, rows, depth];

            for (int x = 0; x < input.Count; x++)
            {
                var currentDepth = x / (rows * columns);
                var pageInfo = x % (rows * columns);
                var currentRow  = pageInfo / columns;
                var currentColumn = pageInfo % columns;

                result[currentColumn, currentRow, currentDepth] = input[x];
            }

            return result;
        }

        public static List<int> LoadData(string fileName)
        {
            var input = new List<int>();
            using (TextReader reader = File.OpenText(fileName))
            {
                var data = reader.ReadLine();
                foreach (var current in data)
                {
                    input.Add(current - '0');
                }
            }

            return input;
        }
    }
}
