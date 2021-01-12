using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FractionLibrary;

namespace FractionLibraryTest
{
    class FractionClassConstructorTest
    {
        public void NonBigIntTest()
        {
            //Arrange
            var expected = new Fraction(5, 1);
            //Act
            Fraction actual = 5;
            var flong = new Fraction(5L, 2);
            //Assert
        }
    }
}
