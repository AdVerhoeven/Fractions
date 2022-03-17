using FractionLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using System.Numerics;

namespace FractionLibraryTest;

[TestClass]
public class FractionClassConstructorTests
{
    [TestMethod]
    public void NegativeDenominator_ShouldBe_Positive_AfterConstructorTest()
    {
        var fraction = new Fraction(1, -5);

        fraction.Numerator.Sign.Should().BeNegative();
    }

    [TestMethod]
    public void NegativeNumerator_And_Denominator_ShouldBe_Positive_AfterConstructorTest()
    {
        var fraction = new Fraction(-3, -9);

        fraction.Denominator.Sign.Should().BePositive();
        fraction.Numerator.Sign.Should().BePositive();
    }

    [TestMethod]
    public void Fraction_With_Fraction_And_Integer_ConstructorTest()
    {
        var expected = new Fraction(1, 3);
        var actual = new Fraction(expected, 1);

        Assert.AreEqual(actual, expected);
    }

    [TestMethod]
    public void Fraction_With_Integer_And_Fraction_ConstructorTest()
    {
        var expected = new Fraction(1, 5);
        var actual = new Fraction(1, expected);

        Assert.AreNotEqual(expected, actual);
        Assert.AreEqual(expected, ~actual);
    }

    [TestMethod]
    public void Fraction_FromContinuedFraction_ConstructorTest()
    {
        var denominatorSequence = new List<BigInteger>() { 1, 2 };
        (BigInteger, List<BigInteger>, bool) continuedFraction = new(1, denominatorSequence, true);

        var expected = FractionMath.Sqrt(3);
        var actual = new Fraction(continuedFraction, 30);

        Assert.AreEqual(expected, actual);
    }
}
