using System;
using System.Reflection.Emit;

namespace CallingDynamicMethods
{
    class Program
    {
        static void Main(string[] args)
        {
            var squareMethod = new DynamicMethod("Square",
                typeof(int),
                new[] { typeof(int) },
                typeof(Program).Module);
            var sqIl = squareMethod.GetILGenerator();
            // return arg0 * arg0
            sqIl.Emit(OpCodes.Ldarg_0);
            sqIl.Emit(OpCodes.Ldarg_0);
            sqIl.Emit(OpCodes.Mul);
            sqIl.Emit(OpCodes.Ret);

            var prodSquareMethod = new DynamicMethod("SquareOfProduct",
                typeof(int),
                new[] { typeof(int), typeof(int) },
                typeof(Program).Module);
            var pdSqIl = prodSquareMethod.GetILGenerator();
            // return Square(arg0 * arg1)
            pdSqIl.Emit(OpCodes.Ldarg_0);
            pdSqIl.Emit(OpCodes.Ldarg_1);
            pdSqIl.Emit(OpCodes.Mul);
            pdSqIl.Emit(OpCodes.Call, squareMethod);
            pdSqIl.Emit(OpCodes.Ret);

            var squareOfProduct = prodSquareMethod.CreateDelegate(
                typeof(Func<int, int, int>)) as Func<int, int, int>;
            
            var result = squareOfProduct(4, 2);
            Console.WriteLine("Result {0}", result);
        }
    }
}
