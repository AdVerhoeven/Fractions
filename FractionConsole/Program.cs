using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FractionLibrary;
using System.Numerics;

namespace FractionConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //var rootThree = FractionMath.Sqrt(3);
            //Console.WriteLine($"double.Parse({rootThree.ApproximateAsString()}) = {rootThree.Approximate()}");
            //Console.WriteLine($"double.Parse({(rootThree * rootThree).ApproximateAsString()}) = {(rootThree * rootThree).Approximate()}");
            //Console.WriteLine($"{rootThree.ToString()} == { rootThree.ToString("b")}");

            //Euler71();
            //Euler73();
            //Euler64();
            Euler72();

            Console.WriteLine("Press <enter> to continue...");
            Console.ReadLine();
        }

        private static void Euler72()
        {
            BigInteger count = 0;
            BigInteger impropers = 0;

            Parallel.For(2, 1000001, i =>
             {
                 if((i & 1) == 1)
                 {
                     for (int j = 1; j < i; j++)
                     {
                         var f = new Fraction(j, i);
                         if (!f.IsReduced)
                         {
                             impropers++;
                         }
                         else
                         {
                             count++;
                         }
                     }
                 }
                 else
                 {
                     for (int j = 1; j < i; j+=2)
                     {
                         var f = new Fraction(j, i);
                         if (!f.IsReduced)
                         {
                             impropers++;
                         }
                         else
                         {
                             count++;
                         }
                     }
                 }
                 if (impropers > count)
                 {
                     throw new Exception("fuck me");
                 }
             });
            Console.WriteLine(count);
        }

        private static void Euler64()
        {
            int count = 0;
            for (int i = 2; i < 10000; i++)
            {
                var continuedFrac = FractionMath.SqrtAsContinuedFraction(i);
                if (continuedFrac.Item2.Count % 2 == 1)
                    count++;
            }
            Console.WriteLine(count);
        }

        private static void Euler73()
        {
            var bottomLimit = new Fraction(1, 3);
            var topLimit = new Fraction(1, 2);
            //var fractions = new List<Fraction>();
            var count = 0;

            for (int i = 1; i <= 12000; i++)
            {
                for (int j = i / 3; j < i / 2 + 1; j++)
                {
                    var f = new Fraction(j, i);
                    if (f.IsReduced && f > bottomLimit && f < topLimit)
                    {
                        //fractions.Add(f);
                        count++;
                    }
                }
            }
            Console.WriteLine(count);
        }

        private static void Euler71()
        {
            var limit = new Fraction(3, 7);
            Fraction ans = new Fraction(2, 5);
            // for i <= 1.000.000 e.g. 1000000 order al proper fractions (fractions smaller than 1)
            for (int i = 500000; i <= 1000000; i++)
            {

                for (int j = i / 7 * 3; j < i / 7 * 3 + 7; j++)
                {
                    var f = new Fraction(j, i);
                    if (f > ans && f.IsReduced && f < limit)
                    {
                        ans = f;
                    }
                }
            }
            Console.WriteLine(ans);
        }
    }
}
