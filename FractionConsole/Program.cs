using FractionLibrary;
using System.Diagnostics;
using System.Numerics;


Console.WriteLine("A quick start to show how precise this class can be. We calculate the root of 3 and cast at different moments");
Console.WriteLine(new Fraction(1,7).ApproximateAsString());

#region SquareRoot and formatting
var rootThreeFrac = FractionMath.Sqrt(3);
var rootThreeDouble = Math.Sqrt(3);
Console.WriteLine($"String approximation up to 18 digits: {rootThreeFrac.ApproximateAsString()}" +
    $" \nThe double of the same fraction object: {(double)rootThreeFrac}" +
    $" \nMath.Sqrt(3): {rootThreeDouble}" +
    $" \nThe difference between the two: {rootThreeDouble - (double)rootThreeFrac} (math-frac)");
Console.WriteLine($"The square of sqrt(3) as string: {(rootThreeFrac * rootThreeFrac).ApproximateAsString()}) = {(double)(rootThreeFrac * rootThreeFrac)} as double");
Console.WriteLine($"The actual fraction: {rootThreeFrac}");
Console.WriteLine("It is possible to format a fraction to take out the wholes or integers of the fraction object.");
Console.WriteLine($"The square root of three then becomes: {rootThreeFrac.ToString("B")}");
Console.WriteLine($"It is also possible to get a less precise but smaller fraction " +
    $"that approaches the square root of three." +
    $"As an example {FractionMath.Sqrt(3, 5)} approaches the square root of three.");
#endregion
Console.WriteLine();

#region PI and E
var doubFracPI = (double)FractionMath.PI;
Console.WriteLine($"FractionMath.PI = {FractionMath.PI} or {doubFracPI}");
Console.WriteLine($"Math.PI = {Math.PI}");
Console.WriteLine($"Difference at index = {StringDiffIndex(Math.PI.ToString(), doubFracPI.ToString())}");
var precisePI = new Fraction(FractionMath.PIContinuedFraction, FractionMath.PIContinuedFraction.denominatorSequence.Count);
Console.WriteLine($"The most precise approximation of PI you can get is: \n{precisePI}\nor\n{precisePI.ToString("B")}");
Console.WriteLine($"This will end up being approximately: {precisePI.ApproximateAsString(30)}");
var doubFracE = (double)FractionMath.E;
Console.WriteLine($"FractionMath.E = {FractionMath.E} or {doubFracE}");
Console.WriteLine($"Math.E = {Math.E}");
Console.WriteLine($"Difference at index = {StringDiffIndex(Math.E.ToString(), doubFracE.ToString())}");
#endregion
#region Project Euler
Console.WriteLine("The next few samples of code have execution times between 0 seconds up till several minutes.");
ExecuteTask("Project Euler 71, Ordered fractions", "several seconds", Euler71());
ExecuteTask("Project Euler 73, Counting fractions in a range", "under a minute", Euler73());
ExecuteTask("Project Euler 64, Odd period square roots.", "several seconds", Euler64());
ExecuteTask("Project Euler 72, .", "incredibly long (expect it to run 100 times longer than 73)", Euler72());
#endregion
Console.WriteLine("Press <enter> to continue...");
Console.ReadLine();


int StringDiffIndex(string a, string b)
{
    for (int i = 0; i < a.Length; i++)
    {
        if (a[i] != b[i])
        {
            return i;
        }
    }
    return -1;
}

bool ExecuteTask(string projectName, string expectedTime, Task t)
{
    Console.WriteLine($"\n{projectName}\n" +
        $"The code that you are about to execute might take a long time to complete.\n" +
        $"It is expected to run {expectedTime}.\n\n" +
        $"Do you wish to execute this code: Y/N\n");

    char key = 'a';

    while (key != 'y' && key != 'Y')
    {
        key = Console.ReadKey(true).KeyChar;
        switch (key)
        {
            case 'y':
            case 'Y':
                Console.WriteLine($"Executing: {projectName}");
                break;
            case 'n':
            case 'N':
                Console.WriteLine($"Skipped Execution of: {projectName}");
                return true;
            default:
                break;
        }
    }

    Stopwatch stopwatch = new();
    stopwatch.Start();
    t.Start();
    t.Wait();
    stopwatch.Stop();
    Console.WriteLine($"Time elapsed: {stopwatch.ElapsedMilliseconds} ms");
    Console.WriteLine($"Ticks elapsed: {stopwatch.ElapsedTicks}");
    Console.WriteLine("Press <enter> to continue...");
    Console.ReadLine();
    return t.IsCompleted;
}



/// <summary>
/// Project Euler 72, Counting Fractions
/// </summary>
Task Euler72()
{
    return new Task(() =>
    {
        BigInteger count = 0;
        BigInteger impropers = 0;

        Parallel.For(2, 1000001, i =>
         {
             if ((i & 1) == 1)
             {
                 for (int j = 1; j < i; j++)
                 {
                     var f = new Fraction(j, i);
                     if (!f.IsReduced)
                     {
                         impropers++;
                     }
                     else
                     {
                         count++;
                     }
                 }
             }
             else
             {
                 for (int j = 1; j < i; j += 2)
                 {
                     var f = new Fraction(j, i);
                     if (!f.IsReduced)
                     {
                         impropers++;
                     }
                     else
                     {
                         count++;
                     }
                 }
             }
             if (impropers > count)
             {
                 throw new Exception("Something went terribly wrong, we cannot have more improper fractions than fractions because it is a subset.");
             }
         });
    });
}


/// <summary>
/// Project euler 64, Odd period square roots
/// </summary>
Task Euler64()
{
    return new Task(() =>
    {
        int count = 0;
        for (int i = 2; i < 10000; i++)
        {
            var continuedFrac = FractionMath.SqrtAsContinuedFraction(i);
            if (continuedFrac.denominatorSequence.Count % 2 == 1)
                count++;
        }
        Console.WriteLine($"The number of odd period square roots is {count}");
    });
}

/// <summary>
/// Project euler 73, Counting fractions in a range
/// </summary>
Task Euler73()
{
    return new Task(() =>
    {
        var bottomLimit = new Fraction(1, 3);
        var topLimit = new Fraction(1, 2);
        //var fractions = new List<Fraction>();
        var count = 0;

        for (int i = 1; i <= 12000; i++)
        {
            for (int j = i / 3; j < i / 2 + 1; j++)
            {
                var f = new Fraction(j, i);
                if (f.IsReduced && f > bottomLimit && f < topLimit)
                {
                    //fractions.Add(f);
                    count++;
                }
            }
        }
        Console.WriteLine($"The number of reduced, proper fractions between 1/3 and 1/2 is: {count}");
    });
}

/// <summary>
/// Project euler 71, Ordered fractions.
/// </summary>
Task Euler71()
{
    return new Task(() =>
    {
        var limit = new Fraction(3, 7);
        var ans = new Fraction(2, 5);
        // for i <= 1.000.000 e.g. 1000000 order all proper fractions (fractions smaller than 1)
        for (int i = 500_000; i <= 1_000_000; i++)
        {

            for (int j = i / 7 * 3; j < i / 7 * 3 + 7; j++)
            {
                var f = new Fraction(j, i);
                if (f > ans && f.IsReduced && f < limit)
                {
                    ans = f;
                }
            }
        }
        Console.WriteLine($"The fraction you are looking for is: {ans}");
    });
}

