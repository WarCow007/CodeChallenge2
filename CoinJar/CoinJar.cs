using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CoinJar
{
    public class CoinJar : ICoinJar
    {
        private List<ICoin> _coinsInJar = new List<ICoin>();

        private const decimal MaxCoinVolumeInFluidOunces = 42.0M;

        public decimal TotalAmount
        {
            get { return _coinsInJar.Sum(c => c.Amount); }
        }

        public decimal Volume { get { return _coinsInJar.Sum(c => c.Volume); } }

        public void AddCoin(ICoin coin)
        {
            if (Volume + coin.Volume <= MaxCoinVolumeInFluidOunces)
                _coinsInJar.Add(coin);
            else
                throw new ArgumentException($"Coin {coin.Amount} could not be added because volume {MaxCoinVolumeInFluidOunces} exceeded.");
        }

        public void Reset()
        {
            _coinsInJar.Clear();
        }
    }
}
