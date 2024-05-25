namespace rscconventer.JavaGenerator.System;

public static class MonthClass
{
    public static ClassDefinition Class { get; }
    public static MethodDefinition Of { get; }
    static MonthClass()
    {
        Class = new("java.time", "Month")
        {
            NeedGenerate = false
        };
        Of = new("of")
        {
            IsStatic = true,
            ReturnType = MonthClass.Class,
            ParameterTypes = [SystemClass.IntegerClass]
        };
        Class.Methods.Add(Of);
    }
}
