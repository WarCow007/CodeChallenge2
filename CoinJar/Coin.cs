using System;
using System.Collections.Generic;
using System.Text;

namespace CoinJar
{
    /// <summary>
    /// The volume of the coin is calculated from its diameter and height. The height
    /// and diameter values are specified in inches.
    /// 
    /// In order to calculate fluid ounces we use a multiplier (0.554)
    /// 
    /// Volume: this value is in fluid ounces
    /// 
    /// References:
    ///     https://sciencing.com/calculate-volume-cylinder-ounces-8481368.html
    /// </summary>
    public class Coin : ICoin
    {
        public string _name = string.Empty;
        public string _description = string.Empty;

        private decimal _amount = 0.0M;
        private decimal _volumeInFluidOunces = 0.0M;
        private const double FluidOunceMultiplier = 0.554D;

        public Coin(string name, string description, decimal amount, double diameterInInches, double heightInInches)
        {
            _name = name;
            _description = description;

            _amount = amount;

            double volumeInCubicInches = (Math.PI * Math.Pow((diameterInInches / 2), 2) * heightInInches);
            _volumeInFluidOunces = (decimal)(volumeInCubicInches * FluidOunceMultiplier);
        }

        public string Name { get { return _name; } }

        public string Description { get { return _description; } }

        public decimal Amount { get { return _amount; } }

        public decimal Volume { get { return _volumeInFluidOunces; } }
    }
}
