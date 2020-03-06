using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;

namespace FractionLibrary
{
    /// <summary>
    /// A class containing several extension methods for expensive calculations on fractions.
    /// </summary>
    public static class FractionMath
    {
        #region Square root Methods
        /// <summary>
        /// Takes the square root out of a number.
        /// </summary>
        /// <param name="n">Returns a fraction </param>
        /// <returns></returns>
        public static Fraction Sqrt(int n)
        {
            //HACK: default precision of 30, more can/could be achieved.
            return new Fraction(SqrtAsContinuedFraction(n), 30);
        }

        /// <summary>
        /// Gets a list of integers that represent the initial and denominator values in the continued fraction of a given square root.
        /// </summary>
        /// <param name="n"></param>
        /// <returns>Initial value and the denominator sequence</returns>
        public static KeyValuePair<int, List<int>> SqrtAsContinuedFraction(int n)
        {
            //find a0, if this is the exact root, we're done
            int an = 1;
            while (an * an < n)
            {
                an++;
            }
            if (an * an == n)
            {
                return new KeyValuePair<int, List<int>>(an, new List<int>());
            }
            else
            {
                an--;
            }
            int initial = an;
            var continuedFraction = new KeyValuePair<int, List<int>>(initial, new List<int>());
            var signatures = new List<Tuple<int, int, int>>();

            //now we get into the repeating part
            int mn = 0;
            int dn = 1;
            while (true)
            {
                int mn1 = dn * an - mn;
                int dn1 = (n - (mn1 * mn1)) / dn;
                int an1 = (initial + mn1) / dn1;
                var newSignature = new Tuple<int, int, int>(an1, mn1, dn1);
                if (signatures.Count > 0)
                {
                    if (signatures.Contains(newSignature))
                    {
                        return continuedFraction;
                    }
                    else
                    {
                        signatures.Add(newSignature);
                        continuedFraction.Value.Add(an1);//.Value.Add
                    }
                }
                else
                {
                    signatures.Add(newSignature);
                    continuedFraction.Value.Add(an1);
                }
                //the next a has been found, now we set up to find the new next
                an = an1;
                dn = dn1;
                mn = mn1;
            }
        }

        /// <summary>
        /// Attempts to find the root of a fraction. Warning: this method is very expensive.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Fraction Sqrt(this Fraction f)
        {
            if (f.Numerator == f.Denominator)
            {
                return 1;
            }
            try
            {
                var num = Sqrt((int)f.Numerator);
                var den = Sqrt((int)f.Denominator);
                return new Fraction(num, den);
            }
            catch (InvalidCastException ex)
            {
                throw new OverflowException($"This fraction has a numerator or denominator that is to big.", ex);
            }
        }
        #endregion

        #region Power methods
        public static Fraction Pow(this Fraction f, int n)
        {
            Fraction ans;

            if (n > 1)
            {
                ans = f * Pow(f, --n);
            }
            else if (n < 0)
            {
                ans = 1 / Pow(f, -1 * n);
            }
            else if (n == 1)
            {
                ans = f;
            }
            else
            {
                ans = Fraction.Identity;
            }

            return ans.Simplify();
        }
        #endregion

        #region Approximate Methods
        /// <summary>
        /// Approximates as a Fraction to a double. For full precision use the ApproximatePrecise() method.
        /// </summary>
        /// <returns></returns>
        public static double Approximate(this Fraction f)
        {
            var t = (double)f.Numerator / (double)f.Denominator;
            if (double.IsNaN(t))
            {
                return double.Parse(ApproximateAsString(f, 10));
            }
            // A double has a limited precision of 15-17 decimal digits.
            return (double.IsNaN(t)) ? double.Parse(ApproximateAsString(f)) : t;
        }

        /// <summary>
        /// Approximates a Fraction to a floating point string.
        /// The brackets encapsulate the repeating sequence within the real/floating point number.
        /// </summary>
        /// <returns></returns>
        public static string ApproximateAsString(this Fraction f)
        {
            //HACK: Default limit of 30 decimal digits.
            return ApproximateAsString(f, 30);
        }

        /// <summary>
        /// Approximates a Fraction to a floating point string.
        /// The brackets encapsulate the repeating sequence within the real/floating point number.
        /// </summary>
        /// <param name="lim">The amount of digits behind the decimal point to be found.</param>
        /// <returns>A floating point string.</returns>
        public static string ApproximateAsString(this Fraction f, int lim)
        {
            //TODO: Use stringbuilder?
            string ans = string.Empty;
            BigInteger numerator = f.Numerator;
            BigInteger denominator = f.Denominator;
            bool zero = false;
            List<BigInteger> numerators = new List<BigInteger>();
            while (numerator != 0)
            {
                // Long divison
                if (numerator > denominator && !zero)
                {
                    BigInteger quotient = numerator / denominator;
                    BigInteger rest = numerator % denominator;
                    ans += quotient.ToString();
                    numerator = rest;
                }
                // Once we have calculated the whole numbers we get into this part.
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
                    ans += CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                    zero = true;
                }
            }
            return ans;
        }

        #endregion
    }
}
