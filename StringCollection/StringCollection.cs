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
    public class StringCollection : IStringCollection
    {
        private int _isLocked = 0;

        private const int Locked = 1;
        private const int NotLocked = 0;

        private List<string> _stringList = new List<string>();

        public StringCollection()
        {
        }

        public int NumberOfWords { get { return _stringList.Count; } }

        public void AddString(string s)
        {
            try
            {
                TryLock(NotLocked, Locked);

                _stringList.Add(s);
            }
            finally
            {
                TryLock(Locked, NotLocked);
            }
        }

        public override string ToString()
        {
            string result = string.Empty;

            try
            {
                TryLock(NotLocked, Locked);

                foreach (string s in _stringList)
                    result += s + ",";
            }
            finally
            {
                TryLock(Locked, NotLocked);
            }

            return result.Substring(0, result.Length > 1 ? result.Length - 1 : result.Length);
        }

        private void TryLock(int currentLockState, int desiredLockState)
        {
            int originalLockState = Interlocked.CompareExchange(ref _isLocked, desiredLockState, currentLockState);

            // State has not changed
            while (originalLockState == _isLocked)
            {
                originalLockState = Interlocked.CompareExchange(ref _isLocked, desiredLockState, currentLockState);
            }
        }
    }
}
