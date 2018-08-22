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
            TryLock(Thread.CurrentThread.ManagedThreadId, () =>
            {
                Thread.MemoryBarrier();
                _stringList.Add(s);
                Thread.MemoryBarrier();
            });
        }

        public override string ToString()
        {
            string result = string.Empty;

            TryLock(Thread.CurrentThread.ManagedThreadId, () =>
            {
                Thread.MemoryBarrier();
                foreach (string s in _stringList)
                    result += s + ",";
                Thread.MemoryBarrier();
            });

            return result.Substring(0, result.Length > 1 ? result.Length - 1 : result.Length);
        }

        private void TryLock(int lockingThreadId, Action actionToPerform)
        {
            int originalLockState = NotLocked;

            try
            {
                Interlocked.Exchange(ref originalLockState, _isLocked);

                SpinWait.SpinUntil(() =>
                {
                    originalLockState = Interlocked.CompareExchange(ref _isLocked, lockingThreadId, NotLocked);

                    return originalLockState != _isLocked;
                });

                actionToPerform();
            }
            finally
            {
                Interlocked.Exchange(ref originalLockState, _isLocked);

                SpinWait.SpinUntil(() =>
                {
                    originalLockState = Interlocked.CompareExchange(ref _isLocked, NotLocked, lockingThreadId);

                    return originalLockState != _isLocked;
                });

            }
        }
    }
}
