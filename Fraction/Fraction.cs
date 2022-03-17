using System.Globalization;
using System.Numerics;
using System.Text;

namespace FractionLibrary;

/// <summary>
/// A class that provides fractions to calculate with.
/// Allows using the default arithmetic operators on members of this class.
/// The sign is always on the Numerator.
/// Can be compared with integers, doubles and fractions.
/// Can be used within its own constructors for simplified writing of recursive behaviour.
/// </summary>
public struct Fraction : IComparable<Fraction>, IEquatable<Fraction>, IFormattable
{
    #region Conversions
    // Implicit Convserions
    public static implicit operator Fraction(BigInteger b) => new Fraction(b, 1);
    public static implicit operator Fraction(int i) => new Fraction(i, 1);

    // NOTE: We do not really need these implicit conversions since it will not be used most of the time, it could however be added. 
    // BigIntegers have the implicit conversions so the BigInteger conversion should suffice for most situations.
    public static implicit operator Fraction(uint ui) => new Fraction(ui, 1);
    public static implicit operator Fraction(long l) => new Fraction(l, 1);
    public static implicit operator Fraction(ulong ul) => new Fraction(ul, 1);
    public static implicit operator Fraction(short s) => new Fraction(s, 1);
    public static implicit operator Fraction(ushort us) => new Fraction(us, 1);
    public static implicit operator Fraction(sbyte sb) => new Fraction(sb, 1);
    public static implicit operator Fraction(byte b) => new Fraction(b, 1);

    // Explicit conversions
    public static explicit operator BigInteger(Fraction f) => f.Numerator / f.Denominator;
    // a double has a limited precision of 15-17 decimal digits.
    public static explicit operator double(Fraction f) => double.Parse(f.ApproximateAsString());

    public static explicit operator decimal(Fraction f) => decimal.Parse(f.ApproximateAsString(58));

    public static explicit operator int(Fraction f)
    {
        var ans = f.Numerator / f.Denominator;
        return (ans < int.MaxValue) ? (int)ans : throw new OverflowException("This fraction is to big");
    }
    #endregion

    #region Properties
    private BigInteger numerator;
    private BigInteger denominator;
    private static readonly Fraction identity = new(1, 1);
    public static Fraction Identity => identity;
    /// <summary>
    /// The Numerator or top part of the fraction.
    /// </summary>
    public BigInteger Numerator
    {
        set
        {
            numerator = value;
        }
        get
        {
            return numerator;
        }
    }

    /// <summary>
    /// The Denominator or bottom part of the fraction.
    /// </summary>
    public BigInteger Denominator
    {
        set
        {
            if (value > 0)
            {
                denominator = value;
            }
            else if (value < 0)
            {
                denominator = value * -1;
                numerator *= -1;
            }
            else
            {
                throw new DivideByZeroException("The Denominator must be greater then 0");
            }
        }
        get
        {
            return denominator;
        }
    }

    /// <summary>
    /// A proper fraction should have an absolute value of 1 or less.
    /// </summary>
    public bool IsProper => BigInteger.Abs(this.Numerator) < this.Denominator;

    /// <summary>
    /// A reduced fraction is a simplified fraction, e.g. the biggest common factor is 1.
    /// </summary>
    public bool IsReduced => GCD(this.numerator, this.denominator) == 1;
    #endregion

    #region Constructors
    /// <summary>
    /// Default Constructor.
    /// Creates a fraction object. A denominator of 0 will throw a divide by zero exception.
    /// </summary>
    /// <param name="num">Numerator</param>
    /// <param name="den">Denominator</param>
    public Fraction(BigInteger num, BigInteger den)
    {
        if (den == 0)
        {
            throw new DivideByZeroException($"Can't divide by zero. {den} is zero");
        }
        if (den < 0)
        {
            num *= -1;
            den *= -1;
        }

        numerator = num;
        denominator = den;
    }

    /// <summary>
    /// Copy Constructor
    /// </summary>
    /// <param name="frac"></param>
    public Fraction(Fraction frac)
    {
        numerator = frac.Numerator;
        denominator = frac.Denominator;
    }

    /// <summary>
    /// Creates a fraction object. A denominator of 0 will throw a divide by zero exception.
    /// </summary>
    /// <param name="num">Numerator</param>
    /// <param name="den">Denominator</param>
    public Fraction(BigInteger num, Fraction den)
    {
        if (den.Numerator == 0)
        {
            throw new DivideByZeroException($"Can't divide by zero. {den} is zero");
        }
        // X diveded by Y over Z equals X times Z over Y
        this = ~den * num;
    }

    /// <summary>
    /// Creates a fraction object. A denominator of 0 will throw a divide by zero exception.
    /// </summary>
    /// <param name="num">Numerator</param>
    /// <param name="den">Denominator</param>
    public Fraction(Fraction num, BigInteger den)
    {
        if (den == 0)
        {
            throw new DivideByZeroException($"Can't divide by zero. {den} is zero");
        }
        this = num / den;
    }

