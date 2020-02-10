.assembly extern mscorlib {}
.assembly ClassDemo {}

.method public static void main(string[])
{
    .entrypoint
    .maxstack 10
    .locals init (string name)

    // name = new Person(args[0]).get_Name()
    ldarg.0
    ldc.i4.0
    ldelem.ref
    newobj instance void ClassDemo.Person::.ctor(string)
    call instance string ClassDemo.Person::get_Name()
    stloc.0

    ldstr "My name is {0}"
    ldloc.0
    call void [mscorlib]System.Console::WriteLine(string, object)

    ret
}

// public class Person
// {
//     private string name;

//     public Person(string name)
//     {
//         this.name = name;
//     }

//     public string Name { get { return name; } }
// }

.class public ClassDemo.Person
{
    .field private string name
    .method public hidebysig specialname instance string get_Name() cil managed
    {
        ldarg.0
        ldfld string ClassDemo.Person::name
        ret
    }

    .method public hidebysig specialname rtspecialname instance void .ctor(string) cil managed
    {
        ldarg.0
        ldarg.1
        stfld string ClassDemo.Person::name
        ret
    }

    .property instance string Name()
    {
        .get instance string ClassDemo.Person::get_Name()
    }
}