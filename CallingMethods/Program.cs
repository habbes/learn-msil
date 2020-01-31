using System;
using System.Reflection.Emit;

namespace CallingMethods
{
    class Program
    {
        static void Main(string[] args)
        {
            Print(42);

            var myMethod = new DynamicMethod("DynamicPrint",
                typeof(void),
                null,
                typeof(Program).Module);

            var il = myMethod.GetILGenerator();
            il.Emit(OpCodes.Ldc_I4, 42);
            il.Emit(OpCodes.Call, typeof(Program).GetMethod("Print"));
            il.Emit(OpCodes.Ret);

            var myDelegate = (Action) myMethod.CreateDelegate(typeof(Action));
            myDelegate();
        } 

        public static void Print(int i)
        {
            Console.WriteLine("Printing {0}", i);
        }
    }
}
