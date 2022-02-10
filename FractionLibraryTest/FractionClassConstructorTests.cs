using FractionLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FractionLibraryTest;

[TestClass]
public class FractionClassConstructorTests
{
    [TestMethod]
    public void TestNegativeDenominator()
    {
        var expected = new Fraction(-1, 5);

        var actual = new Fraction(1, -5);

        Assert.AreEqual(expected, actual);
    }
}
