namespace rscconventer.Classes.Interfaces;

public interface IFileGenerator
{
    IList<FileData>? OnGenerate(BuildSession session);
}