    /// <summary>
    /// Creates a fraction object. A denominator of 0 will throw a divide by zero exception. 
    /// </summary>
    /// <param name="num">Numerator</param>
    /// <param name="den">Denominator</param>
    public Fraction(Fraction num, Fraction den)
    {
        if (den.Numerator == 0)
        {
            throw new DivideByZeroException($"Can't divide by zero. {den} is zero");
        }
        this = num / den;
    }

    /// <summary>
    /// <para>The fraction obtained after going through <paramref name="steps"/> steps of the <paramref name="continuedFraction"/> denominatorSequence</para>
    /// The sequence can/should not contain any zeroes. 
    /// </summary>
    /// <param name="continuedFraction">The continued fraction object</param>
    /// <param name="steps">The amount of steps to execute the continued fration.</param>
    public Fraction((BigInteger initial, List<BigInteger> denominatorSequence, bool repeats) continuedFraction, int steps = 30)
    {
        var period = continuedFraction.denominatorSequence;
        var periodLength = period.Count;
        if (periodLength == 0)
            steps = 0;

        if (steps == 0)
        {
            this = new Fraction(continuedFraction.initial, 1);
        }
        else
        {
            steps--;
            // An invalid denominator list (e.g. one that contains zero) will throw a divide by zero exception
            this = new Fraction(1, period[steps % periodLength]);
            while (steps > 0)
            {
                //a + 1/r
                steps--;
                this = 1 / (period[steps % periodLength] + this);
                //r = period[steps % periodLength] + new Fraction(1/r); 
            }
            // At this point we've reached the upper/left part of the continued fraction so we need to add the initial value.
            this = (continuedFraction.initial + this);
        }
    }
    #endregion

    #region Methods
    /// <summary>
    /// Simplifies a fraction by dividing the nominator and denominator by their greatest common divisor.
    /// </summary>
    /// <param name="frac">The original <see cref="Fraction"/></param>
    /// <returns>A simplified or reduced <see cref="Fraction"/></returns>
    public Fraction Simplify()
    {
        var gcd = GCD(this.Numerator, this.Denominator);

        var ans = new Fraction(this.Numerator / gcd, this.Denominator / gcd);

        return ans;
    }

    #endregion

    #region Operators

    #region Arithmatic
    public static Fraction operator +(Fraction a, Fraction b)

        => new Fraction(a.numerator * b.denominator + b.numerator * a.denominator, a.denominator * b.denominator);

    public static Fraction operator -(Fraction a, Fraction b)

        => new Fraction(a.Numerator * b.Denominator - a.Denominator * b.Numerator, a.Denominator * b.Denominator);


    public static Fraction operator *(Fraction a, Fraction b)

        => new Fraction(a.Numerator * b.Numerator, a.Denominator * b.Denominator);


    public static Fraction operator /(Fraction a, Fraction b) => a * ~b;
    #endregion

    #region Unary
    /// <summary>
    /// Inverts a fraction.
    /// </summary>
    /// <param name="a"></param>
    /// <returns></returns>
    public static Fraction operator ~(Fraction a) => new Fraction(a.denominator, a.numerator);

    public static Fraction operator -(Fraction a) => new Fraction(a.numerator * -1, a.denominator);

    public static Fraction operator +(Fraction a) => a;
    #endregion

    #region Comparison
    public static bool operator ==(Fraction a, Fraction b) => a.Equals(b);
    public static bool operator !=(Fraction a, Fraction b) => !(a == b);
    public static bool operator >(Fraction a, Fraction b)
    {
        var c = a - b;
        //if a - b results in 0, it is false, otherwise return if a - b is larger than 0.
        return c.Numerator != 0 && c.Numerator > 0;
    }
    public static bool operator <(Fraction a, Fraction b) => b > a;
    public static bool operator >=(Fraction a, Fraction b) => a == b || a > b;
    public static bool operator <=(Fraction a, Fraction b) => b >= a;
    #endregion

    #endregion

    #region Helper Methods
    /// <summary>
    /// Returns the greatest common divisor of two integers.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    private static BigInteger GCD(BigInteger a, BigInteger b)
    {
        if (a == 0)
            return b;
        return GCD(b % a, a);
    }

    #region ToString
    /// <summary>
    /// Returns the fraction as "(a / b)"
    /// </summary>
    /// <returns></returns>
    public override string ToString() => $"({Numerator} / {Denominator})";

