using FluentAssertions;
using FractionLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FractionLibraryTest;

[TestClass]
public class FractionFormattingTests
{
    [TestMethod]
    public void Fraction_Interpolation_Test()
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
}
