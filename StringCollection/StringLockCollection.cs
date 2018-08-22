using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace StringCollection
{
    /// <summary>
    /// The lock access uses Interlocked to protect the lock flag and control access to the
    /// read write operations. This avoids the latency when waking up threads in the situation
    /// where we use a lock.
    /// </summary>
    public class StringLockCollection : IStringCollection
    {
        private List<string> _stringList = new List<string>();

        public StringLockCollection()
        {
        }

        public int NumberOfWords { get { return _stringList.Count; } }

        public void AddString(string s)
        {
			lock(_stringList)
            {
                _stringList.Add(s);
            }
        }

        public override string ToString()
        {
            string result = string.Empty;

            lock (_stringList)
            {
                foreach (string s in _stringList)
                    result += s + ",";
            }

            return result.Substring(0, result.Length > 1 ? result.Length - 1 : result.Length);
        }
    }
}
