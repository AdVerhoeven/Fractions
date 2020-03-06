﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;

namespace FractionLibrary
{
    /// <summary>
    /// A class containing several extension methods for expensive calculations on fractions. 
    /// Including approximating, rooting and taking a power of fractions.
    /// </summary>
    public static class FractionMath
    {
        #region Square root Methods
        /// <summary>
        /// Takes the square root out of a number by approximating it with a continued fraction.
        /// </summary>
        /// <param name="n">Returns a fraction </param>
        /// <returns></returns>
        public static Fraction Sqrt(int n)
        {
            //HACK: default precision of 30, more can/could be achieved.
            return new Fraction(SqrtAsContinuedFraction(n), 30);
        }

        // TODO: Add a method that has a limit? This might give a significant speed-up without to much loss of precision.
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
        /// Attempts to find the root of a fraction. Warning: this method can be very expensive.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Fraction Sqrt(this Fraction f)
        {
            // Check if the fraction is 1, this is WAY faster than using the method below
            if (f.Numerator == f.Denominator)
            {
                return 1;
            }
            try
            {
                // Sqrt(a/b) == Sqrt(a)/Sqrt(b) 
                var num = Sqrt((int)f.Numerator);
                var den = Sqrt((int)f.Denominator);
                return new Fraction(num, den);
            }
            catch (InvalidCastException ex)
            {
                //TODO: change SqrtAsContinuedFraction to accept BigIntegers. Why would you want to do that though?
                throw new OverflowException($"This fraction has a numerator or denominator that is to big.", ex);
            }
        }
        #endregion

        #region Power methods
        /// <summary>
        /// Provides the ability to power a fraction to any integer power.
        /// </summary>
        /// <param name="f">The fraction to power</param>
        /// <param name="n">The power</param>
        /// <returns>f to the power of n</returns>
        public static Fraction Pow(this Fraction f, int n)
        {
            Fraction ans;

            if (n > 1)
            {
                // As long as our power is larger than 1 we multiply f * f^(n-1)
                ans = f * Pow(f, --n);
            }
            else if (n < 0)
            {
                // A negative power equals 1 / f^(-n)
                ans = 1 / Pow(f, -1 * n);
            }
            else if (n == 1)
            {
                // f^1 = f
                ans = f;
            }
            else
            {
                // n = 0 so we return 1 or Fraction.Identity.
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
        /// <param name="lim">The amount of digits behind the decimal separator to be found.</param>
        /// <returns>A floating point string.</returns>
        public static string ApproximateAsString(this Fraction f, int lim)
        {
            //TODO: Use stringbuilder?            
            string ans = string.Empty;

            BigInteger numerator = f.Numerator;
            BigInteger denominator = f.Denominator;
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
                    ans += quotient.ToString();                    
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
                        ans += quotient.ToString();
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