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
            bool withArtificialDelay = false;
            int numberOfIterations = 100000;

            if (args.Length > 0)
            {
                withArtificialDelay = args[0] == "1";
            }

            string[] words = Constants.WordData.Split(' ');

            StringPopulation stringPopulation = new StringPopulation(words);
            StringLockCollection lockStrings = new StringLockCollection();
            StringCollection nonBlockingStrings = new StringCollection();

            List<ExecutionStatistics> lockStringsSuccess = new List<ExecutionStatistics>();
            List<ExecutionStatistics> nonBlockingStringsSuccess = new List<ExecutionStatistics>();

            stringPopulation.StartProgressMonitor();

            for (int lockCycleIndex = 0; lockCycleIndex < numberOfIterations; lockCycleIndex++)
            {
                lockStringsSuccess.Add(stringPopulation.Start("Lock", lockCycleIndex, lockStrings, withArtificialDelay));
                lockStrings.Reset();
            }

            for (int lockCycleIndex = 0; lockCycleIndex < numberOfIterations; lockCycleIndex++)
            {
                nonBlockingStringsSuccess.Add(stringPopulation.Start("NonBlocking", lockCycleIndex, nonBlockingStrings, withArtificialDelay));
                nonBlockingStrings.Reset();
            }

            stringPopulation.StopProgressMonitor();

            Console.Clear();
            Console.WriteLine("*** Lock Statistics ***");
            Console.WriteLine($" Words: { lockStringsSuccess.FindAll(e => e.Success).Count } of { lockStringsSuccess.Count } added");
            Console.WriteLine($" Time (avg): { lockStringsSuccess.Average(e => e.ExecutionTime.Milliseconds) }");
            Console.WriteLine("*** Non Blocking Statistics ***");
            Console.WriteLine($" Words: { nonBlockingStringsSuccess.FindAll(e => e.Success).Count } of { nonBlockingStringsSuccess.Count } * **");
            Console.WriteLine($" Time (avg): { nonBlockingStringsSuccess.Average(e => e.ExecutionTime.Milliseconds) }");
            Console.ReadLine();
        }
    }
}
