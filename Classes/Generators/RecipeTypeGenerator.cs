using rscconventer.Classes.Interfaces;
using rscconventer.Classes.Yaml;
using rscconventer.JavaGenerator;
using rscconventer.JavaGenerator.Actions;
using rscconventer.JavaGenerator.Bukkit;
using rscconventer.JavaGenerator.Interfaces;
using rscconventer.JavaGenerator.Slimefun;
using rscconventer.JavaGenerator.Values;
using YamlDotNet.RepresentationModel;

namespace rscconventer.Classes.Generators;

public class RecipeTypeGenerator : IClassGenerator
{
    public IList<ClassDefinition>? OnGenerate(BuildSession session)
    {
        YamlStream stream = [];
        stream.Load(new StringReader(File.ReadAllText(Path.Combine(session.Directory.FullName, "recipe_types.yml"))));
        YamlMappingNode yaml = (YamlMappingNode)stream.Documents[0].RootNode;
        ClassDefinition generated = new($"me.ddggdd135.{session.Name}.items", $"{char.ToUpper(session.Name[0])}{session.Name[1..]}RecipeTypes");
        if (yaml is not YamlMappingNode mappingNode) return null;
        foreach (KeyValuePair<YamlNode, YamlNode> pair in mappingNode)
        {
            YamlNode key = pair.Key;
            if (key is not YamlScalarNode scalarNode) continue;
            string? stringKey = scalarNode.Value;
            if (stringKey == null) continue;

            YamlNode value = pair.Value;

            IValue itemStack = mappingNode.ReadItem(stringKey, session.Directory);
            IValue namespacedKey = new NewInstanceAction(NamespacedKeyClass.Class, new StringValue(session.Name), new StringValue(stringKey.ToLower()));
            IValue recipeType = new NewInstanceAction(RecipeTypeClass.Class, namespacedKey, itemStack);
            FieldDefinition fieldDefinition = new(RecipeTypeClass.Class, stringKey.ToUpper())
            {
                IsStatic = true,
                DefaultValue = recipeType
            };
            generated.FieldList.Add(fieldDefinition);
        }

        return [generated];
    }
}
