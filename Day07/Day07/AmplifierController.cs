using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
            var values = Enumerable.Range(5, numberOfAmplifiers).Select(x => x).ToList();
            var permutations = GetPermutations(values);
            var results = new int[permutations.Count()];

            var max = Int32.MinValue;
            Parallel.ForEach(permutations, (current, state, index) =>
            {
                results[index] =  ProcessPermutationWithFeedbackLoop(new List<int>(program), current.ToList());

            });
            //foreach (var permutation in permutations)
            //{
            //    max = Math.Max(max, ProcessPermutationWithFeedbackLoop(new List<int>(program), permutation.ToList()));
            //}
            return results.Max();
        }

        public static int ProcessPermutation(List<int> program, List<int> input)
        {
            int result = 0;
            foreach (var currentInput in input)
            {
                //result = Computer.ProcessInput(program, new List<int>(){currentInput, result});
            }

            return result;
        }

        public static int ProcessPermutationWithFeedbackLoop(List<int> program, List<int> input)
        {
            var results = new int[input.Count];
            var queues = new List<ConcurrentQueue<int>>();
            var tasks = new Task[input.Count];

            // Initilize input queues
            foreach (var currentInput in input)
            {
                var queue = new ConcurrentQueue<int>();
                queue.Enqueue(currentInput);
                queues.Add(queue);
            }
            queues[0].Enqueue(0);
            


            for (int x = 0; x < input.Count; x++)
            {
                var currentValue = x;
                var inputQueue = queues[x];
                var outputQueue = queues[(x + 1) % input.Count()];
                var task = new Task(() => results[currentValue] = Computer.ProcessInput(new List<int>(program), inputQueue, outputQueue, true));
                tasks[x] = task;
                task.Start();
            }

            Task.WaitAll(tasks);
            return results[input.Count - 1];
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
