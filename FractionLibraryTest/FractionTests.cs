using FractionLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace FractionLibraryTest;

[TestClass]
public class FractionTests
{
    /// <summary>
    /// Tests if the basic rules of addition.
    /// a + b = b + a
    /// a + b + c = c + (b + a) = c + (a + b)
    /// </summary>
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
        Assert.AreEqual(frac1 + frac2 + frac3, frac3 + expected);
    }

    /// <summary>
    /// Test subtraction.
    /// a - b = c
    /// c + b = a
    /// </summary>
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
        Assert.AreEqual(frac1, (frac1 - frac2) + frac2);
    }

    /// <summary>
    /// Test multiplication rules
    /// </summary>
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

    /// <summary>
    /// Test basic division rules
    /// </summary>
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

    /// <summary>
    /// Test the Pow() method.
    /// </summary>
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
    }

    /// <summary>
    /// Test the Sqrt() method. A very basic check wether the square root of 4 is 2.
    /// </summary>
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

    /// <summary>
    /// Test if we can reverse the square of a fraction by taking the root
    /// </summary>
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

    /// <summary>
    /// By definition, sqrt(1/x) = 1/sqrt(x) = sqrt(x)/x
    /// However, a prime number has no rational root.
    /// </summary>
    /// <param name="n"></param>
    [TestMethod]
    [DynamicData(nameof(GetPrimes), DynamicDataSourceType.Method)]
    public void FracSqrtTest(int n)
    {
        //Arrange
        var expected = FractionMath.Sqrt(new Fraction(1, n));

        //Act
        var actual = new Fraction(1, FractionMath.Sqrt(n));
        var actually = new Fraction(FractionMath.Sqrt(n), n);

        //Assert
        Assert.AreEqual((double)expected, (double)actual);
        Assert.AreEqual((double)expected, (double)actually);
    }

    /// <summary>
    /// Generate primes to test the precision of the double cast and square root operation.
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Test wether the inverse operator works as intended.
    /// (x/y) = ~(y/x)
    /// </summary>
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
    public void EpsilonPITest()
    {
        Assert.AreEqual(Math.PI, (double)FractionMath.PI, double.Epsilon);
    }
    [TestMethod]
    public void EpsilonETest()
    {
        Assert.AreEqual(Math.E, (double)FractionMath.E, double.Epsilon);
    }
    [TestMethod]
    public void EpsilonGoldenRatioTest()
    {
        double goldenRatio = (1 + Math.Sqrt(5)) / 2;
        Assert.AreEqual(goldenRatio, (double)FractionMath.GoldenRatio, double.Epsilon);
    }

    /// <summary>
    /// test wether the inverse results the same as when one were to use a negative power.
    /// x to the power -1 = 1/x
    /// </summary>
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

    /// <summary>
    /// Test wether multiplying or dividing with the Identity changes the fraction (should not happen).
    /// </summary>
    [TestMethod]
    public void IdentityTest()
    {
        //Arrange
        var expected = new Fraction(1, 11);

        //Act
        var actualMult = expected * Fraction.Identity;
        var actualDiv = expected / Fraction.Identity;

        //Assert
        Assert.AreEqual(expected, actualMult);
        Assert.AreEqual(expected, actualDiv);
    }

    /// <summary>
    /// Test implicit casting.
    /// </summary>
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

    /// <summary>
    /// 
    /// </summary>
    [TestMethod]
    public void ContinuedFractionTest()
    {
        //Arrange
        var sequence = new List<BigInteger> { 0, 1, 2, 3 };
        var contFrac = new ValueTuple<BigInteger, List<BigInteger>, bool>(0, sequence, false);

        //Act
        var continuedFraction = new Fraction(contFrac, 10);

        //Assert
        // This test is mostly to test for divide by zero exceptions that could be thrown.
        Assert.IsNotNull(continuedFraction);
    }

    /// <summary>
    /// Test wether 0 remains 0 when taking its root. No exceptions should be thrown.
    /// </summary>
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

    /// <summary>
    /// Test if convergent series converge as intended.
    /// </summary>
    [TestMethod]
    public void SqrtOfTwoTest()
    {
        //Arrange
        //The fifth convergent of the continued fraction of the square root of 2
        var expected = new Fraction(99, 70);

        //Act
        var actual = FractionMath.Sqrt(2, 5);

        //Assert
        Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// Test wether the power of the square root of a fraction returns said fraction.
    /// This only works if both the numerator and the denominator have a rational root.
    /// </summary>
    [TestMethod]
    [DynamicData(nameof(Squares), DynamicDataSourceType.Method)]
    public void PowerOfSqrtTest(int x, int y)
    {
        //Arrange
        var expected = new Fraction(x, y);

        //Act
        var actual = new Fraction(x, y);
        actual = actual.Sqrt().Pow(2);

        //Assert
        Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// Generate a few squares.
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<object[]> Squares()
    {
        for (int i = 1; i < 20; i++)
        {
            for (int j = 1; j < 20; j++)
            {
                yield return new object[] { i * i, j * j };
            }
        }
    }
}
