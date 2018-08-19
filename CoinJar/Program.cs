using System;
using System.Linq;
using System.Collections.Generic;

namespace CoinJar
{
    /// <summary>
    /// References:
    ///     https://en.wikipedia.org/wiki/Coins_of_the_United_States_dollar
    /// </summary>
    class Program
    {
        private static Dictionary<string, Coin> AvailableCoins = new Dictionary<string, Coin>
        {
            { "1c", new Coin("1c", "1 US cent", 0.01M, 0.75, 0.06102362) },
            { "5c", new Coin("5c", "5 US cents", 0.05M, 0.835, 0.07677165) },
            { "10c", new Coin("10c", "10 US cents", 0.1M, 0.705, 0.05314961) },
            { "25c", new Coin("25c", "25 US cents", 0.25M, 0.955, 0.06889764) },
            { "50c", new Coin("50c", "50 US cents", 0.50M, 1.205, 0.08464567) },
            { "1d", new Coin("1d", "1 US Doller", 1.0M, 1.043, 0.0787402) }
        };

        static void Main(string[] args)
        {
            CoinJar coinJar = new CoinJar();

            if (args.Length == 0)
            {
                Console.WriteLine(GenerateHelpText());
                return;
            }

            foreach (string arg in args)
            {
                string coinName = string.Empty;
                int numCoins = 0;

                string[] values = arg.ToLower().Split('x');

                if (values.Length != 2)
                {
                    Console.WriteLine($"Number of values specified for {arg} is incorrect");
                    return;
                }

                coinName = values[0];

                if (!int.TryParse(values[1], out numCoins))
                {
                    Console.WriteLine($"The value specified for the number of coins ({values[1]}) is invalid.");
                    return;
                }

                if (AvailableCoins.Keys.Contains(coinName))
                {
                    try
                    {
                        for (int i = 1; i <= numCoins; i++)
                            coinJar.AddCoin(AvailableCoins[coinName]);
                    }
                    catch (Exception coinAdditionException)
                    {
                        Console.WriteLine($"Error: {coinAdditionException.Message}");
                        break;
                    }
                }
                else
                {
                    Console.WriteLine($"Could not find coin: {coinName}, please check the coin parameters");
                    return;
                }
            }

            Console.WriteLine("Current Coin Jar Contents:\r\n");
            Console.WriteLine($" Total Amount: {coinJar.TotalAmount}");
            Console.WriteLine($" Volume: {coinJar.Volume:F3} fluid ounces");

            Console.ReadLine();

            Console.WriteLine("Resetting Coin Jar ...\r\n");
            coinJar.Reset();
            Console.WriteLine($" Total Amount: {coinJar.TotalAmount}");
            Console.WriteLine($" Volume: {coinJar.Volume:F3} fluid ounces");
        }

        static string GenerateHelpText()
        {
            string coinDescriptions =
                "You have not specified any arguments.\r\n"+
                "Please specify the value and number of coins in the following format:\r\n" +
                "  [coin name]x[number of coins], e.g. coinjar 1cx1 25cx10\r\n\r\n" +
                "Coin values available are:\r\n";

            foreach (var item in AvailableCoins)
                coinDescriptions += $"{item.Value.Name}: {item.Value.Description}\r\n";

            return coinDescriptions;
        }
    }
}
