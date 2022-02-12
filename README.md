# Fractions
A small program that can calculate with precise fractions.
The Fraction struct works with BigIntegers but can implicitly and explicitly be converted to most other numerical ValueTypes in C#.
If precision might be lost upon converting it must be done explicitly, but putting any integer into a fraction works just fine because there would be no information lost were you to cast it back.
The FractionMath class provides a few extra's that enable taking the root or the power of a fraction and a more precise approximation method.
The Fraction struct in this project is mostly aimed at being as precise as possible.
Code that uses very big operations or fractions might be slow to execute but the answer will be as precise as you aimed it to be.

Short explanation on the Square Root methods provided:

To build the continued fraction that is required for the approximation of irrational square roots we perform a lot of divisions.
At some point it might end up being a repeating sequence, instead of calculating the rest of the sequence, it stops and sets the boolean at the end to true.
This is a sign that the sequence is complete, but it will repeat an infinite amount.
The continued fraction of the square root of 3 for example is mathematically often denoted as: \[1;1,2,1,2,…\]
This would end up looking like (1,{1,2},true) inside the FractionMath class.
The assumption I have made here is that any square root that is not an integer has no finite rational representation.
Therefor any approximation will include a never ending continued fraction.
These continued fractions do however repeat themselves at one point.

The constructor accepts a continued fraction in this form and a integer to tell it how many steps must be taken.
I will not be rewriting or refactoring the code much more for now because of the complexity.

Implements: IComparable\<Fraction\>, IEquatable\<Fraction\>, IFormattable

Arithmetic operators:

Fraction + Fraction, addition
Fraction - Fraction, subtraction
Fraction * Fraction, multiplication
Fraction / Fraction, division
~Fraction, inverst Fraction 1/2 =\> 2/1
-Fraction, negate Fraction
+Fraction, Fraction, does not have any effects as of yet.

Comparison operators:

==, equals
!=, not equals
\>, greater than
\<, lesser than
\>=, greater than or equal
\<=, lesser than or equal

Casting operators:

implicit to (Fraction) casts: sbyte, byte, ushort, short, uint, int, ulong, long, BigInteger

explicit Fraction to casts: int, double, decimal, BigInteger

Properties:

Numerator

Denominator
    
Identity => Fraction(1,1), multiplying or dividing by this should never change a fraction.

IsProper, checks if the Fraction is a proper fraction, false if you can take out an integer and a fraction (e.g. 1+1/2 is improper/false)

IsReduced, checks if the Fraction is in its reduced form, false if it can be reduced/simplified.



