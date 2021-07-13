
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpPlayGround.Operators
{
    using System;

    public struct Point
    {
        public Point(byte tag, double x, double y) => (Tag, X, Y) = (tag, x, y);

        public byte Tag { get; }
        public double X { get; }
        public double Y { get; }
    }

    public class SizeOfOperator
    {
        public static void MainTest()
        {
            //Console.WriteLine(sizeof(byte));  // output: 1
            //Console.WriteLine(sizeof(double));  // output: 8
            ////Console.WriteLine(sizeof(string)); // compilation error
            //Console.WriteLine(sizeof(char));

            DisplaySizeOf<sbyte>();  
            DisplaySizeOf<byte>();  
            DisplaySizeOf<short>();  
            DisplaySizeOf<ushort>();  
            DisplaySizeOf<int>();  
            DisplaySizeOf<uint>();  
            DisplaySizeOf<long>();  
            DisplaySizeOf<ulong>();  
            DisplaySizeOf<char>();  
            DisplaySizeOf<float>();  
            DisplaySizeOf<double>();  
            DisplaySizeOf<decimal>();  
            DisplaySizeOf<bool>();  

            DisplaySizeOf<Point>();  // output: Size of Point is 24
            DisplaySizeOf<decimal>();  // output: Size of System.Decimal is 16

            unsafe
            {
                Console.WriteLine(sizeof(Point*));  // output: 8
            }
        }

        static unsafe void DisplaySizeOf<T>() where T : unmanaged
        {
            Console.WriteLine($"Size of {typeof(T)} is {sizeof(T)}");
        }
    }
}
