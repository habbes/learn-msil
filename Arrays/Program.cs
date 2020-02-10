using System;
using System.Reflection.Emit;

namespace Arrays
{
    class Program
    {
        static void Main(string[] args)
        {
            var method = new DynamicMethod("MultiplyElementsDynamic",
                typeof(int),
                new[]{ typeof(int[]) },
                typeof(Program).Module);
            
            var il = method.GetILGenerator();
            
            // items[0]
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldc_I4_0);
            il.Emit(OpCodes.Ldelem_I4);
            // items[1]
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldc_I4_1);
            il.Emit(OpCodes.Ldelem_I4);
            // return items[0] * items[1]
            il.Emit(OpCodes.Mul);
            il.Emit(OpCodes.Ret);

            var multElems = method.CreateDelegate(typeof(Func<int[], int>)) as Func<int[], int>;
            Console.WriteLine(multElems(new int[] { 10, 20 }));
            Console.WriteLine(MultiplyElements(new int[] { 10, 20 }));
        }

        static int MultiplyElements(int[] items)
        {
            return items[0] * items[1];
        }
    }
}
