using FluentAssertions;
using FractionLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FractionLibraryTest;

[TestClass]
public class FractionFormattingTests
{
    [TestMethod]
    public void Fraction_ToString_WithInterpolation_ShouldBe_SameAs_ToString()
    {
        //Arrange
        var frac = new Fraction(12, 5);
        var expected = "(2 + 2 / 5)";

        //Act
        var tostringUpper = frac.ToString("B");
        var tostringLower = frac.ToString("b");
        var interPolUpper = $"{frac:B}";
        var interPolLower = $"{frac:b}";

        //Assert
        tostringLower.Should().Be(expected);
        tostringUpper.Should().Be(expected);
        interPolLower.Should().Be(expected);
        interPolUpper.Should().Be(expected);
    }

    [TestMethod]
    public void Fraction_ToString_With_NullFormat_ShouldUseDefault()
    {
        // Arrange
        var frac = new Fraction(5, 33);
        // Act
        Func<string> act = () => { return frac.ToString(null); };

        // Assert
        act.Should().NotThrow();
        act.Invoke().Should().Be(frac.ToString());
    }
}
