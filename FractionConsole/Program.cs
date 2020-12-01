using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FractionLibrary;
using System.Numerics;
using System.Diagnostics;

namespace FractionConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("A quick start to show how precise this class can be. We calculate the root of 3 and cast at different moments");
            Console.WriteLine();
            var rootThree = FractionMath.Sqrt(3);
            Console.WriteLine($"String approximation up to 18 digits: {rootThree.ApproximateAsString()}" +
                $" \nThe double from the same fraction: {(double)rootThree}");
            Console.WriteLine($"The square of sqrt(3) as string: {(rootThree * rootThree).ApproximateAsString()}) = {(double)(rootThree * rootThree)} as double");
            Console.WriteLine($"The actual fraction: {rootThree}");
            Console.WriteLine($"{rootThree.ToString("b")}");
            Console.WriteLine();
            Console.WriteLine("The next few samples of code have execution times between 0 seconds up till several minutes.");
            Euler71();
            Euler73();
            ExecuteTask("Project Euler 64, Odd period square roots.", "less than 4 seconds", Euler64());
            Euler72();

            Console.WriteLine("Press <enter> to continue...");
            Console.ReadLine();
        }
        private static bool ExecuteTask(string projectName, string expectedTime, Task t)
        {
            Console.WriteLine($"\n{projectName}\n" +
                $"The code that you are about to execute might take a long time to complete.\n" +
                $"The expected runtime is: {expectedTime}\n" +
                $"Do you wish to execute this code: Y/N");

            char key = 'a';

            while (key != 'y' && key != 'Y')
            {
                key = Console.ReadKey(true).KeyChar;
                switch (key)
                {
                    case 'y':
                    case 'Y':
                        Console.WriteLine($"Executing: {projectName}");
                        break;
                    case 'n':
                    case 'N':
                        Console.WriteLine($"Skipped Execution of: {projectName}");
                        return true;
                    default:
                        break;
                }
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            t.Start();
            t.Wait();
            stopwatch.Stop();
            Console.WriteLine($"Time elapsed: {stopwatch.ElapsedMilliseconds} ms");
            return t.IsCompleted;
        }

        private static bool Execute(string projectName, string expectedTime)
        {
            Console.WriteLine($"\nThe code that you are about to execute might take a long time to complete.\n" +
                $"The expected runtime is: {expectedTime}\n" +
                $"Do you wish to execute this code: {projectName} Y/N");
            while (true)
            {
                var key = Console.ReadKey(true);
                switch (key.KeyChar)
                {
                    case 'y':
                    case 'Y':
                        Console.WriteLine($"Executing: {projectName}");
                        return true;
                    case 'n':
                    case 'N':
                        Console.WriteLine($"Skipped Execution of: {projectName}");
                        return false;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Project Euler 72, Counting Fractions
        /// </summary>
        private static void Euler72()
        {
            if (!Execute("Euler72", "Several minutes and maybe even hours... very inefficient code for the problem")) return;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            BigInteger count = 0;
            BigInteger impropers = 0;

            Parallel.For(2, 1000001, i =>
             {
                 if ((i & 1) == 1)
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
                     for (int j = 1; j < i; j += 2)
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
                     throw new Exception("Something went terribly wrong, we cannot have more improper fractions than fractions because it is a subset.");
                 }
             });
            stopwatch.Stop();
            Console.WriteLine($"The number of proper fractions is: {count}");
            Console.WriteLine($"Time elapsed: {stopwatch.ElapsedMilliseconds} ms");
            Console.WriteLine();
        }


        /// <summary>
        /// Project euler 64, Odd period square roots
        /// </summary>
        private static Task Euler64()
        {
            return new Task(() =>
            {
                int count = 0;
                for (int i = 2; i < 10000; i++)
                {
                    var continuedFrac = FractionMath.SqrtAsContinuedFraction(i);
                    if (continuedFrac.Item2.Count % 2 == 1)
                        count++;
                }
                Console.WriteLine($"The number of odd period square roots is {count}");
            });
        }

        /// <summary>
        /// Project euler 73, Counting fractions in a range
        /// </summary>
        private static void Euler73()
        {
            if (!Execute("Euler73", "Minutes")) return;
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

        /// <summary>
        /// Project euler 71, Ordered fractions.
        /// </summary>
        private static void Euler71()
        {
            if (!Execute("Euler71", "a few seconds")) return;
            var limit = new Fraction(3, 7);
            Fraction ans = new Fraction(2, 5);
            // for i <= 1.000.000 e.g. 1000000 order al proper fractions (fractions smaller than 1)
            for (int i = 500_000; i <= 1_000_000; i++)
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
