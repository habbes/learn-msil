using System;
using System.Reflection.Emit;

namespace CreateInstance
{
    class Program
    {
        static void Main(string[] args)
        {
            var method = new DynamicMethod(
                "CreatePerson",
                typeof(Person),
                Type.EmptyTypes,
                typeof(Program).Module
            );

            var il = method.GetILGenerator();
            // return new Person("Habbes")
            il.Emit(OpCodes.Ldstr, "Habbes");
            il.Emit(OpCodes.Newobj, typeof(Person).GetConstructor(new[] { typeof(string) }));
            il.Emit(OpCodes.Ret);

            var createPerson = (Func<Person>) method.CreateDelegate(typeof(Func<Person>));

            dynamic person = createPerson();
            Console.WriteLine(person.Name);
        }
    }

    public class Person
    {
        public string Name { get; set; }

        public Person(string name)
        {
            Name = name;
        }
    }
}
