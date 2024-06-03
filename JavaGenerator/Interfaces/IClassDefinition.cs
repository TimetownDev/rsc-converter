namespace JavaGenerator.Interfaces;

public interface IClassDefinition
{
    public string Name { get; }
    public string Namespace { get; }
    public string FullName { get; }
    public bool NeedImport => true;
    public string OnImport(ClassDefinition classDefinition);
}
