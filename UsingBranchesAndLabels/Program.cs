using System;
using System.Reflection.Emit;

namespace UsingBranchesAndLabels
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = SomeClass.Calculate(1000);

            var calc = new DynamicMethod(
                "DynamicCalculate",
                typeof(int),
                new [] { typeof(int) },
                typeof(Program).Module
            );

            var ilGenerator = calc.GetILGenerator();
            var loopStartLabel = ilGenerator.DefineLabel();
            var loopEndLabel = ilGenerator.DefineLabel();

            // int result = 0
            ilGenerator.DeclareLocal(typeof(int));
            ilGenerator.Emit(OpCodes.Ldc_I4_0);
            ilGenerator.Emit(OpCodes.Stloc_0);
            // int i = 0
            ilGenerator.DeclareLocal(typeof(int));
            ilGenerator.Emit(OpCodes.Ldc_I4_0);
            ilGenerator.Emit(OpCodes.Stloc_1);

            // start of loop header
            ilGenerator.MarkLabel(loopStartLabel);
            // if !(i < 10) jump end of loop
            ilGenerator.Emit(OpCodes.Ldloc_1);
            ilGenerator.Emit(OpCodes.Ldc_I4, 10);
            ilGenerator.Emit(OpCodes.Bge, loopEndLabel);

            // loop body
            // result += i * x;
            ilGenerator.Emit(OpCodes.Ldloc_1);
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Mul);
            ilGenerator.Emit(OpCodes.Ldloc_0);
            ilGenerator.Emit(OpCodes.Add);
            ilGenerator.Emit(OpCodes.Stloc_0);

            // i++
            ilGenerator.Emit(OpCodes.Ldloc_1);
            ilGenerator.Emit(OpCodes.Ldc_I4_1);
            ilGenerator.Emit(OpCodes.Add);
            ilGenerator.Emit(OpCodes.Stloc_1);
            // jump to top of loop
            ilGenerator.Emit(OpCodes.Br, loopStartLabel);

            // end loop
            ilGenerator.MarkLabel(loopEndLabel);
            // return result
            ilGenerator.Emit(OpCodes.Ldloc_0);
            ilGenerator.Emit(OpCodes.Ret);
            

            var func = (Func<int, int>)calc.CreateDelegate(typeof(Func<int, int>));
            var dynamicResult = func(1000);
            Console.WriteLine("result {0} {1}", result, dynamicResult);
        }
    }
}
