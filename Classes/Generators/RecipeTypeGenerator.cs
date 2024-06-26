﻿using rsc_converter.Classes.Interfaces;
using rsc_converter.JavaGenerator;
using rsc_converter.JavaGenerator.Actions;
using rsc_converter.JavaGenerator.Bukkit;
using rsc_converter.JavaGenerator.Interfaces;
using rsc_converter.JavaGenerator.Slimefun;
using rsc_converter.JavaGenerator.Values;
using rscconventer.Classes.Yaml;
using YamlDotNet.RepresentationModel;

namespace rsc_converter.Classes.Generators;

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
