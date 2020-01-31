using System;

namespace SimpleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Calculate(5));
            Console.WriteLine(Factorial(5));
        }

        static int Calculate(int x)
        {
            var result = x;
            do
            {
                result = result + 5;
            } while (result < 15);
            return result;
        }

        static int Factorial(int x)
        {
            if (x <= 1) return x;
            return x * Factorial(x - 1);
        }
    }
}
