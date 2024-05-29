namespace rscconventer.JavaGenerator.GuguSlimefunLib.Script;

public static class JavaScriptEvalClass
{
    public static ClassDefinition Class { get; }

    static JavaScriptEvalClass()
    {
        Class = new("me.ddggdd135.guguslimefunlib.script", "JavaScriptEval")
        {
            NeedGenerate = false
        };
    }
}
