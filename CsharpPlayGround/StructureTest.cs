using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpPlayGround
{
    public struct Coords
    {
        public Coords(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; }
        public double Y { get; }

        public override string ToString() => $"({X}, {Y})";
    }

    public class StructureTest
    {

        public static void MainTest()
        {

        }
    }
}
