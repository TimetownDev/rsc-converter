using rscconventer.JavaGenerator.Interfaces;

namespace rscconventer.JavaGenerator;

public class RawAction : IAction
{
    public string ActionText { get; set; } = string.Empty;
    public IList<string> Imports { get; set; } = [];

    public string ToString(ClassDefinition classDefinition)
    {
        throw new NotImplementedException();
    }
}
