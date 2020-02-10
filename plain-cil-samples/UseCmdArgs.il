.assembly extern mscorlib {}
.assembly UsingCmdArgs {}

.method static public void main(string[]) cil managed
{
    .entrypoint
    .maxstack 3

    ldstr "Hi, my name is {0}"

    ldarg.0
    ldc.i4.0
    // we use ldelem.ref because we are loading a reference type (class instance) from the array
    ldelem.ref

    call void [mscorlib]System.Console::WriteLine(string, object)

    ret
}