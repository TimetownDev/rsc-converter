namespace rsc_converter.JavaGenerator.Exceptions;

public class NoSuchMethodException : Exception
{
    public string? MethodName { get; private set; }
    public MethodDefinition? Method { get; private set; }
    public NoSuchMethodException(string message, string methodName) : base(message)
    {
        MethodName = methodName;
    }
    public NoSuchMethodException(string methodName) : base()
    {
        MethodName = methodName;
    }
    public NoSuchMethodException(MethodDefinition method) : base()
    {
        Method = method;
        MethodName = Method.Name;
    }
}
