.assembly extern mscorlib {}
.assembly FirstILSample {}

.method static public void main() cil managed
{
    .entrypoint
    .maxstack 1

    ldstr "Hello, World"
    call void [mscorlib]System.Console::WriteLine(class System.String)

    ret
}