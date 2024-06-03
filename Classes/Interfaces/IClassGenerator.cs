using JavaGenerator;

namespace Classes.Interfaces;

public interface IClassGenerator
{
    IList<ClassDefinition>? OnGenerate(BuildSession session);
}
