using System;
using System.Collections.Generic;
using System.Text;

namespace CoinJar
{
    public interface ICoin
    {
        decimal Amount { get; }
        decimal Volume { get; }
    }
}
