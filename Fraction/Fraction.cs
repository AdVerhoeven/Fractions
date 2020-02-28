using System;
using System.Numerics;

namespace Fractions
{
    /// <summary>
    /// A class that provides fractions to calculate with.
    /// Allows using the default arithmetic operators on members of this class.
    /// Can be used in its own constructor.
    /// </summary>
    class Fraction : IComparable<Fraction>,IComparable<BigInteger>
    {
        #region Properties
        private static readonly Fraction identity = new Fraction(1, 1);
        private BigInteger numerator;
        private BigInteger denominator;

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

        public static Fraction Identity => identity;
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor.
        /// Creates a fraction object. A denominator of 0 will throw a divide by zero exception.
        /// </summary>
        /// <param name="a">Numerator</param>
        /// <param name="b">Denominator</param>
        public Fraction(BigInteger a, BigInteger b)
        {
            if (b == 0)
            {
                throw new DivideByZeroException($"Can't divide by zero. {b} is zero");
            }
            if (b < 0)
            {
                a *= -1;
                b *= -1;
            }

            Numerator = a;
            Denominator = b;
        }

        /// <summary>
        /// Creates a fraction object. A denominator of 0 will throw a divide by zero exception.
        /// </summary>
        /// <param name="a">Numerator</param>
        /// <param name="b">Denominator</param>
        public Fraction(BigInteger a, Fraction b)
        {
            if (b.Numerator == 0)
            {
                throw new DivideByZeroException($"Can't divide by zero. {b} is zero");
            }
            ////If the bottomside is negative, change the sign of both sides so that only the topside can be negative.
            //if (b.Numerator < 0)
            //{
            //    a *= -1;
            //    b.Numerator *= -1;
            //}

            Fraction ans = new Fraction(a, 1);
            ans /= b;
            Numerator = ans.Numerator;
            Denominator = ans.Denominator;
        }

        /// <summary>
        /// Creates a fraction object. A denominator of 0 will throw a divide by zero exception.
        /// </summary>
        /// <param name="a">Numerator</param>
        /// <param name="b">Denominator</param>
        public Fraction(Fraction a, BigInteger b)
        {
            if (b == 0)
            {
                throw new DivideByZeroException($"Can't divide by zero. {b} is zero");
            }
            ////If the bottomside is negative, change the sign of both sides so that only the topside can be negative.
            //if (b < 0)
            //{
            //    a.Numerator *= -1;
            //    b *= -1;
            //}

            Fraction ans = new Fraction(1, b);
            ans = a * ans;
            Numerator = ans.Numerator;
            Denominator = ans.Denominator;
        }

        /// <summary>
        /// Creates a fraction object. A denominator of 0 will throw a divide by zero exception. 
        /// </summary>
        /// <param name="a">Numerator</param>
        /// <param name="b">Denominator</param>
        public Fraction(Fraction a, Fraction b)
        {
            if (b.Numerator == 0)
            {
                throw new DivideByZeroException($"Can't divide by zero. {b} is zero");
            }
            Fraction ans = a / b;
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
        public Fraction Pow(long n)
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
            else
            {
                ans = this;
            }

            return ans.Simplify();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="depth">The precision or depth that the root function should go.</param>
        /// <returns></returns>
        public Fraction Sqrt(int depth)
        {
            Fraction top = this - 1;
            Fraction ans = new Fraction(1, 1);
            Fraction start = top / 2;
            Fraction last = start;
            for (int i = 1; i < depth; i++)
            {
                last = top / (2 + last);
            }
            ans = ans + last;
            return ans;
        }
        #endregion

        #region Operators
        #region add
        public static Fraction operator +(Fraction a, Fraction b)
            => new Fraction(a.numerator * b.denominator + b.numerator * a.denominator, a.denominator * b.denominator);

        public static Fraction operator +(Fraction a, BigInteger b) => new Fraction(a.Numerator + a.Denominator * b, a.Denominator);
        public static Fraction operator +(BigInteger a, Fraction b) => new Fraction(b.Numerator + a * b.Denominator, b.Denominator);

        #endregion
        #region subtract
        public static Fraction operator -(Fraction a, Fraction b)
            => new Fraction(a.Numerator * b.Denominator - a.Denominator * b.Numerator, a.Denominator * b.Denominator);
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
        public static Fraction operator /(Fraction a, Fraction b) => a * ~b;
        public static Fraction operator /(Fraction a, BigInteger b) => new Fraction(a.Numerator, a.Denominator * b);
        public static Fraction operator /(BigInteger a, Fraction b) => ~b * a;
        #endregion
        #region Unary
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

        #region Compare
        /// <summary>
        /// Compares a fraction object with another fraction object.
        /// Returns a not implemented exception if the object type has not been implemented.
        /// Some types can also return a not supported exception. (double.NaN)
        /// </summary>
        /// <param name="obj">Fraction, BigInteger, decimal or double</param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            if (obj is BigInteger || obj is long || obj is int || obj is short || obj is byte)
            {
                BigInteger integer = (BigInteger)obj;
                BigInteger div = this.Numerator / this.Denominator;
                if (div != integer)
                {
                    BigInteger diff = div - integer;
                    //If the difference between the two is not 0, return the sign.
                    return diff.Sign;
                }
                else
                {
                    BigInteger rem = this.Numerator % this.Denominator;
                    //If the remainder is greater then 0 the fraction is larger.
                    return (rem > 0) ? 1 : 0;
                }
            }
            else if (obj is decimal)
            {
                decimal frac = (decimal)this.Numerator / (decimal)this.Denominator;

                decimal dec = (decimal)obj;

                decimal diff = frac - dec;

                if (diff == 0)
                {
                    return 0;
                }
                else
                {
                    return (diff > 0) ? 1 : -1;
                }
            }
            else if (obj is double || obj is float)
            {
                BigInteger div = this.Numerator / this.Denominator;
                int sign = div.Sign;
                double number = (double)obj;
                try
                {
                    BigInteger num = (BigInteger)number;
                    if (div.CompareTo(num) == 0)
                    {
                        return div.CompareTo(number);
                    }
                    else
                    {
                        return div.CompareTo(num);
                    }
                }
                catch (OverflowException)
                {

                    if (double.IsNaN(number))
                    {
                        throw new NotSupportedException("Can't compare with NaN");
                    }
                    else
                    {
                        return number.CompareTo(sign);
                    }
                }
            }
            else
            {
                throw new NotImplementedException($"Cannot compare {obj.GetType()} with Fraction");
            }
        }
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
            BigInteger div = this.Numerator / this.Denominator;
            if (div != other)
            {
                BigInteger diff = div - other;
                //If the difference between the two is not 0, return the sign.
                return diff.Sign;
            }
            else
            {
                BigInteger rem = this.Numerator % this.Denominator;
                //If the remainder is greater then 0 the fraction is larger.
                return (rem > 0) ? 1 : 0;
            }
        }
        #endregion

        #endregion
    }
}
