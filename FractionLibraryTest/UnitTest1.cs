using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FractionLibrary;
using System.Numerics;

namespace FractionLibraryTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void MultiplicationTest()
        {
            //Arrange
            var expected = new Fraction(1, 6);
            var x = new Fraction(1, 2);
            var y = new Fraction(1, 3);

            //Act
            var actual = x * y;

            //Assert
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(x * y, y * x);
        }

        [TestMethod]
        public void DivisionTest()
        {
            //Arrange
            var expected = new Fraction(1, 2);
            var one = 1;
            var two = new Fraction(2, 1);

            //Act
            var actual = one / two;

            //Assert            
            Assert.AreEqual(expected, actual);
            //HACK AreEqual does not work on reference types.
            //Assert.IsTrue(expected.Equals(actual));
        }

        [TestMethod]
        public void PowerTest()
        {
            //Arrange
            var fraction = new Fraction(1, 5);
            // expected = fraction^5
            var expected = fraction *
                fraction * fraction *
                fraction * fraction;

            //Act
            var actual = fraction.Pow(5);

            //Assert
            Assert.AreEqual(expected, actual);
            Assert.AreNotSame(actual, fraction);
            Assert.AreNotEqual(actual, fraction);
        }


        //TODO: fix and test Sqrt() method
        //[TestMethod]
        //public void SqrtTest()
        //{
        //    //Arrange
        //    var expected = new Fraction(1, 2);
        //    var toRoot = new Fraction(1, 4);

        //    //Act
        //    var actual = toRoot.Sqrt(10).Simplify();

        //    //Arrange
        //    Assert.AreEqual(expected, actual);
        //}

        [TestMethod]
        public void InverseTest()
        {
            //Arrange
            var expected = new Fraction(2, 1);
            var actual = new Fraction(1, 2);

            //Act
            actual = ~actual;

            //Arrange
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IdentityTest()
        {
            //Arrange
            var expected = Fraction.Identity;

            //Act
            var actualMult = Fraction.Identity * Fraction.Identity;
            var actualDiv = Fraction.Identity / Fraction.Identity;
            var actualPow = Fraction.Identity.Pow(10);

            //Assert
            Assert.AreEqual(expected, actualMult);
            Assert.AreEqual(expected, actualDiv);
            Assert.AreEqual(expected, actualPow);
        }

        [TestMethod]
        public void ImplicitBigIntegerTest()
        {
            BigInteger bigint = 20;
            var expected = new Fraction(bigint, 1);

            Fraction actual = bigint;

            Assert.AreEqual(expected, actual);
        }
    }
}
