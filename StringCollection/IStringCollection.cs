using System;
using System.Collections.Generic;
using System.Text;

namespace StringCollection
{
    public interface IStringCollection
    {
        int NumberOfWords { get; }
        void AddString(string s);
        string ToString();
    }
}
