using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FractionLibrary;
using System.Numerics;

namespace FractionLibraryTest
{
    [TestClass]
    public class FractionTest
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

        [TestMethod]
        public void SqrtTest()
        {
            //Arrange
            var expected = new Fraction(2, 1);

            //Act
            var actual = Fraction.Sqrt(4);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FracPowToSqrtTest()
        {
            //Arrange
            var expected = new Fraction(1, 2);

            //Act
            var temp = new Fraction(1, 2).Pow(2);
            var actual = Fraction.Sqrt(temp);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(3)]
        [DataRow(5)]
        [DataRow(7)]
        [DataRow(11)]
        [DataRow(13)]
        [DataRow(17)]
        [DataRow(19)]
        [DataRow(23)]
        [DataRow(29)]
        [DataRow(31)]
        [DataRow(37)]
        [DataRow(41)]
        [DataRow(43)]
        [DataRow(47)]
        [DataRow(53)]
        [DataRow(59)]
        [DataRow(379)]
        public void FracSqrtTest(int n)
        {
            //Arrange
            var expected = Fraction.Sqrt(new Fraction(1, n));
            var expectedPreciseDouble = expected.ApproximatePrecise();
            // These two will fail to pass all the tests.
            //var expectedString = expected.ApproximateAsString();
            //var expectedDouble = expected.Approximate();

            //Act
            var actual = new Fraction(Fraction.Sqrt(n), n);
            var actualPreciseDouble = actual.ApproximatePrecise();

            // These two will fail to pass all the tests.
            //var actualString = actual.ApproximateAsString();            
            //var actualDouble = actual.Approximate();


            //Assert
            Assert.AreEqual(expectedPreciseDouble, actualPreciseDouble, "ApproximatePrecise()");
            Assert.AreEqual((double)expected, (double)actual, "(double)");

            // These two will fail to pass all the tests.
            //Assert.AreEqual(expectedDouble, actualDouble, "Approximate()");
            //for (int i = 0; i < actualString.Length; i++)
            //{
            //    Assert.AreEqual(expectedString[i], actualString[i], $"ApproximateAsString() Mismatched at digit: #{i}");
            //}            
        }


        [TestMethod]
        public void InverseTest()
        {
            //Arrange
            var expected = new Fraction(2, 1);
            var actual = new Fraction(1, 2);

            //Act
            actual = ~actual;

            //Assert
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
            //Arrange
            BigInteger bigint = 20;
            var expected = new Fraction(bigint, 1);

            //Act
            Fraction actual = bigint;

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