    /// <summary>
    /// Returns a fraction as <br/>"(a / b)", format string "G" or "S"<br/> 
    /// or <br/>"(a + b / c)" format string "B"<br/>or<br/>"a" format string "H" 
    /// </summary>
    /// <param name="format">
    /// "B" = full integers separately<br/>
    /// "G" or "S" = default<br/>
    /// "H" = only the integer part
    /// </param>
    /// <returns>The formatted string representation of the fraction</returns>
    /// <remarks>Format strings are case-insensitive.</remarks>
    public string ToString(string format)
    {
        if (string.IsNullOrEmpty(format))
            return ToString();

        switch (format.ToUpperInvariant())
        {
            case "G":
            case "S":
                return this.ToString();
            case "B":
                var sb = new StringBuilder();
                sb.Append('(');
                sb.Append(this.Numerator / this.Denominator);
                sb.Append(" + ");
                sb.Append(this.Numerator % this.Denominator);
                sb.Append(" / ");
                sb.Append(this.Denominator);
                sb.Append(')');
                return sb.ToString();
            case "H":
                return $"{Numerator / Denominator}";
            default:
                throw new FormatException($"The {format} format string is not supported.");
        }
    }

    /// <summary>
    /// Not yet fully implemented.<br/>
    /// <inheritdoc cref="ToString(string)"/>
    /// </summary>
    /// <param name="format"></param>
    /// <param name="formatProvider"></param>
    /// <returns></returns>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        if (formatProvider == null)
        {
            return ToString(format);
        }

        return ToString();
    }

    /// <summary>
    /// Approximates a Fraction to a floating point string.
    /// The brackets encapsulate the repeating sequence within the real/floating point number.
    /// </summary>
    /// <param name="lim">The amount of digits behind the decimal separator to be found.</param>
    /// <returns>A floating point string.</returns>
    public string ApproximateAsString(int lim = 18)
    {
        // 0. lim * x is the minimum amount of characters/digits in our result.
        var sb = new StringBuilder(lim + 2);

        BigInteger numerator = this.Numerator;
        BigInteger denominator = this.Denominator;
        bool zero = false;
        // Keep a list of numerators once we passed the decimal separator to prevent calculating an ever repeating sequence.
        List<BigInteger> numerators = new List<BigInteger>();

        while (numerator != 0)
        {
            // Perform long divison on the fraction to find the right digits.
            if (numerator > denominator && !zero)
            {
                BigInteger quotient = numerator / denominator;
                BigInteger remainder = numerator % denominator;
                // Add the new digit and set the numerator to the remainder to continue with the long division.
                sb.Append(quotient.ToString());
                numerator = remainder;
            }
            // Once we have calculated the whole numbers we get into this part.
            if (zero)
            {
                // Check how many digits behind the decimal separator are still left to calculate.
                if (lim >= 1)
                {
                    // The next digit
                    BigInteger quotient = (numerator * 10) / denominator;
                    // The remainder
                    BigInteger remainder = (numerator * 10) % denominator;
                    sb.Append(quotient.ToString());
                    // To prevent doing continueing a endlessly repeating fraction (like 0.33...) add the current numerator to a list.
                    numerators.Add(numerator);
                    // Set the numerator to the remainder.
                    numerator = remainder;
                    // We have just calculated one more digit behind the decimal separator
                    lim--;
                    continue;
                }
                break;
            }
            // If our numerator is smaller than the denominator at some point during the long division we have reached the decimal separator.
            if (numerator < denominator)
            {
                //Check if we actually have any whole numbers leading the decimal separator.
                if (sb.Length == 0)
                {
                    sb.Append(0);
                }
                sb.Append(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                zero = true;
            }
        }
        return sb.ToString();
    }
    #endregion

    #region CompareTo
    public int CompareTo(Fraction other)
    {
        // NOTE: this isn't fully required, however it can be much faster since it skips the division and modulo operations below.
        if (this.denominator == other.denominator)
        {
            return this.numerator.CompareTo(other.numerator);
        }
        var divThis = this.Numerator / this.Denominator;
        var divOther = other.Numerator / other.Denominator;
        var remThis = this.Numerator % this.Denominator * this.Denominator;
        var remOther = other.Numerator % other.Denominator * other.Denominator;

        // If the quotient of the fractions is equal...
        if (divThis == divOther)
        {
            // If the remainders are equal, the fractions are equal.
            if (remThis == remOther)
            {
                return 0;
            }
            // The first fraction is bigger.
            else if (remThis > remOther)
            {
                return 1;
            }
            // The first fraction is smaller.
            else
            {
                return -1;
            }
        }
        // If the quotient of the fractions is not equal, check which fraction is larger.
        else
        {
            // Return based on whichever quotient is bigger.
            if (divThis > divOther)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
    }
    #endregion

    #region Equals
    public bool Equals(Fraction other)
    {
        var simplified = this.Simplify();
        var simplifiedOther = other.Simplify();
        return simplified.Numerator == simplifiedOther.Numerator && simplified.Denominator == simplifiedOther.Denominator;
    }

    public override bool Equals(object? obj)
    {
        if (obj is Fraction frac)
        {
            return this.Equals(frac);
        }
        return base.Equals(obj);
    }

    public override int GetHashCode() => this.ToString().GetHashCode();
    #endregion

    #endregion
}
