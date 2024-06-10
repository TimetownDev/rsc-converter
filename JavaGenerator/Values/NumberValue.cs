using rsc_converter.JavaGenerator.Interfaces;
using System.Numerics;

namespace rsc_converter.JavaGenerator.Values;

public class NumberValue<T> : IValue where T : INumber<T>
{
    public T Value { get; set; }
    public string BuildContent(ClassDefinition classDefinition) => Value.ToString();
    public NumberValue(T value)
    {
        Value = value;
    }
}
