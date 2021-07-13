using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpPlayGround
{
    public struct MutablePoint
    {
        public int X;
        public int Y;

        public MutablePoint(int x, int y) => (X, Y) = (x, y);

        public override string ToString() => $"({X}, {Y})";
    }

    public class ValueTypeTest
    {
        public static void MainTest()
        {
            var p1 = new MutablePoint(1, 2);
            var p2 = p1;
            p2.Y = 200;
            Console.WriteLine($"{nameof(p1)} after {nameof(p2)} is modified: {p1}");
            Console.WriteLine($"{nameof(p2)}: {p2}");

            MutateAndDisplay(p2);
            Console.WriteLine($"{nameof(p2)} after passing to a method: {p2}");


            // value type containing a data member of a reference type
            // only the reference to the instance of the reference type is copied

            var n1 = new TaggedInteger(0);
            n1.AddTag("A");
            Console.WriteLine(n1);  // output: 0 [A]

            var n2 = n1;
            n2.Number = 7;
            n2.AddTag("B");

            Console.WriteLine(n1);  // output: 0 [A, B]
            Console.WriteLine(n2);  // output: 7 [A, B]
        }

        private static void MutateAndDisplay(MutablePoint p)
        {
            p.X = 100;
            Console.WriteLine($"Point mutated in a method: {p}");
        }
// Expected output:
// p1 after p2 is modified: (1, 2)
// p2: (1, 200)
// Point mutated in a method: (100, 200)
// p2 after passing to a method: (1, 200)



    }


    public struct TaggedInteger
    {
        public int Number;
        private List<string> tags;

        public TaggedInteger(int n)
        {
            Number = n;
            tags = new List<string>();
        }

        public void AddTag(string tag) => tags.Add(tag);

        public override string ToString() => $"{Number} [{string.Join(", ", tags)}]";
    }
}
