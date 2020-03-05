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
            var rootThree = Fraction.Sqrt(3);
            Console.WriteLine(rootThree.Approximate());
            Console.WriteLine((rootThree * rootThree).Approximate());


            Console.ReadKey();
        }
    }
}
