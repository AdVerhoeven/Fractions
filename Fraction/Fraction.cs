using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace FractionLibrary
{
    /// <summary>
    /// A class that provides fractions to calculate with.
    /// Allows using the default arithmetic operators on members of this class.
    /// The sign is always on the Numerator.
    /// Can be compared with integers, doubles and fractions.
    /// Can be used within its own constructors for simplified writing of recursive behaviour.
    /// </summary>
    public struct Fraction : IComparable<Fraction>, IComparable<BigInteger>, IEquatable<Fraction>
    {
        #region Conversions
        // Implicit Convserions
        public static implicit operator Fraction(BigInteger b) => new Fraction(b, 1);
        public static implicit operator Fraction(int i) => new Fraction(i, 1);

        //HACK: We do not really need this implicit conversion since it will not be used.
        //public static implicit operator Fraction(long l) => new Fraction(l, 1);

        // Explicit conversions
        public static explicit operator BigInteger(Fraction f) => f.Numerator / f.Denominator;
        public static explicit operator double(Fraction f) => (double)f.Numerator / (double)f.Denominator;
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
            if (den < 0)
            {
                num *= -1;
                den *= -1;
            }

            numerator = num;
            denominator = den;
        }

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
        /// </summary>
        /// <param name="continuedFractions">The sequence of denominators in the continued fraction.</param>
        /// <param name="steps">The amount of steps to execute the continued fration.</param>
        public Fraction(Dictionary<int, List<int>> continuedFractions, int steps)
        {
            int initial = continuedFractions.First().Key;
            List<int> period = continuedFractions.First().Value;
            int periodLength = period.Count;

            Fraction r = Fraction.Identity;
            if (steps == 0)
            {
                r = new Fraction(initial, 1);
            }
            else
            {
                steps--;
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

        /// <summary>
        /// Returns the n'th power of a Fraction.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public Fraction Pow(int n)
        {
            Fraction ans = Identity;

            if (n > 1)
            {
                ans = this * this.Pow(--n);
            }
            else if (n < 0)
            {
                ans = this.Pow(-1 * n);
            }
            else if (n == 1)
            {
                ans = this;
            }

            return ans.Simplify();
        }

        /// <summary>
        /// Approximates as a Fraction to a double.
        /// </summary>
        /// <returns></returns>
        public double Approximate()
        {
            return (double)Numerator / (double)Denominator;
        }

        /// <summary>
        /// Approximates a Fraction to a floating point string.
        /// The brackets encapsulate the repeating sequence within the real/floating point number.
        /// </summary>
        /// <returns></returns>
        public string ApproximateAsString()
        {
            //TODO: fix string limit?
            return ApproximateAsString(1000);
        }

        /// <summary>
        /// Approximates a Fraction to a floating point string.
        /// The brackets encapsulate the repeating sequence within the real/floating point number.
        /// </summary>
        /// <param name="lim">The amount of digits behind the decimal point to be found.</param>
        /// <returns>A floating point string.</returns>
        public string ApproximateAsString(int lim)
        {
            string ans = string.Empty;
            BigInteger numerator = this.Numerator;
            BigInteger denominator = this.Denominator;
            bool zero = false;
            List<BigInteger> numerators = new List<BigInteger>();
            while (numerator != 0)
            {
                if (numerator > denominator && !zero)
                {
                    BigInteger quotient = numerator / denominator;
                    BigInteger rest = numerator % denominator;
                    ans += quotient.ToString();
                    numerator = rest;
                }
                if (zero)
                {
                    if (lim < 1)
                    {
                        break;
                    }
                    BigInteger quotient = (numerator * 10) / denominator;
                    BigInteger rest = (numerator * 10) % denominator;
                    ans += quotient.ToString();
                    numerators.Add(numerator);
                    numerator = rest;
                    lim--;
                }
                if (numerator < denominator && !zero)
                {
                    if (ans == string.Empty)
                    {
                        ans = "0";
                    }
                    ans += ".";
                    zero = true;
                }
            }
            return ans;
        }

        //TODO: rewrite or overload to allow for rooting of fractions
        public Dictionary<int, List<int>> SqrtAsContinuedFraction(int n)
        {
            Dictionary<int, List<int>> continuedFraction = new Dictionary<int, List<int>>();
            //find a0, if this is the exact root, we're done
            int an = 1;
            while (an * an < n)
            {
                an++;
            }
            if (an * an == n)
            {
                continuedFraction.Add(an, new List<int>());
                return continuedFraction;
            }
            else
            {
                an--;
            }
            int initial = an;
            continuedFraction.Add(initial, new List<int>());
            List<Tuple<int, int, int>> signatures = new List<Tuple<int, int, int>>();

            //now we get into the repeating part
            int mn = 0;
            int dn = 1;
            while (true)
            {
                int mn1 = dn * an - mn;
                int dn1 = (n - (mn1 * mn1)) / dn;
                int an1 = (initial + mn1) / dn1;
                Tuple<int, int, int> newSignature = new Tuple<int, int, int>(an1, mn1, dn1);
                if (signatures.Count > 0)
                {
                    if (signatures.Contains(newSignature))
                    {
                        return continuedFraction;
                    }
                    else
                    {
                        signatures.Add(newSignature);
                        continuedFraction[initial].Add(an1);
                    }
                }
                else
                {
                    signatures.Add(newSignature);
                    continuedFraction[initial].Add(an1);
                }
                //the next a has been found, now we set up to find the new next
                an = an1;
                dn = dn1;
                mn = mn1;
            }
        }

        public Dictionary<Fraction, List<Fraction>> SqrtAsContinuedFraction(Fraction n)
        {
            Dictionary<Fraction, List<Fraction>> continuedFraction = new Dictionary<Fraction, List<Fraction>>();
            //find a0, if this is the exact root, we're done
            Fraction an = Fraction.Identity;
            while (an * an < n)
            {
                an.Numerator += an.Denominator;
            }
            if (an * an == n)
            {
                continuedFraction.Add(an, new List<Fraction>());
                return continuedFraction;
            }
            else
            {
                an.Numerator -= an.Denominator;
            }
            Fraction initial = an;
            continuedFraction.Add(initial, new List<Fraction>());
            List<Tuple<Fraction, Fraction, Fraction>> signatures = new List<Tuple<Fraction, Fraction, Fraction>>();

            //now we get into the repeating part
            Fraction mn = new Fraction(0, 1);
            Fraction dn = Fraction.Identity;
            while (true)
            {
                Fraction mn1 = dn * an - mn;
                Fraction dn1 = (n - (mn1 * mn1)) / dn;
                Fraction an1 = (initial + mn1) / dn1;
                Tuple<Fraction, Fraction, Fraction> newSignature = new Tuple<Fraction, Fraction, Fraction>(an1, mn1, dn1);
                if (signatures.Count > 0)
                {
                    if (signatures.Contains(newSignature))
                    {
                        return continuedFraction;
                    }
                    else
                    {
                        signatures.Add(newSignature);
                        continuedFraction[initial].Add(an1);
                    }
                }
                else
                {
                    signatures.Add(newSignature);
                    continuedFraction[initial].Add(an1);
                }
                //the next a has been found, now we set up to find the new next
                an = an1;
                dn = dn1;
                mn = mn1;
            }
        }
        #endregion

        #region Operators
        #region add
        public static Fraction operator +(Fraction a, Fraction b)
            => new Fraction(a.numerator * b.denominator + b.numerator * a.denominator, a.denominator * b.denominator);

        public static Fraction operator +(Fraction a, BigInteger b) => new Fraction(a.Numerator + a.Denominator * b, a.Denominator);
        public static Fraction operator +(BigInteger a, Fraction b) => new Fraction(b.Numerator + a * b.Denominator, b.Denominator);
        //public static double operator +(double a, Fraction b) => a + (double)b.Numerator / (double)b.Denominator;
        #endregion
        #region subtract
        public static Fraction operator -(Fraction a, Fraction b)
            => new Fraction(a.Numerator * b.Denominator - a.Denominator * b.Denominator, a.Denominator * b.Denominator);
        public static Fraction operator -(Fraction a, BigInteger b) => new Fraction(a.Numerator - a.Denominator * b, a.Denominator);

        public static Fraction operator -(BigInteger a, Fraction b) => new Fraction(b.Numerator - a * b.Denominator, b.Denominator);
        #endregion
        #region multiply
        public static Fraction operator *(Fraction a, Fraction b)
            => new Fraction(a.Numerator * b.Numerator, a.Denominator * b.Denominator);
        public static Fraction operator *(Fraction a, BigInteger b) => new Fraction(a.Numerator * b, a.Denominator);
        public static Fraction operator *(BigInteger a, Fraction b) => b * a;
        #endregion
        #region divide
        /// <summary>
        /// Divides a fraction with a fraction.
        /// </summary>
        /// <param name="a">Fraction</param>
        /// <param name="b">Fraction</param>
        /// <returns>Fraction</returns>
        public static Fraction operator /(Fraction a, Fraction b) => a * ~b;
        /// <summary>
        /// Divides a fraction with an integer.
        /// </summary>
        /// <param name="a">Fraction</param>
        /// <param name="b">(Big)Integer</param>
        /// <returns>Fraction</returns>
        public static Fraction operator /(Fraction a, BigInteger b) => new Fraction(a.Numerator, a.Denominator * b);
        /// <summary>
        /// Divides an integer with a fraction.
        /// </summary>
        /// <param name="a">(Big)Integer</param>
        /// <param name="b">Fraction</param>
        /// <returns>Fraction</returns>
        public static Fraction operator /(BigInteger a, Fraction b) => ~b * a;
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

        /// <summary>
        /// Returns the fraction as "(a / b)"
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"({this.Numerator} / {this.Denominator})";

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
        public int CompareTo(BigInteger other)
        {
            BigInteger frac = Numerator / Denominator;
            if (frac != other)
            {
                return (frac - other).Sign;
            }
            else
            {
                BigInteger rem = Numerator % Denominator;
                if (rem == 0)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
        }


        #endregion

        #region Equals
        public bool Equals(Fraction other)
        {
            return this.Numerator == other.Numerator && this.Denominator == other.Denominator;
        }

        //HACK Equals override to prevent stupid behavior in unittesting.
        /// <summary>
        /// Stops the Assert.AreEqual from defying logic.
        /// Why would I want to do the same as with Assert.AreSame????
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
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
