using rscconventer.JavaGenerator.Actions;
using System.Runtime.CompilerServices;

namespace rscconventer.JavaGenerator.Interfaces;

public interface IValue
{
    string BuildContent(ClassDefinition classDefinition);
}

public static class IValueExtension
{
    public static ObjectInvokeAction Invoke(this IValue value, MethodDefinition method, params IValue[] parameters)
    {
        return new ObjectInvokeAction(value, method, parameters);
    }
}
