namespace JavaGenerator.GuguSlimefunLib.Script;

public static class ScriptEvalClass
{
    public static ClassDefinition Class { get; }

    static ScriptEvalClass()
    {
        Class = new("me.ddggdd135.guguslimefunlib.script", "ScriptEval")
        {
            NeedGenerate = false,
            IsAbstract = true
        };
    }
}
