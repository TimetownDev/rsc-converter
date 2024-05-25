namespace rscconventer.JavaGenerator.System;

public static class SystemClass
{
    public static ClassDefinition StringClass { get; }
    public static ClassDefinition IntegerClass { get; }
    static SystemClass()
    {
        StringClass = new("java.lang", "String")
        {
            NeedGenerate = false
        };
        IntegerClass = new("java.lang", "Integer")
        {
            NeedGenerate = false
        };
    }
}
