using rscconventer.Classes.Utils;
using rscconventer.JavaGenerator;
using rscconventer.JavaGenerator.Interfaces;
using rscconventer.JavaGenerator.Slimefun;
using rscconventer.JavaGenerator.Values;
using YamlDotNet.Core.Tokens;
using YamlDotNet.RepresentationModel;

namespace rscconventer.Classes.Yaml;

public static class ItemGroupReader
{
    public static IValue ReadItemGroup(this YamlNode yaml, ClassDefinition itemGroupClass)
    {
        string? itemGroupId = yaml.GetString("item_group") ?? throw new ArgumentException("item_group不能为空");
        RawValue itemGroup = new($"{itemGroupClass.Name}.{itemGroupId.ToUpper()}");
        itemGroup.ImportList.Import(itemGroupClass);

        return itemGroup;
    }
}
