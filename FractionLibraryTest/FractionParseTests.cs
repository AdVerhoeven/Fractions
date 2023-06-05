using FluentAssertions;
using FractionLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FractionLibraryTest;

[TestClass]
public class FractionParseTests
{
    [TestMethod]
    public void Fraction_Parse_WithValidString_ReturnsFraction()
    {
        //Arrange
        var toParse = "1/2";
        var expected = new Fraction(1, 2);

        //Act
        var actual = Fraction.Parse(toParse);

        //Assert
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Fraction_Parse_WithValidParenthesisString_ReturnsFraction()
    {
        //Arrange
        var toParse = "(1/2)";
        var expected = new Fraction(1, 2);

        //Act
        var actual = Fraction.Parse(toParse);

        //Assert
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Fraction_TryParse_WithInvalidString_ReturnsFalse()
    {
        //Arrange
        var toParse = "0.2";

        //Act
        var actual = Fraction.TryParse(toParse, out Fraction fraction);

        //Assert
        Assert.IsFalse(actual);
        fraction.Should().Be(Fraction.Identity);
    }

    [TestMethod]
    public void Fraction_TryParse_WithValidString_ReturnsTrue()
    {
        //Arrange
        var toParse = "4/2";
        var expectedFraction = new Fraction(4, 2);

        //Act
        var actual = Fraction.TryParse(toParse, out Fraction actualFraction);

        //Assert
        Assert.IsTrue(actual);
        Assert.AreEqual(expectedFraction, actualFraction);
    }
}
