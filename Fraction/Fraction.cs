using System;
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
    public class Fraction : IComparable<Fraction>, IComparable<BigInteger>, IEquatable<Fraction>
    {
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

            Numerator = num;
            Denominator = den;
        }

        public Fraction(Fraction frac)
        {
            Numerator = frac.Numerator;
            Denominator = frac.Denominator;
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
            //OLD //If the bottomside is negative, change the sign of both sides so that only the topside can be negative.
            //if (b.Numerator < 0)
            //{
            //    a *= -1;
            //    b.Numerator *= -1;
            //}

            Fraction ans = new Fraction(num, 1);
            ans /= den;
            Numerator = ans.Numerator;
            Denominator = ans.Denominator;
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
            Numerator = ans.Numerator;
            Denominator = ans.Denominator;
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
            Numerator = ans.Numerator;
            Denominator = ans.Denominator;
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

        //TODO: Implement
        /// <summary>
        /// Returns a fractional approximation of a root.
        /// Prints exact roots if they are found in the given depth.
        /// </summary>
        /// <param name="depth">The precision or depth that the root function should go.</param>
        /// <returns></returns>
        public Fraction Sqrt(int depth)
        {
            throw new NotImplementedException();
            //Fraction top = this - 1;
            //Fraction ans = new Fraction(1, 1);
            //Fraction start = top / 2;
            //Fraction last = start;
            //for (int i = 1; i < depth; i++)
            //{
            //    last = top / (2 + last);
            //}
            //ans += last;
            //return ans;
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
            if (obj is Fraction)
            {
                return this.Equals(obj as Fraction);
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
