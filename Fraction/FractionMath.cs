using System.Numerics;

namespace FractionLibrary
{
    /// <summary>
    /// A class containing several extension methods for expensive calculations on fractions. 
    /// Including approximating, rooting and taking a power of fractions.
    /// </summary>
    public static class FractionMath
    {
        #region Constants
        /// <summary>
        /// Relative error of 0.00_000_008 or 8*10^-10
        /// </summary>
        public static readonly Fraction PI = new(355, 113);

        //public static readonly Fraction E = new Fraction(); TODO: research if eulers number can be approached with a fraction

        /// <summary>
        /// A Fraction that approaches the golden ratio after taking 100 steps.
        /// A more precise answer can be obtained by using a continued fraction consisting of initial value 1 and repeating 1
        /// </summary>
        public static readonly Fraction GoldenRatio = new(continuedFractions: (1, new List<BigInteger>(1) { 1 }, true), 100);
        #endregion

        #region Square root Methods
        /// <summary>
        /// Takes the square root out of a number by approximating it with a continued fraction.
        /// </summary>
        /// <param name="n">The input <see cref="BigInteger"/></param>
        /// <param name="steps">The number of steps to use to calculate the root</param>
        /// <returns>A fractional approach of the root of <paramref name="n"/> in the given amount of <paramref name="steps"/></returns>
        public static Fraction Sqrt(BigInteger n, int steps = 30)
        {
            if (n < 0)
            {
                throw new ArgumentException($"Cannot take the root of {n} because it is negative.");
            }
            if (steps < 0)
            {
                throw new ArgumentException($"Cannot take a negative amount of steps");
            }
            return new Fraction(SqrtAsContinuedFraction(n, steps), steps);
        }

        /// <summary>
        /// Gets a list of integers that represent the initial and denominator values in the continued fraction of a given square root.
        /// </summary>
        /// <param name="n">The integer to square root</param>
        /// <param name="steps">The maximum amount of steps or continued fraction entries to be had.</param>
        /// <returns>
        /// <para>Initial value and the denominator sequence used to approach the square root of <paramref name="n"/>.</para>
        /// <para>The boolean at the end will be false if there was no duplicate found (yet), or true if the sequence repeats.</para>
        /// </returns>
        public static (BigInteger intial, List<BigInteger> denominatorSequence, bool repeats) SqrtAsContinuedFraction(BigInteger n)
        {
            //find a0, if this is the exact root, we're done
            BigInteger an = 1;
            while (an * an < n)
            {
                an++;
            }
            if (an * an == n)
            {
                return new ValueTuple<BigInteger, List<BigInteger>, bool>(an, new List<BigInteger>(), false);
            }
            else
            {
                an--;
            }
            BigInteger initial = an;

            (BigInteger intial, List<BigInteger> denominatorSequence, bool repeats) continuedFraction = 
                new(initial, new List<BigInteger>(), false);

            var signatures = new List<ValueTuple<BigInteger, BigInteger, BigInteger>>();

            //now we get into the repeating part
            BigInteger mn = 0;
            BigInteger dn = 1;
            while (true)
            {
                BigInteger mn1 = dn * an - mn;
                BigInteger dn1 = (n - (mn1 * mn1)) / dn;
                BigInteger an1 = (initial + mn1) / dn1;
                var newSignature = new ValueTuple<BigInteger, BigInteger, BigInteger>(an1, mn1, dn1);
                if (signatures.Count > 0)
                {
                    if (signatures.Contains(newSignature))
                    {
                        // We have reached the end of the repeating sequence. set the bolean to true.
                        continuedFraction.repeats = true;
                        return continuedFraction;
                    }
                    else
                    {
                        signatures.Add(newSignature);
                        continuedFraction.denominatorSequence.Add(an1);
                    }
                }
                else
                {
                    signatures.Add(newSignature);
                    continuedFraction.denominatorSequence.Add(an1);
                }
                //the next a has been found, now we set up to find the new next
                an = an1;
                dn = dn1;
                mn = mn1;
            }
        }

