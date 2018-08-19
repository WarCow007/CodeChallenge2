using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace StringCollection
{
    public class StringCollection : IStringCollection
    {
        ReaderWriterLockSlim stringLock = new ReaderWriterLockSlim();

        private List<string> _stringList = new List<string>();

        private bool _threadSafe = false;

        public StringCollection(bool threadSafe = true)
        {
            _threadSafe = threadSafe;
        }

        public int NumberOfWords { get { return _stringList.Count; } }

        public void AddString(string s)
        {
            if (_threadSafe)
                stringLock.EnterWriteLock();

            try
            {
                _stringList.Add(s);
            }
            finally
            {
                if (_threadSafe)
                    stringLock.ExitWriteLock();
            }
        }

        public override string ToString()
        {
            string result = string.Empty;

            if (_threadSafe)
                stringLock.EnterReadLock();

            try
            {
                _stringList.ForEach((s) => result += s + ",");
            }
            finally
            {
                if (_threadSafe)
                    stringLock.ExitReadLock();
            }

            return result.Substring(0, result.Length - 1);
        }
    }
}
