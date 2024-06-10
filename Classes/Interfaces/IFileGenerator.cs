namespace rsc_converter.Classes.Interfaces;

public interface IFileGenerator
{
    IList<FileData>? OnGenerate(BuildSession session);
}
