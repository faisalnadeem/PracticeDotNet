using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpPlayGround
{
    public class TuplesTest
    {
        public static void MainTest()
        {
            var ToString = "This is some text";
            var one = 1;
            var Item1 = 5;
            var projections = (ToString, one, Item1);
            // Accessing the first field:
            Console.WriteLine(projections.Item1);
            // There is no semantic name 'ToString'
            // Accessing the second field:
            Console.WriteLine(projections.one);
            Console.WriteLine(projections.Item2);
            // Accessing the third field:
            Console.WriteLine(projections.Item3);
            // There is no semantic name 'Item1`.

            var pt1 = (X: 3, Y: 0);
            var pt2 = (X: 3, Y: 4);

            var xCoords = (pt1.X, pt2.X);
            // There are no semantic names for the fields
            // of xCoords.

            // Accessing the first field:
            Console.WriteLine(xCoords.Item1);
            // Accessing the second field:
            Console.WriteLine(xCoords.Item2);


            EqualityAndTuples();
            AssignmentsAndTuples();
            TuplesAsMethodReturnValues();
        }

        public static void EqualityAndTuples()
        {
            Console.WriteLine("Equality and tuples");
            var left = (a: 5, b: 10);
            var right = (a: 5, b: 10);
            Console.WriteLine(left == right); // displays 'true'

            left = (a: 5, b: 10);
            right = (a: 5, b: 10);
            (int a, int b)? nullableTuple = right;
            Console.WriteLine(left == nullableTuple); // Also true

            // lifted conversions
            left = (a: 5, b: 10);
            (int? a, int? b) nullableMembers = (5, 10);
            Console.WriteLine(left == nullableMembers); // Also true

            // converted type of left is (long, long)
            (long a, long b) longTuple = (5, 10);
            Console.WriteLine(left == longTuple); // Also true

            // comparisons performed on (long, long) tuples
            (long a, int b) longFirst = (5, 10);
            (int a, long b) longSecond = (5, 10);
            Console.WriteLine(longFirst == longSecond); // Also true


            (int a, string b) pair = (1, "Hello");
            (int z, string y) another = (1, "Hello");
            Console.WriteLine(pair == another); // true. Member names don't participate.
            Console.WriteLine(pair == (z: 1, y: "Hello")); // warning: literal contains different member names


            Console.WriteLine("Nested tuples");
            (int, (int, int)) nestedTuple = (1, (2, 3));
            Console.WriteLine(nestedTuple == (1, (2, 3)));


            Console.WriteLine("End EqualityAndTuples");
            Console.WriteLine("===========================================");
        }

        public static void AssignmentsAndTuples()
        {
            Console.WriteLine("AssignmentsAndTuples");

            // The 'arity' and 'shape' of all these tuples are compatible.
            // The only difference is the field names being used.
            var unnamed = (42, "The meaning of life");
            var anonymous = (16, "a perfect square");
            var named = (Answer: 42, Message: "The meaning of life");
            var differentNamed = (SecretConstant: 42, Label: "The meaning of life");

            unnamed = named;

            named = unnamed;
            // 'named' still has fields that can be referred to
            // as 'answer', and 'message':
            Console.WriteLine($"{named.Answer}, {named.Message}");

            // unnamed to unnamed:
            anonymous = unnamed;

            // named tuples.
            named = differentNamed;
            // The field names are not assigned. 'named' still has
            // fields that can be referred to as 'answer' and 'message':
            Console.WriteLine($"{named.Answer}, {named.Message}");

            // With implicit conversions:
            // int can be implicitly converted to long
            (long, string) conversion = named;

            //// Does not compile.
            //// CS0029: Cannot assign Tuple(int,int,int) to Tuple(int, string)
            //var differentShape = (1, 2, 3);
            //named = differentShape;

            Console.WriteLine("End AssignmentsAndTuples");
            Console.WriteLine("===========================================");

        }

        public static void TuplesAsMethodReturnValues()
        {
            Console.WriteLine("TuplesAsMethodReturnValues");

            IEnumerable<double> sequence = new List<double>() {2, 3, 4, 5};
            var result = StandardDeviation(sequence);
            Console.WriteLine("standard deviation result " + result);

            result = StandardDeviationUsingTuple(sequence);
            Console.WriteLine("standard deviation using tuple  result " + result);

            result = StandardDeviationUsingTupleRefactored(sequence);
            Console.WriteLine("standard deviation using tuple  result " + result);

            Console.WriteLine("End TuplesAsMethodReturnValues");
            Console.WriteLine("===========================================");

        }

        public static double StandardDeviation(IEnumerable<double> sequence)
        {
            // Step 1: Compute the Mean:
            var mean = sequence.Average();

            // Step 2: Compute the square of the differences between each number
            // and the mean:
            var squaredMeanDifferences = from n in sequence
                select (n - mean) * (n - mean);
            // Step 3: Find the mean of those squared differences:
            var meanOfSquaredDifferences = squaredMeanDifferences.Average();

            // Step 4: Standard Deviation is the square root of that mean:
            var standardDeviation = Math.Sqrt(meanOfSquaredDifferences);
            return standardDeviation;

            /* OR
             *
double sum = 0;
    double sumOfSquares = 0;
    double count = 0;

    foreach (var item in sequence)
    {
        count++;
        sum += item;
        sumOfSquares += item * item;
    }

    var variance = sumOfSquares - sum * sum / count;
    return Math.Sqrt(variance / count);
    */
        }

        public static double StandardDeviationUsingTuple(IEnumerable<double> sequence)
        {
            var computation = (Count: 0, Sum: 0.0, SumOfSquares: 0.0);

            foreach (var item in sequence)
            {
                computation.Count++;
                computation.Sum += item;
                computation.SumOfSquares += item * item;
            }

            var variance = computation.SumOfSquares - computation.Sum * computation.Sum / computation.Count;
            return Math.Sqrt(variance / computation.Count);
        }


        public static double StandardDeviationUsingTupleRefactored(IEnumerable<double> sequence)
        {
            var computation = ComputeSumAndSumOfSquares(sequence);

            var variance = computation.SumOfSquares - computation.Sum * computation.Sum / computation.Count;
            return Math.Sqrt(variance / computation.Count);
        }

        private static (int Count, double Sum, double SumOfSquares) ComputeSumAndSumOfSquares(IEnumerable<double> sequence)
        {
            double sum = 0;
            double sumOfSquares = 0;
            int count = 0;

            foreach (var item in sequence)
            {
                count++;
                sum += item;
                sumOfSquares += item * item;
            }

            return (count, sum, sumOfSquares);
        }
    }
}
