namespace rsc_converter.JavaGenerator.System;

public static class SystemClass
{
    public static ClassDefinition StringClass { get; }
    public static ClassDefinition IntegerClass { get; }
    public static ClassDefinition BooleanClass { get; }
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
        BooleanClass = new("java.lang", "Boolean")
        {
            NeedGenerate = false
        };
    }
}
