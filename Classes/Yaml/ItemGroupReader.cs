using Classes.Utils;
using JavaGenerator;
using JavaGenerator.Interfaces;
using JavaGenerator.Values;
using YamlDotNet.RepresentationModel;

namespace Classes.Yaml;

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
