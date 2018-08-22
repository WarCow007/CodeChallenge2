using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StringCollection
{
    public class StringPopulation<TStringCollection>
        where TStringCollection : IStringCollection, new()
    {
        private TStringCollection _stringCollection = new TStringCollection();

        string[] _wordsToPopulate = null;
        CancellationTokenSource _stringReadTokenSource = new CancellationTokenSource();

        DateTime _startTime = DateTime.MinValue;
        DateTime _endTime = DateTime.MinValue;

        public StringPopulation(string[] wordsToPopulate)
        {
            _wordsToPopulate = wordsToPopulate;
        }

        public void Start(string executionName, bool withArtificialDelay)
        {
            Task statusUpdateTask = new Task(() =>
            {
                while (!_stringReadTokenSource.Token.IsCancellationRequested)
                {
                    Console.Clear();
                    Console.WriteLine($"{ executionName } Execution ({ DateTime.Now })");
                    Console.WriteLine($"  { _stringCollection.NumberOfWords } of { _wordsToPopulate.Length } words added");
                    Console.WriteLine($"  Words: { _stringCollection.ToString() }");
                    Thread.Sleep(500);
                }
            }, _stringReadTokenSource.Token);

            statusUpdateTask
                .ContinueWith((task) =>
                {
                    Console.Clear();
                    Console.WriteLine($"{ executionName } Statistics ({ DateTime.Now })");
                    Console.WriteLine($"  { _stringCollection.NumberOfWords } of { _wordsToPopulate.Length } words added");
                    Console.WriteLine($"  Words: { _stringCollection.ToString() }");
                    Console.WriteLine($"  Execution Time: { (_endTime - _startTime).TotalMilliseconds }");
                });

            statusUpdateTask.Start();

            _startTime = DateTime.Now;
            Parallel.ForEach(_wordsToPopulate, (word) =>
            {
                _stringCollection.AddString(word);

                if (withArtificialDelay)
                    Thread.Sleep(100);
            });
            _endTime = DateTime.Now;

            _stringReadTokenSource.Cancel();
            Console.ReadLine();
        }
    }
}
