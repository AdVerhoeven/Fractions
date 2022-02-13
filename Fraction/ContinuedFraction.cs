using System.Numerics;
using System.Text;

namespace FractionLibrary;

public struct ContinuedFraction
{
    /// <summary>
    /// The initial value.
    /// </summary>
    public BigInteger Initial { get; private set; }
    /// <summary>
    /// The DenominatorSequence.
    /// </summary>
    public List<BigInteger> DenominatorSequence { get; private set; }
    /// <summary>
    /// This is true only for those continued Fractions that reached a point where they would end up repeating.
    /// </summary>
    public bool Repeats { get; private set; }

    public ContinuedFraction(BigInteger initial, List<BigInteger> denominatorSequence, bool repeats)
    {
        Initial = initial;
        DenominatorSequence = denominatorSequence;
        Repeats = repeats;
    }

    public ContinuedFraction(ValueTuple<BigInteger, List<BigInteger>, bool> tuple)
    {
        Initial = tuple.Item1;
        DenominatorSequence = tuple.Item2;
        Repeats = tuple.Item3;
    }

    /// <summary>
    /// Gets a list of integers that represent the initial and denominator values in the continued fraction of a given square root.
    /// </summary>
    /// <param name="n">The integer to square root</param>
    /// <returns>
    /// <para>Initial value and the denominator sequence used to approach the square root of <paramref name="n"/>.</para>
    /// <para>The boolean at the end will be false if there was no duplicate found (yet), or true if the sequence repeats.</para>
    /// </returns>
    /// <remarks>Can potentially take hours if you happen to try out a big number</remarks>
    public ContinuedFraction SqrtAsContinuedFraction(BigInteger n)
    {
        {
            //find a0, if this is the exact root, we're done
            BigInteger an = 1;
            while (an * an < n)
            {
                an++;
            }
            if (an * an == n)
            {
                return new ContinuedFraction(an, new List<BigInteger>(), false);
            }
            else
            {
                an--;
            }

            ContinuedFraction continuedFraction = new(an, new List<BigInteger>(), false);

            var signatures = new List<ValueTuple<BigInteger, BigInteger, BigInteger>>();

            //now we get into the repeating part
            BigInteger mn = 0;
            BigInteger dn = 1;
            while (true)
            {
                BigInteger mn1 = dn * an - mn;
                BigInteger dn1 = (n - (mn1 * mn1)) / dn;
                BigInteger an1 = (an + mn1) / dn1;
                var newSignature = new ValueTuple<BigInteger, BigInteger, BigInteger>(an1, mn1, dn1);
                if (signatures.Count > 0)
                {
                    if (signatures.Contains(newSignature))
                    {
                        // We have reached the end of the repeating sequence. set the bolean to true.
                        continuedFraction.Repeats = true;
                        return continuedFraction;
                    }
                    else
                    {
                        signatures.Add(newSignature);
                        continuedFraction.DenominatorSequence.Add(an1);
                    }
                }
                else
                {
                    signatures.Add(newSignature);
                    continuedFraction.DenominatorSequence.Add(an1);
                }
                //the next a has been found, now we set up to find the new next
                an = an1;
                dn = dn1;
                mn = mn1;
            }
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
    public ContinuedFraction SqrtAsContinuedFractionIn(BigInteger n, int steps)
    {
        //find a0, if this is the exact root, we're done
        BigInteger an = 1;
        while (an * an < n)
        {
            an++;
        }
        if (an * an == n)
        {
            return new ContinuedFraction(an, new List<BigInteger>(), false);
        }
        else
        {
            an--;
        }

        ContinuedFraction continuedFraction = new(an, new List<BigInteger>(), false);

        var signatures = new List<ValueTuple<BigInteger, BigInteger, BigInteger>>();

        //now we get into the repeating part
        BigInteger mn = 0;
        BigInteger dn = 1;
        while (steps > 0)
        {
            steps--;
            BigInteger mn1 = dn * an - mn;
            BigInteger dn1 = (n - (mn1 * mn1)) / dn;
            BigInteger an1 = (an + mn1) / dn1;
            var newSignature = new ValueTuple<BigInteger, BigInteger, BigInteger>(an1, mn1, dn1);
            if (signatures.Count > 0)
            {
                if (signatures.Contains(newSignature))
                {
                    // We have reached the end of the repeating sequence. set the bolean to true.
                    continuedFraction.Repeats = true;
                    return continuedFraction;
                }
                else
                {
                    signatures.Add(newSignature);
                    continuedFraction.DenominatorSequence.Add(an1);
                }
            }
            else
            {
                signatures.Add(newSignature);
                continuedFraction.DenominatorSequence.Add(an1);
            }
            //the next a has been found, now we set up to find the new next
            an = an1;
            dn = dn1;
            mn = mn1;
        }
        return continuedFraction;
    }

    public override string ToString()
    {
        var sb = new StringBuilder(DenominatorSequence.Count + 6);
        sb.Append('[');
        sb.Append(Initial);
        sb.Append(';');
        foreach (var number in DenominatorSequence)
        {
            sb.Append(number);
            sb.Append(", ");
        }
        sb.Remove(sb.Length - 2, 2);
        sb.Append(']');
        return sb.ToString();
    }
}
