.assembly extern mscorlib {}
.assembly ConvertAndBox {}

.method static public void main(string[]) cil managed
{
    .entrypoint
    .maxstack 10
    .locals init (int32 V_0)

    // Convert.ToInt32(args[0])
    ldarg.0
    ldc.i4.0
    ldelem.ref
    call int32 [mscorlib]System.Convert::ToInt32(string)
    // local0 = Convert.ToInt32(args[0]) * 10
    ldc.i4 10
    mul
    stloc.0

    ldstr "Result is: {0}"
    ldloc.0
    // box the integer value type because the second argument expects an object (reference type)
    box int32
    call void [mscorlib]System.Console::WriteLine(string, object)

    ret
}