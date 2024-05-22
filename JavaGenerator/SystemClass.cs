namespace rscconventer.JavaGenerator;

public static class SystemClass
{
    public static ClassDefinition StringClass { get; }
    static SystemClass()
    {
        StringClass = new("java.lang", "String")
        {
            NeedGenerate = false
        };
    }
}
