using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// TODO:
/// 1. implement threadsafe reads and writes
/// </summary>
namespace StringCollection
{
    /// <summary>
    /// The code executes asyn and non async so that we can compare the execution
    /// time and the results.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            bool threadSafe = true;

            if (args.Length > 0)
            {
                threadSafe = !(args[0] == "0");
            }

            string sentence =
                "The mass and composition of the cent changed to the current copper plated zinc core in 1982. Both types were minted in 1982 with no distinguishing mark. " +
                "Cents minted in 1943 were struck on planchets punched from zinc coated steel which left the resulting edges uncoated. This caused many of these coins to " +
                "rust. These 'steel pennies' are not likely to be found in circulation today, as they were later intentionally removed from circulation for recycling the metal. " +
                "However, cents minted from 1944 to 1946 were made from a special salvaged WWII brass composition to replace the steel cents, but still save material for the war " +
                "effort, and are more common in circulation than their 1943 counterparts. The wheat cent was mainstream and common during its time. Some dates are rare, but many " +
                "can still be found in circulation.This is partially due to the fact that unlike the formerly silver denominations(dollar, half, quarter, dime), the composition of " +
                "the pre - 1982 cent, nearly pure copper, is not so much more valuable over face value for it to be hoarded to the extreme extent of the silver denominations. " +
                "Nickels produced from mid-1942 through 1945 were manufactured from 56 % copper, 35 % silver and 9 % manganese.This allowed the saved nickel metal to be shifted " +
                "to industrial production of military supplies during World War II.Few of these are still found in circulation. Prior to 1965 and passage of the Coinage Act of " +
                "1965 the composition of the dime, quarter, half - dollar and dollar coins was 90 % silver and 10 % copper.The half - dollar continued to be minted in a 40 % silver " +
                "- clad composition between 1965 and 1970.Dimes and quarters from before 1965 and half - dollars from before 1971 are generally not in circulation due to being removed " +
                "for their silver content. In 1975 and 1976 bicentennial coinage was minted.Regardless of date of coining, each coin bears the dual date '1776-1976'. " +
                "The Quarter - Dollar, Half - Dollar and Dollar coins were issued in the copper 91.67 % nickel 8.33 % composition for general circulation and the Government " +
                "issued 6 - coin Proof Set.A special 3 - coin set of 40 % silver coins were also issued by the U.S.Mint in both Uncirculated and Proof. Use of the half - dollar " +
                "is not as widespread as that of other coins in general circulation; most Americans use dollar coins, quarters, dimes, nickels and cents only, as these are the " +
                "only coins most often found in general circulation. When found, many 50¢ coins are quickly hoarded, spent, or brought to banks. The Presidential Dollar series " +
                "features portraits of all deceased U.S.Presidents with four coin designs issued each year in the order of the president's inauguration date. These coins began " +
                "circulating on February 15, 2007. Starting 2012, these coins have been minted only for collectible sets because of a large stockpile. The Susan B.Anthony dollar " +
                "coin was minted from 1979–1981 and 1999.The 1999 minting was in response to Treasury supplies of the dollar becoming depleted and the inability to accelerate the " +
                "minting of the Sacagawea dollars by a year. 1981 Anthony dollars can sometimes be found in circulation from proof sets that were broken open, but these dollars "  +
                "were not minted with the intent that they circulate.";

            string[] words = sentence.Split(' ');
            CancellationToken cancellationToken = new CancellationToken();

            StringCollection stringsAsync = new StringCollection(threadSafe);
            Task readAsynStringsTask = new Task(() =>
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine($"Async Execution ({DateTime.Now})");
                    Console.WriteLine($"  Words to Add: {words.Length}");
                    Console.WriteLine($"  Words Added: {stringsAsync.NumberOfWords}");
                    Console.WriteLine($"  Words: {stringsAsync.ToString()}");
                    Thread.Sleep(3000);
                }
            });
            readAsynStringsTask.Start();

            DateTime startTimeAsync = DateTime.Now;
            foreach (var word in words)
            {
                Task.Run(() => stringsAsync.AddString(word));
            };
            DateTime endTimeAsync = DateTime.Now;

            readAsynStringsTask
                .ContinueWith(result =>
                {
                    Console.WriteLine($"Asyn Word Statistics ({DateTime.Now}):");
                    Console.WriteLine($"  Execution Time: { (endTimeAsync - startTimeAsync).TotalMilliseconds }");
                    Console.ReadLine();
                });

            readAsynStringsTask.Wait();
        }
    }
}
