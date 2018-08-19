using System;
using System.Collections.Generic;
using System.Text;

namespace CoinJar
{
    public interface ICoinJar
    {
        void AddCoin(ICoin coin);
        decimal TotalAmount { get; }
        void Reset();
    }
}
