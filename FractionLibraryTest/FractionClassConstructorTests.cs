using FractionLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using System.Numerics;

namespace FractionLibraryTest;

[TestClass]
public class FractionClassConstructorTests
{
    [TestMethod]
    public void NegativeDenominatorShouldBePositiveAfterConstructorTest()
    {
        var fraction = new Fraction(1, -5);

        fraction.Numerator.Sign.Should().BeNegative();
        fraction.Denominator.Sign.Should().BePositive();
    }

    [TestMethod]
    public void WhenInversingAFraction_SignOnDenominator_ShouldBePositive()
    {
        var fraction = new Fraction(1, -5);
        var otherFraction = new Fraction(1, 3);

        var result = fraction * otherFraction;

        result.Denominator.Sign.Should().BePositive();
        result.Numerator.Sign.Should().BeNegative();
        result.IsReduced.Should().BeTrue();
        result.IsProper.Should().BeTrue();
    }

    [TestMethod]
    public void NegativeNumeratorAndDenominatorShouldBePositiveAfterConstructorTest()
    {
        var fraction = new Fraction(-3, -9);

        fraction.Denominator.Sign.Should().BePositive();
        fraction.Numerator.Sign.Should().BePositive();
    }

    [TestMethod]
    public void FractionWithFractionAndIntegerTest()
    {
        var expected = new Fraction(1, 3);
        var actual = new Fraction(expected, 1);

        Assert.AreEqual(actual, expected);
    }

    [TestMethod]
    public void FractionWithIntegerAndFractionTest()
    {
        var expected = new Fraction(1, 5);
        var actual = new Fraction(1, expected);

        Assert.AreNotEqual(expected, actual);
        Assert.AreEqual(expected, ~actual);
    }

    [TestMethod]
    public void FractionFromContinuedFractionTest()
    {
        var denominatorSequence = new List<BigInteger>() { 1, 2 };
        (BigInteger, List<BigInteger>, bool) continuedFraction = new(1, denominatorSequence, true);

        var expected = FractionMath.Sqrt(3);
        var actual = new Fraction(continuedFraction, 30);

        Assert.AreEqual(expected, actual);
    }
}
