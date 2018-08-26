using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private Stopwatch _stopWatch = new Stopwatch();

        Task _statusUpdateTask = null;

        public StringPopulation(string[] wordsToPopulate)
        {
            _wordsToPopulate = wordsToPopulate;

            _statusUpdateTask = new Task(() =>
            {
                while (!_stringReadTokenSource.Token.IsCancellationRequested)
                {
                    Console.Clear();
                    Console.WriteLine($"{ _executionName } Statistics ({ DateTime.Now })");
                    Console.WriteLine($"  { _stringCollection?.NumberOfWords } of { _wordsToPopulate.Length } words added");
                    Console.WriteLine($"  Words: { _stringCollection.ToString() }");
                    Thread.Sleep(500);
                }
            });
        }

        public void Initialize()
        {
            _statusUpdateTask.Start();
        }

        public List<ExecutionStatistics> Start<TStringCollection>(string executionName, int numberOfExecutionIterations)
            where TStringCollection : IStringCollection, new()
        {
            _stringCollection = new TStringCollection();
            _executionName = executionName;

            List<ExecutionStatistics> executionStatistics = new List<ExecutionStatistics>();

            for (int executionIteration = 0; executionIteration < numberOfExecutionIterations; executionIteration++)
            {
                _stopWatch.Start();
                ParallelLoopResult loopResult = Parallel.ForEach(_wordsToPopulate, (word) =>
                {
                    _stringCollection.AddString(word);
                });
                _stopWatch.Stop();

                SpinWait.SpinUntil(() => loopResult.IsCompleted);

                executionStatistics.Add(new ExecutionStatistics()
                {
                    WordCount = _wordsToPopulate.Length,
                    PopulatedWordCount = _stringCollection.NumberOfWords,
                    ExecutionTime = _stopWatch.Elapsed,
                });

                _stringCollection.Reset();
                _stopWatch.Reset();
            }

            return executionStatistics;
        }

        public void Terminate()
        {
            _stringReadTokenSource.Cancel();
            SpinWait.SpinUntil(() => _statusUpdateTask.IsCompleted);
        }
    }
}
