using System;
using System.Reflection.Emit;

namespace SecondDynamicILDemo
{
    class Program
    {
        // this is the reference method we want to recreate using IL
        static int Calculate(int a, int b, int c)
        {
            var result = a * b;
            return result - c;
        }
        static void Main(string[] args)
        {
            var myMethod = new DynamicMethod(
                "DynamicCalculate",
                typeof(int),
                new[] { typeof(int), typeof(int), typeof(int) },
                typeof(Program).Module);
            
            var ilGenerator = myMethod.GetILGenerator();
            ilGenerator.DeclareLocal(typeof(int)); // declare local variable to hold intermidate result
            ilGenerator.Emit(OpCodes.Ldarg_0); // push first arg to eval stack (a)
            ilGenerator.Emit(OpCodes.Ldarg_1); // push second arg to eval stack (b)
            ilGenerator.Emit(OpCodes.Mul); // pop the two, multiply and push result
            ilGenerator.Emit(OpCodes.Stloc_0); // pop the stack and store value in first local var (result)
            ilGenerator.Emit(OpCodes.Ldloc_0); // push value of first local var to stack
            ilGenerator.Emit(OpCodes.Ldarg_2); // push third arg to stack
            ilGenerator.Emit(OpCodes.Sub); // pop the two, subtract, and push result to stack
            ilGenerator.Emit(OpCodes.Ret); // return

            var calculate = (Func<int, int, int, int>) myMethod.CreateDelegate(typeof(Func<int, int, int, int>));
            var result = calculate(5, 2, 7);
            Console.WriteLine("Result using dynamic method {0}", result);

            var refResult = Calculate(5, 2, 7);
            Console.WriteLine("Reference result {0}", refResult);
        }
    }
}
