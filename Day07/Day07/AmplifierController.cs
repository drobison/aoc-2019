using System;
using System.Collections.Generic;
using System.Linq;

namespace Day07
{
    public static class AmplifierController
    {

        public static IEnumerable<IEnumerable<int>> GetPermutations(List<int> input)
        {
            var result = input.Permute();
            return result;
        }

        public static int FindMaxPermutation(List<int> program, int numberOfAmplifiers)
        {
            var values = Enumerable.Range(0, numberOfAmplifiers).Select(x => x).ToList();
            var permutations = GetPermutations(values);

            var max = Int32.MinValue;
            foreach (var permutation in permutations)
            {
                max = Math.Max(max, ProcessPermutation(new List<int>(program), permutation.ToList()));
            }

            return max;
        }

        public static int ProcessPermutation(List<int> program, List<int> input)
        {
            int result = 0;
            foreach (var currentInput in input)
            {
                result = Computer.ProcessInput(program, new List<int>(){currentInput, result});
            }

            return result;
        }

        public static IEnumerable<IEnumerable<T>> Permute<T>(this IEnumerable<T> sequence)
        {
            if (sequence == null)
            {
                yield break;
            }

            var list = sequence.ToList();

            if (!list.Any())
            {
                yield return Enumerable.Empty<T>();
            }
            else
            {
                var startingElementIndex = 0;

                foreach (var startingElement in list)
                {
                    var index = startingElementIndex;
                    var remainingItems = list.Where((e, i) => i != index);

                    foreach (var permutationOfRemainder in remainingItems.Permute())
                    {
                        yield return startingElement.Concat(permutationOfRemainder);
                    }

                    startingElementIndex++;
                }
            }
        }

        private static IEnumerable<T> Concat<T>(this T firstElement, IEnumerable<T> secondSequence)
        {
            yield return firstElement;
            if (secondSequence == null)
            {
                yield break;
            }

            foreach (var item in secondSequence)
            {
                yield return item;
            }
        }
    }
}
