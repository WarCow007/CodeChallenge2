using System;
using System.Collections.Generic;
using System.Text;

namespace StringCollection
{
    public class ExecutionStatistics
    {
        public int WordCount { get; set; }
        public int PopulatedWordCount { get; set; }
        public TimeSpan ExecutionTime { get; set; }
        public bool Success { get { return WordCount == PopulatedWordCount; } }
    }
}
