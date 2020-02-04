using System;
using System.Reflection.Emit;

namespace SwitchStatement
{
    class Program
    {
        static void Main(string[] args)
        {
            
            var method = new DynamicMethod(
                "GetResultDynamic",
                typeof(int), new[] { typeof(int), typeof(int), typeof(int) },
                typeof(Program).Module
            );
            var il = method.GetILGenerator();
            il.DeclareLocal(typeof(int));
            il.DeclareLocal(typeof(int));
            il.DeclareLocal(typeof(int));

            Label[] jumpTable = new[] {
                il.DefineLabel(), // case 0
                il.DefineLabel(), // case 1
                il.DefineLabel(), // case 2
                il.DefineLabel(), // case 3
            };

            // switch (operation)
            // the top of the stack should be the index of the label
            // the switch statement will jump to the label at that index in the jump table
            il.Emit(OpCodes.Ldarg_2);
            il.Emit(OpCodes.Switch, jumpTable);

            // default case, i.e. arg did not match the index of any label in the jump table
            // return 0
            il.Emit(OpCodes.Ldc_I4_0);
            il.Emit(OpCodes.Ret);

            // case 0
            il.MarkLabel(jumpTable[0]);
            // return a + b
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Add);
            il.Emit(OpCodes.Ret);

            // case 1
            il.MarkLabel(jumpTable[1]);
            // return a - b
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Sub);
            il.Emit(OpCodes.Ret);

            // case 2
            il.MarkLabel(jumpTable[2]);
            // return a * b
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Mul);
            il.Emit(OpCodes.Ret);

            // case 3
            il.MarkLabel(jumpTable[3]);
            // return a / b
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Div);
            il.Emit(OpCodes.Ret);
            

            var dynGetResult = (Func<int, int, int, int>) method.CreateDelegate(typeof(Func<int, int, int, int>));

            Console.WriteLine("sum {0}", dynGetResult(20, 10, 0));
            Console.WriteLine("diff {0}", dynGetResult(20, 10, 1));
            Console.WriteLine("product {0}", dynGetResult(20, 10, 2));
            Console.WriteLine("quot {0}", dynGetResult(20, 10, 3));
            Console.WriteLine("zero {0}", dynGetResult(20, 10, -1));
           
            Console.WriteLine("Ref sum {0}", GetResult(20, 10, 0));
            Console.WriteLine("Ref diff {0}", GetResult(20, 10, 1));
            Console.WriteLine("Ref product {0}", GetResult(20, 10, 2));
            Console.WriteLine("Ref quot {0}", GetResult(20, 10, 3));
            Console.WriteLine("Ref zero (default) {0}", GetResult(20, 10, -1));
        }

        static int GetResult(int a, int b, int operation)
        {
            switch (operation)
            {
                case 0:
                    return a + b;
                case 1:
                    return a - b;
                case 2:
                    return a * b;
                case 3:
                    return a / b;
                default:
                    return 0;
            }
        }
    }
    
}
