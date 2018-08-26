using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StringCollection
{
    public class StringPopulation
    {
        private IStringCollection _stringCollection = null;

        string[] _wordsToPopulate = null;
        CancellationTokenSource _stringReadTokenSource = new CancellationTokenSource();

        private string _executionName = "No Execution";
        private int _executionIteration = 0;
        DateTime _startTime = DateTime.MinValue;
        DateTime _endTime = DateTime.MinValue;

        Task _statusUpdateTask = null;

        public StringPopulation(string[] wordsToPopulate)
        {
            _wordsToPopulate = wordsToPopulate;

            _statusUpdateTask = new Task(() =>
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine($"{ _executionName }: { _executionIteration} Statistics ({ DateTime.Now })");
                    Console.WriteLine($"  { _stringCollection?.NumberOfWords } of { _wordsToPopulate.Length } words added");
                    Console.WriteLine($"  Words: { _stringCollection.ToString() }");
                    Console.WriteLine($"  Execution Time: { (_endTime - _startTime).TotalMilliseconds }");
                    Thread.Sleep(1000);
                }
            }, _stringReadTokenSource.Token);
        }

        public void StartProgressMonitor()
        {
            _statusUpdateTask.Start();
        }

        public ExecutionStatistics Start(string executionName, int executionIteration, IStringCollection stringCollection, bool withArtificialDelay)
        {
            _stringCollection = stringCollection;
            _executionName = executionName;
            _executionIteration = executionIteration;

            _startTime = DateTime.Now;
            ParallelLoopResult loopResult = Parallel.ForEach(_wordsToPopulate, (word) =>
            {
                _stringCollection.AddString(word);

                if (withArtificialDelay)
                    Thread.Sleep(100);
            });
            _endTime = DateTime.Now;

            SpinWait.SpinUntil(() => loopResult.IsCompleted);

            return new ExecutionStatistics()
            {
                ExecutionTime = _endTime - _startTime,
                Success = _wordsToPopulate.Length == _stringCollection.NumberOfWords
            };
        }

        public void StopProgressMonitor()
        {
            _stringReadTokenSource.Cancel();
        }

        public void Reset()
        {
            _stringCollection.Reset();
        }
    }
}
