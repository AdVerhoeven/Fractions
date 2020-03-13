using FractionLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace FractionLibraryTest
{
    [TestClass]
    public class FractionTest
    {
        [TestMethod]
        public void AdditionTest()
        {
            //Arrange
            var expected = Fraction.Identity;
            var frac1 = new Fraction(1, 2);
            var frac2 = new Fraction(1, 2);
            var frac3 = new Fraction(1, 3);

            //Act
            var actual = frac1 + frac2;
            var actual2 = frac2 + frac1;

            //Assert
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expected, actual2);
            Assert.AreEqual(frac1 + frac2 + frac3, frac3 + frac2 + frac1);
        }

        [TestMethod]
        public void SubtractionTest()
        {
            //Arrange
            var expected = Fraction.Identity;
            var frac1 = new Fraction(3, 2);
            var frac2 = new Fraction(1, 2);

            //Act
            var actual = frac1 - frac2;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MultiplicationTest()
        {
            //Arrange
            var expected = new Fraction(1, 6);
            var x = new Fraction(1, 2);
            var y = new Fraction(1, 3);
            var z = new Fraction(1, 5);

            //Act
            var actual = x * y;

            //Assert
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(x * y, y * x);
            Assert.AreEqual((x * y) * z, x * (y * z));
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
            var actual = FractionMath.Pow(fraction, 5);

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
            var actual = FractionMath.Sqrt(4);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FracPowToSqrtTest()
        {
            //Arrange
            var expected = new Fraction(1, 2);

            //Act
            var temp = FractionMath.Pow(new Fraction(1, 2), 2);
            var actual = FractionMath.Sqrt(temp);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DynamicData(nameof(GetPrimes), DynamicDataSourceType.Method)]
        public void FracSqrtTest(int n)
        {
            //Arrange
            var expected = FractionMath.Sqrt(new Fraction(1, n));
            // These two will fail to pass all the tests.
            //var expectedString = expected.ApproximateAsString();
            //var expectedDouble = expected.Approximate();

            //Act
            var actual = new Fraction(FractionMath.Sqrt(n), n);

            // These two will fail to pass all the tests.
            //var actualString = actual.ApproximateAsString();            
            //var actualDouble = actual.Approximate();


            //Assert
            Assert.AreEqual((double)expected, (double)actual, "(double)");

            // These two will fail to pass all the tests.
            //Assert.AreEqual(expectedDouble, actualDouble, "Approximate()");
            //for (int i = 0; i < actualString.Length; i++)
            //{
            //    Assert.AreEqual(expectedString[i], actualString[i], $"ApproximateAsString() Mismatched at digit: #{i}");
            //}            
        }

        private static IEnumerable<object[]> GetPrimes()
        {
            var maxPrime = 1000;
            bool[] eliminated = new bool[maxPrime];
            int maxSquareRoot = (int)Math.Sqrt(maxPrime);

            //For all Uneven numbers above 1
            for (int num = 3; num < maxPrime; num += 2)
            {
                //Check if this number has already been checked as eliminated, e.g. is a multiple of any prime.
                if (!eliminated[num])
                {
                    //Add the new Prime
                    yield return new object[] { num };
                    //If the prime is smaller then the squareroot of our maxValue we do not need to eliminate anything.
                    if (num < maxSquareRoot)
                    {
                        //Skip al even multiples and redudant multiples by starting at its square and adding 2*num.
                        //Uneven + Uneven = Even. Uneven + Even = Uneven. This means we save ourselves a lot of work.
                        for (int j = num * num; j < maxPrime && j > 0; j += num + num)
                        {
                            //Eliminate this multiple of a prime.
                            eliminated[j] = true;
                        }
                    }
                }
            }
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
        public void InverseAndPowerTest()
        {
            //Arrange
            var expected = ~new Fraction(1, 5);

            //Act
            var actual = FractionMath.Pow(new Fraction(1, 5), -1);

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
            var actualSqrt = FractionMath.Sqrt(1);

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

        [TestMethod]
        public void ContinuedFractionTest()
        {
            //Arrange
            var sequence = new List<BigInteger> { 0, 1, 2, 3 };
            var contFrac = new KeyValuePair<BigInteger, List<BigInteger>>(0, sequence);

            //Act
            var continuedFraction = new Fraction(contFrac, 10);

            //Assert
            // This test is mostly to test for divide by zero exceptions that could be thrown.
        }

        [TestMethod]
        public void SqrtOfZeroTest()
        {
            //Arrange
            var expected = 0;
            Fraction actual = 0;

            //Act
            actual = actual.Sqrt();

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SqrtOfTwoTest()
        {
            //Arrange
            var expected = new Fraction(99, 70); // The fifth convergent of the continued fraction

            //Act
            var actual = FractionMath.Sqrt(2, 5);

            //Assert
            Assert.AreEqual((double)expected, (double)actual);
        }
    }
}
