﻿using System;
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
        // NOTE: We do not really need these implicit conversions since it will not be used most of the time, it could however be added. 
        // BigIntegers have the implicit conversions so the BigInteger conversion should suffice for most situations.
        //public static implicit operator Fraction(long l) => new Fraction(l, 1);
        //public static implicit operator Fraction(short s) => new Fraction(s, 1);
        //public static implicit operator Fraction(ulong ul) => new Fraction(ul, 1);

        // Explicit conversions
        public static explicit operator BigInteger(Fraction f) => f.Numerator / f.Denominator;
        // a double has a limited precision of 15-17 decimal digits.
        public static explicit operator double(Fraction f) => double.Parse(f.ApproximateAsString(17));

        public static explicit operator int(Fraction f)
        {
            var ans = f.Numerator / f.Denominator;
            return (ans < int.MaxValue) ? (int)ans : throw new OverflowException("This fraction is to big");
        }
        #endregion

        #region Properties
        private BigInteger numerator;
        private BigInteger denominator;
        public static Fraction Identity { get; } = new Fraction(1, 1);

        /// <summary>
        /// The Nominator or top part of the fraction.
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

        /// <summary>
        /// Checks wether a fraction is a proper fraction.
        /// </summary>
        public bool IsProper => BigInteger.Abs(this.Numerator) < this.Denominator;

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
        /// Approximates a fraction with a given (repeating) sequence.
        /// The sequence can/should not contain any zeroes.
        /// </summary>
        /// <param name="continuedFractions">The sequence of denominators in the continued fraction.</param>
        /// <param name="steps">The amount of steps to execute the continued fration.</param>
        public Fraction(ValueTuple<BigInteger, List<BigInteger>, bool> continuedFractions, int steps)
        {
            var initial = continuedFractions.Item1;
            var period = continuedFractions.Item2;
            var periodLength = period.Count;
            if (periodLength == 0)
                steps = 0;

            if (steps == 0)
            {
                this = new Fraction(initial, 1);
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
                this = (initial + this);
            }
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
        /// Returns a fraction as "(a / b)" ("G" or "S") or "(a + b / c)" ("B").
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

        // TODO: Implement properly?
        /// <summary>
        /// Not implemented do not use.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (formatProvider == null)
                return ToString();
            return ToString();
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

        public override bool Equals(object obj)
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
}
