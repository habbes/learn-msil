using System;
using System.Reflection.Emit;

namespace RecursiveMethod
{
    class Program
    {
        static void Main(string[] args)
        {
            var factorialMethod = new DynamicMethod("DynamicFactorial",
                typeof(int),
                new [] { typeof(int) },
                typeof(Program).Module);
            
            var il = factorialMethod.GetILGenerator();
            var methodEnd = il.DefineLabel();

            // push x twice cause after checking (x == 1)
            // x is still used in either the subtraction or return value of the base case
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldc_I4_1);
            il.Emit(OpCodes.Beq, methodEnd);

            // x * Facotrial(x - 1)
            il.Emit(OpCodes.Ldc_I4_1);
            il.Emit(OpCodes.Sub);
            il.Emit(OpCodes.Call, factorialMethod);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Mul);

            il.MarkLabel(methodEnd);
            il.Emit(OpCodes.Ret);

            var factorialDel = factorialMethod.CreateDelegate(
                typeof(Func<int, int>)) as Func<int,int>;
            
            Console.WriteLine("Reference result {0}", Factorial(5));
            Console.WriteLine("Dynamic result {0}", factorialDel(5));
        }

        static int Factorial(int x)
        {
            if (x == 1) return 1;
            return x * Factorial(x - 1);
        }
    }
}
