using System;
using System.Reflection;
using System.Reflection.Emit;

namespace CreateAType
{
    class Program
    {
        static void Main(string[] args)
        {
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(
                new AssemblyName("Demo"), AssemblyBuilderAccess.Run);
            var module = assemblyBuilder.DefineDynamicModule("PersonModule");

            var typeBuilder = module.DefineType("Person", TypeAttributes.Public);
            var nameField = typeBuilder.DefineField("name", typeof(string), FieldAttributes.Private);

            var ctor = typeBuilder.DefineConstructor(MethodAttributes.Public,
                CallingConventions.Standard, new[] { typeof(string) });
            
            // constructor implementation, simply sets the .name field to the value of the constructor argument
            var ctorIl = ctor.GetILGenerator();
            ctorIl.Emit(OpCodes.Ldarg_0); // first arg is always the object instance
            ctorIl.Emit(OpCodes.Ldarg_1);
            // this.name = name
            ctorIl.Emit(OpCodes.Stfld, nameField);
            ctorIl.Emit(OpCodes.Ret);

            var nameProperty = typeBuilder.DefineProperty("Name",
                PropertyAttributes.HasDefault, typeof(string), null);

            var namePropertyGetter = typeBuilder.DefineMethod("get_Name",
                MethodAttributes.Public |
                MethodAttributes.SpecialName |
                MethodAttributes.HideBySig,
                typeof(string),
                Type.EmptyTypes);
            nameProperty.SetGetMethod(namePropertyGetter);
            
            var getterIl = namePropertyGetter.GetILGenerator();
            getterIl.Emit(OpCodes.Ldarg_0);
            getterIl.Emit(OpCodes.Ldfld, nameField);
            getterIl.Emit(OpCodes.Ret);

            var personType =  typeBuilder.CreateType();
            dynamic instance = Activator.CreateInstance(personType, "Habbes");
            Console.WriteLine(instance.Name);
        }
    }
}
