using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

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
            string sentence =
                "The mass and composition of the cent changed to the current copper plated zinc core in 1982. Both types were minted in 1982 with no distinguishing mark. " +
                "Cents minted in 1943 were struck on planchets punched from zinc coated steel which left the resulting edges uncoated. This caused many of these coins to " +
                "rust. These 'steel pennies' are not likely to be found in circulation today, as they were later intentionally removed from circulation for recycling the metal. " +
                "However, cents minted from 1944 to 1946 were made from a special salvaged WWII brass composition to replace the steel cents, but still save material for the war " +
                "effort, and are more common in circulation than their 1943 counterparts. The wheat cent was mainstream and common during its time. Some dates are rare, but many " +
                "can still be found in circulation.This is partially due to the fact that unlike the formerly silver denominations(dollar, half, quarter, dime), the composition of " +
                "the pre - 1982 cent, nearly pure copper, is not so much more valuable over face value for it to be hoarded to the extreme extent of the silver denominations. ";

            string[] words = sentence.Split(' ');
            CancellationTokenSource stringReadTokenSource = new CancellationTokenSource();
            DateTime startTimeAsync = DateTime.Now;
            DateTime endTimeAsync = DateTime.Now;

            StringCollection strings = new StringCollection();
            Task statusUpdateTask = new Task(() =>
            {
                while (!stringReadTokenSource.Token.IsCancellationRequested)
                {
                    Console.Clear();
                    Console.WriteLine($"Execution ({DateTime.Now})");
                    Console.WriteLine($"  {strings.NumberOfWords} of {words.Length} words added");
                    Console.WriteLine($"  Words: {strings.ToString()}");
                    Thread.Sleep(500);
                }
            }, stringReadTokenSource.Token);

            statusUpdateTask
                .ContinueWith((task) =>
                {
                    Console.Clear();
                    Console.WriteLine($"Statistics ({DateTime.Now})");
                    Console.WriteLine($"  {strings.NumberOfWords} of {words.Length} words added");
                    Console.WriteLine($"  Words: {strings.ToString()}");
                    Console.WriteLine($"  Execution Time: { (endTimeAsync - startTimeAsync).TotalMilliseconds }");
                });

            statusUpdateTask.Start();

            startTimeAsync = DateTime.Now;
            Parallel.ForEach(words, (word) =>
            {
                strings.AddString(word);
                SpinWait.SpinUntil(() => false, 100);
            });
            endTimeAsync = DateTime.Now;

            Console.ReadLine();
            stringReadTokenSource.Cancel();
            Console.ReadLine();
        }
    }
}
