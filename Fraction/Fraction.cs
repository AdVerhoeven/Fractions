using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace FractionLibrary
{
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
        // HACK: ToString() isn't called in Console.WriteLine()???
        public static implicit operator String(Fraction f) => f.ToString();

        //HACK: We do not really need this implicit conversion since it will not be used, it could however be added.
        //public static implicit operator Fraction(long l) => new Fraction(l, 1);

        // Explicit conversions
        public static explicit operator BigInteger(Fraction f) => f.Numerator / f.Denominator;
        // a double has a limited precision of 15-17 decimal digits.
        public static explicit operator double(Fraction f) => double.Parse(f.ApproximateAsString(17));
        #endregion

        #region Properties
        private BigInteger numerator;
        private BigInteger denominator;
        public static Fraction Identity { get; } = new Fraction(1, 1);

        /// <summary>
        /// The Nominator of the fraction.
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
        /// The Denominator of the fraction.
        /// </summary>
        public BigInteger Denominator
        {
            set
            {
                if (value > 0)
                {
                    denominator = value;
                }
                else if(value < 0)
                {
                    numerator *= -1;
                    denominator *= -1;
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

            Fraction ans = new Fraction(num, 1);
            ans /= den;
            numerator = ans.Numerator;
            denominator = ans.Denominator;
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
            ////If the bottomside is negative, change the sign of both sides so that only the topside can be negative.
            //if (b < 0)
            //{
            //    a.Numerator *= -1;
            //    b *= -1;
            //}

            Fraction ans = new Fraction(1, den);
            ans = num * ans;
            numerator = ans.Numerator;
            denominator = ans.Denominator;
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
            Fraction ans = num / den;
            numerator = ans.Numerator;
            denominator = ans.Denominator;
        }

        /// <summary>
        /// Approximates a fraction with a given (repeating?) sequence.
        /// The sequence can/should not contain any zeroes.
        /// </summary>
        /// <param name="continuedFractions">The sequence of denominators in the continued fraction.</param>
        /// <param name="steps">The amount of steps to execute the continued fration.</param>
        public Fraction(KeyValuePair<BigInteger, List<BigInteger>> continuedFractions, int steps)
        {
            var initial = continuedFractions.Key;
            var period = continuedFractions.Value;
            var periodLength = period.Count;
            if (periodLength == 0)
                steps = 0;

            Fraction r;
            if (steps == 0)
            {
                r = new Fraction(initial, 1);
            }
            else
            {
                steps--;
                //An invalid denominator list (e.g. one that contains zero) will throw a divide by zero exception
                r = new Fraction(1, period[steps % periodLength]);
                while (steps > 0)
                {
                    //a + 1/r
                    steps--;
                    r = 1 / (period[steps % periodLength] + r);
                    //r = period[steps % periodLength] + new Fraction(1/r); 
                }
                r = (initial + r);
            }

            numerator = r.Numerator;
            denominator = r.Denominator;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns the simplified fraction by dividing the nominator and denominator by their greatest common divisor.
        /// </summary>
        /// <param name="frac"></param>
        /// <returns></returns>
        public Fraction Simplify()
        {
            BigInteger gcd = GCD(this.Numerator, this.Denominator);

            Fraction ans = new Fraction(this.Numerator / gcd, this.Denominator / gcd);

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
        public static bool operator ==(Fraction a, Fraction b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(Fraction a, Fraction b) => !(a == b);
        public static bool operator >(Fraction a, Fraction b)
        {
            var c = a - b;
            //if a - b results in 0, it is false, otherwise return if a - b is larger than 0.
            return c.Numerator != 0 ? c.Numerator > 0 : false;
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
        public override string ToString() => $"({this.Numerator} / {this.Denominator})";

        /// <summary>
        /// Returns a fraction as "(a / b)" or "(a + b / c)"
        /// </summary>
        /// <param name="format">S = full integers separately</param>
        /// <returns></returns>
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
                default:
                    throw new FormatException(String.Format("The {0} format string is not supported.", format));
            }
        }

        // TODO: Implement?
        /// <summary>
        /// Not implemented do not use.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (formatProvider == null)
                return ToString(format);
            return ToString();
        }
        #endregion

        #region CompareTo
        public int CompareTo(Fraction fraction)
        {
            BigInteger divA = this.Numerator / this.Denominator;
            BigInteger divB = fraction.Numerator / fraction.Denominator;
            BigInteger remA = this.Numerator % this.Denominator * this.Denominator;
            BigInteger remB = fraction.Numerator % fraction.Denominator * fraction.Denominator;

            if (divA == divB)
            {
                if (remA == remB)
                {
                    return 0;
                }
                else if (remA > remB)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                if (divA > divB)
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

        public override bool Equals(object obj)
        {
            if (obj is Fraction frac)
            {
                return this.Equals(frac);
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
        #endregion

        #endregion
    }
}
