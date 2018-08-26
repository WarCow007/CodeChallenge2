using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

/// <summary>
/// This code implements a parallel task to monitors changes to the collection, while a parallel
/// ForEach provides for the addition of strings.
/// </summary>
namespace StringCollection
{
    /// <summary>
    /// The code executes asyn and non async so that we can compare the execution
    /// time and the results.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            int numberOfIterations = 100000;

            if (args.Length > 0)
            {
                int tempIterations = 0;

                if (int.TryParse(args[0], out tempIterations))
                    numberOfIterations = tempIterations;
            }

            string[] words = Constants.WordData.Split(' ');

            StringPopulation stringPopulation = new StringPopulation(words);

            List<ExecutionStatistics> lockStringsSuccess = new List<ExecutionStatistics>();
            List<ExecutionStatistics> nonBlockingStringsSuccess = new List<ExecutionStatistics>();

            stringPopulation.Initialize();

            lockStringsSuccess = stringPopulation.Start<StringLockCollection>("Lock", numberOfIterations);
            nonBlockingStringsSuccess = stringPopulation.Start<StringCollection>("NonBlocking", numberOfIterations);

            stringPopulation.Terminate();

            Console.Clear();
            Console.WriteLine("*** Lock Statistics ***");
            Console.WriteLine($" Words Added: { lockStringsSuccess.Sum(e => e.PopulatedWordCount) } of { lockStringsSuccess.Sum(e => e.WordCount) }");
            Console.WriteLine($" Iteration Success: { lockStringsSuccess.FindAll(e => e.Success).Count } of { lockStringsSuccess.Count }");
            Console.WriteLine($" Time (avg): { lockStringsSuccess.Average(e => e.ExecutionTime.Milliseconds) }");
            Console.WriteLine("");
            Console.WriteLine("*** Non Blocking Statistics ***");
            Console.WriteLine($" Words Added: { nonBlockingStringsSuccess.Sum(e => e.PopulatedWordCount) } of { nonBlockingStringsSuccess.Sum(e => e.WordCount) }");
            Console.WriteLine($" Iteration Success: { nonBlockingStringsSuccess.FindAll(e => e.Success).Count } of { nonBlockingStringsSuccess.Count }");
            Console.WriteLine($" Time (avg): { nonBlockingStringsSuccess.Average(e => e.ExecutionTime.Milliseconds) }");
            Console.ReadLine();
        }
    }
}
