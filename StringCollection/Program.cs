using System;
using System.Threading.Tasks;

/// <summary>
/// TODO:
/// 1. implement threadsafe reads and writes
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
            bool threadSafe = true;

            if (args.Length > 0)
            {
                threadSafe = !(args[0] == "0");
            }

            string sentence =
                "After being agreed at the start of the contract, the GFV will not change" +
                " meaning the balloon payment you were quoted at the beginning of the contract" +
                " will not change. This is regardless of current market value of the car at the" +
                " end of the contract";
            string[] words = sentence.Split(' ');

            StringCollection strings = new StringCollection(threadSafe);

            DateTime startTime = DateTime.Now;
            foreach (string word in words)
                strings.AddString(word);
            DateTime endTime = DateTime.Now;

            Console.WriteLine("Word Statistics:");
            Console.WriteLine($"  Words to Add: {words.Length}");
            Console.WriteLine($"  Words Added: {strings.NumberOfWords}");
            Console.WriteLine($"  Execution Time: { (endTime - startTime).TotalMilliseconds }");
            Console.WriteLine($"  Words: {strings.ToString()}");

            Console.ReadLine();

            StringCollection stringsAsync = new StringCollection(threadSafe);

            DateTime startTimeAsync = DateTime.Now;
            Parallel.ForEach(words, (word) =>
            {
                stringsAsync.AddString(word);
            });
            DateTime endTimeAsync = DateTime.Now;

            Console.WriteLine("Word Statistics:");
            Console.WriteLine($"  Words to Add: {words.Length}");
            Console.WriteLine($"  Words Added: {stringsAsync.NumberOfWords}");
            Console.WriteLine($"  Execution Time: { (endTimeAsync - startTimeAsync).TotalMilliseconds }");
            Console.WriteLine($"  Words: {stringsAsync.ToString()}");

            Console.ReadLine();
        }
    }
}
