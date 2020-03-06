using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FractionLibrary
{
    public static class FracMath
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
        public static Fraction Sqrt(Fraction f)
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
        public static Fraction Pow(Fraction f, int n)
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
    }
}
