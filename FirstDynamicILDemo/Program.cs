using System;
using System.Reflection.Emit;

namespace FirstDynamicILDemo
{
    class Program
    {
        static double Divide(int a, int b)
        {
            return a / b;
        }

        delegate double DivideDelegate(int a, int b);
        static void Main(string[] args)
        {
            // dynamically create a method that performs division, using IL
            var myMethod = new DynamicMethod(
                "DynamicDivide",  // method name (appears in stack traces)
                typeof(double), // return type
                new [] { typeof(int), typeof(int) }, // param types
                typeof(Program).Module); // module with which method is associated
            
            var ilGenerator = myMethod.GetILGenerator();
            ilGenerator.Emit(OpCodes.Ldarg_0); // push first arg to evaluation stack
            ilGenerator.Emit(OpCodes.Ldarg_1); // push second arg to eval stack
            ilGenerator.Emit(OpCodes.Div); // perform division and push result to stack
            ilGenerator.Emit(OpCodes.Conv_R8); // convert top of stack to double (cause that's what the method should return)
            ilGenerator.Emit(OpCodes.Ret); // return from current method with value at top of stack as return value

            // invoke method
            var result = myMethod.Invoke(null, new object[] { 10, 2 });
            Console.WriteLine("Result using DynamicMethod.Invoke {0}", result);

            // create delegate to execute the method
            var divide = (DivideDelegate) myMethod.CreateDelegate(typeof(DivideDelegate));
            var result2 = divide(6, 2);
            Console.WriteLine("Result using delegate {0}", result2);

            // statically defined method
            var result3 = Divide(9, 3);
            Console.WriteLine("Result using normal statically defined method {0}", result3);
        }
    }
}
