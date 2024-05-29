using rscconventer.Classes.Interfaces;
using rscconventer.Classes.Yaml;
using rscconventer.JavaGenerator;
using rscconventer.JavaGenerator.Actions;
using rscconventer.JavaGenerator.Interfaces;
using rscconventer.JavaGenerator.Slimefun;
using rscconventer.JavaGenerator.Values;
using YamlDotNet.RepresentationModel;

namespace rscconventer.Classes.Generators;

public class ItemsGenerator : IClassGenerator
{
    public IList<ClassDefinition>? OnGenerate(BuildSession session)
    {
        YamlStream stream = [];
        stream.Load(new StringReader(File.ReadAllText(Path.Combine(session.Directory.FullName, "items.yml"))));
        YamlMappingNode items = (YamlMappingNode)stream.Documents[0].RootNode;
        stream.Load(new StringReader(File.ReadAllText(Path.Combine(session.Directory.FullName, "foods.yml"))));
        YamlMappingNode foods = (YamlMappingNode)stream.Documents[0].RootNode;

        ClassDefinition generated = new($"me.ddggdd135.{session.Name}.items", $"{char.ToUpper(session.Name[0])}{session.Name[1..]}Items");
        MethodDefinition onSetup = new("onSetup")
        {
            ParameterTypes = [SlimefunAddonClass.Class],
            IsStatic = true
        };
        generated.Methods.Add(onSetup);

        if (items is not YamlMappingNode itemsMappingNode) return null;
        foreach (KeyValuePair<YamlNode, YamlNode> pair in itemsMappingNode)
        {
            YamlNode key = pair.Key;
            if (key is not YamlScalarNode scalarNode) continue;
            string? stringKey = scalarNode.Value;
            if (stringKey == null) continue;

            YamlNode value = pair.Value;

            IValue itemStack = value.ReadItem("item", session.Directory, generated);

            IValue slimefunItemStack = new NewInstanceAction(SlimefunItemStackClass.Class, new StringValue(stringKey.ToUpper()), itemStack);
            FieldDefinition slimefunItemStackField = new(SlimefunItemStackClass.Class, stringKey.ToUpper(), slimefunItemStack)
            {
                IsStatic = true
            };
            generated.FieldList.Add(slimefunItemStackField);
        }

        if (foods is not YamlMappingNode foodsMappingNode) return null;
        foreach (KeyValuePair<YamlNode, YamlNode> pair in foodsMappingNode)
        {
            YamlNode key = pair.Key;
            if (key is not YamlScalarNode scalarNode) continue;
            string? stringKey = scalarNode.Value;
            if (stringKey == null) continue;

            YamlNode value = pair.Value;

            IValue itemStack = value.ReadItem("item", session.Directory, generated);

            IValue slimefunItemStack = new NewInstanceAction(SlimefunItemStackClass.Class, new StringValue(stringKey.ToUpper()), itemStack);
            FieldDefinition slimefunItemStackField = new(SlimefunItemStackClass.Class, stringKey.ToUpper(), slimefunItemStack)
            {
                IsStatic = true
            };
            generated.FieldList.Add(slimefunItemStackField);
        }

        return [generated];
    }
}