        /// <summary>
        /// Gets a list of integers that represent the initial and denominator values in the continued fraction of a given square root.
        /// </summary>
        /// <param name="n">The integer to square root</param>
        /// <param name="steps">The maximum amount of steps or continued fraction entries to be had.</param>
        /// <returns>
        /// <para>Initial value and the denominator sequence used to approach the square root of <paramref name="n"/>.</para>
        /// <para>The boolean at the end will be false if there was no duplicate found (yet), or true if the sequence repeats.</para>
        /// </returns>
        public static (BigInteger intial, List<BigInteger> denominatorSequence, bool repeats) SqrtAsContinuedFraction(BigInteger n, int steps)
        {
            //find a0, if this is the exact root, we're done
            BigInteger an = 1;
            while (an * an < n)
            {
                an++;
            }
            if (an * an == n)
            {
                return new ValueTuple<BigInteger, List<BigInteger>, bool>(an, new List<BigInteger>(), false);
            }
            else
            {
                an--;
            }
            BigInteger initial = an;

            (BigInteger intial, List<BigInteger> denominatorSequence, bool repeats) continuedFraction =
                new(initial, new List<BigInteger>(), false);

            var signatures = new List<ValueTuple<BigInteger, BigInteger, BigInteger>>();

            //now we get into the repeating part
            BigInteger mn = 0;
            BigInteger dn = 1;
            while (steps > 0)
            {
                steps--;
                BigInteger mn1 = dn * an - mn;
                BigInteger dn1 = (n - (mn1 * mn1)) / dn;
                BigInteger an1 = (initial + mn1) / dn1;
                var newSignature = new ValueTuple<BigInteger, BigInteger, BigInteger>(an1, mn1, dn1);
                if (signatures.Count > 0)
                {
                    if (signatures.Contains(newSignature))
                    {
                        // We have reached the end of the repeating sequence. set the bolean to true.
                        continuedFraction.repeats = true;
                        return continuedFraction;
                    }
                    else
                    {
                        signatures.Add(newSignature);
                        continuedFraction.denominatorSequence.Add(an1);
                    }
                }
                else
                {
                    signatures.Add(newSignature);
                    continuedFraction.denominatorSequence.Add(an1);
                }
                //the next a has been found, now we set up to find the new next
                an = an1;
                dn = dn1;
                mn = mn1;
            }
            return continuedFraction;
        }

        /// <summary>
        /// Attempts to find the root of a fraction. Warning: this method can be very expensive.
        /// The result is sqrt(numerator)/sqrt(denominator). hence the expensive execution.
        /// </summary>
        /// <param name="f">The fraction to take the root from</param>
        /// <returns>The square root of f</returns>
        public static Fraction Sqrt(this Fraction f, int steps = 30)
        {
            // Check if the fraction is 1, this is WAY faster than using the method below
            if (f.Numerator == f.Denominator)
            {
                return 1;
            }
            else if (f.Numerator == 0)
            {
                return 0;
            }
            try
            {
                // Sqrt(a/b) == Sqrt(a)/Sqrt(b) 
                var num = Sqrt((int)f.Numerator, steps);
                var den = Sqrt((int)f.Denominator, steps);
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
        /// Also simplifies the answer to get the smallest possible fractional representation.
        /// </summary>
        /// <param name="f">The fraction to power</param>
        /// <param name="n">The power</param>
        /// <returns>f to the power of n</returns>
        public static Fraction Pow(this Fraction f, int n)
        {
            if (n > 1)
            {
                // As long as our power is larger than 1 we multiply f * f^(n-1)
                return (f * Pow(f, --n)).Simplify();
            }
            else if (n < 0)
            {
                // A negative power equals 1 / f^(-n)
                return (1 / Pow(f, -1 * n)).Simplify();
            }
            else if (n == 1)
            {
                // f^1 = f
                return f;
            }
            else
            {
                // n = 0 so we return 1 or Fraction.Identity.
                return Fraction.Identity;
            }
        }
        #endregion
    }
}
