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
            var rootThree = FractionMath.Sqrt(3);
            Console.WriteLine($"double.Parse({rootThree.ApproximateAsString()}) = {rootThree.Approximate()}");
            Console.WriteLine($"double.Parse({(rootThree * rootThree).ApproximateAsString()}) = {(rootThree * rootThree).Approximate()}");
            Console.WriteLine($"{rootThree.ToString()} == { rootThree.ToString("b")}");

            Console.ReadKey();
        }
    }
}
