using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace StringCollection
{
    public class StringCollection : IStringCollection
    {
        private int _isWriteLocked = 0;
        private int _isReadLocked = 0;

        private const int Locked = 1;
        private const int NotLocked = 0;

        private List<string> _stringList = new List<string>();

        private bool _threadSafe = false;

        public StringCollection(bool threadSafe = true)
        {
            _threadSafe = threadSafe;
        }

        public int NumberOfWords { get { return _stringList.Count; } }

        public void AddString(string s)
        {
            try
            {
                TryLock(ref _isWriteLocked, NotLocked, Locked);

                _stringList.Add(s);
            }
            finally
            {
                TryLock(ref _isWriteLocked, Locked, NotLocked);
            }
        }

        public override string ToString()
        {
            string result = string.Empty;

            try
            {
                TryLock(ref _isReadLocked, NotLocked, Locked);

                _stringList.ForEach((s) => result += s + ",");
            }
            finally
            {
                TryLock(ref _isReadLocked, Locked, NotLocked);
            }

            return result.Substring(0, result.Length > 1 ? result.Length - 1 : result.Length);
        }

        private void TryLock(ref int isLockedValue, int currentLockState, int desiredLockState)
        {
            bool desiredLockStateAchieved = false;

            if (_threadSafe)
            {
                do
                {
                    desiredLockStateAchieved = Interlocked.CompareExchange(ref isLockedValue, desiredLockState, currentLockState) == desiredLockState;
                }
                while (desiredLockStateAchieved);
            }
        }
    }
}
