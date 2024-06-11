using rsc_converter.Classes.Interfaces;
using rsc_converter.Classes.Utils;
using rsc_converter.JavaGenerator;
using rsc_converter.JavaGenerator.Actions;
using rsc_converter.JavaGenerator.Bukkit;
using rsc_converter.JavaGenerator.GuguSlimefunLib.Items;
using rsc_converter.JavaGenerator.Interfaces;
using rsc_converter.JavaGenerator.Slimefun;
using rsc_converter.JavaGenerator.Values;
using rscconventer.Classes.Yaml;
using YamlDotNet.RepresentationModel;

namespace rsc_converter.Classes.Generators;

public class ItemsGenerator : IClassGenerator
{
    public static IList<string> ArmorTypes { get; } = ["helmet", "chestplate", "leggings", "boots"];
    public IList<ClassDefinition>? OnGenerate(BuildSession session)
    {
        YamlStream stream = [];
        stream.Load(new StringReader(File.ReadAllText(Path.Combine(session.Directory.FullName, "items.yml"))));
        YamlNode items = stream.Documents[0].RootNode;
        stream.Load(new StringReader(File.ReadAllText(Path.Combine(session.Directory.FullName, "foods.yml"))));
        YamlNode foods = stream.Documents[0].RootNode;
        stream.Load(new StringReader(File.ReadAllText(Path.Combine(session.Directory.FullName, "geo_resources.yml"))));
        YamlNode geos = stream.Documents[0].RootNode;
        stream.Load(new StringReader(File.ReadAllText(Path.Combine(session.Directory.FullName, "mob_drops.yml"))));
        YamlNode mobDrops = stream.Documents[0].RootNode;
        stream.Load(new StringReader(File.ReadAllText(Path.Combine(session.Directory.FullName, "armors.yml"))));
        YamlNode armors = stream.Documents[0].RootNode;
        stream.Load(new StringReader(File.ReadAllText(Path.Combine(session.Directory.FullName, "capacitors.yml"))));
        YamlNode capacitors = stream.Documents[0].RootNode;
        stream.Load(new StringReader(File.ReadAllText(Path.Combine(session.Directory.FullName, "machines.yml"))));
        YamlNode machines = stream.Documents[0].RootNode;

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

        if (geos is not YamlMappingNode geosMappingNode) return null;
        foreach (KeyValuePair<YamlNode, YamlNode> pair in geosMappingNode)
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

        if (mobDrops is not YamlMappingNode mobDropsMappingNode) return null;
        foreach (KeyValuePair<YamlNode, YamlNode> pair in mobDropsMappingNode)
        {
            YamlNode key = pair.Key;
            if (key is not YamlScalarNode scalarNode) continue;
            string? stringKey = scalarNode.Value;
            if (stringKey == null) continue;

            YamlNode value = pair.Value;

            string entityType = value.GetString("entity")!;
            RawValue entity = new($"{EntityTypeClass.Class.Name}.{entityType.ToUpper()}");
            entity.ImportList.Import(EntityTypeClass.Class);
            int chance = value.GetInt("chance")!;
            IValue itemStack = AdvancedCustomItemStackClass.Class.Invoke(AdvancedCustomItemStackClass.FromLore, value.ReadItem("item", session.Directory, generated), AdvancedCustomItemStackClass.Class.Invoke(AdvancedCustomItemStackClass.MakeChanceLore, entity, new NumberValue<int>(chance)));

            IValue slimefunItemStack = new NewInstanceAction(SlimefunItemStackClass.Class, new StringValue(stringKey.ToUpper()), itemStack);
            FieldDefinition slimefunItemStackField = new(SlimefunItemStackClass.Class, stringKey.ToUpper(), slimefunItemStack)
            {
                IsStatic = true
            };
            generated.FieldList.Add(slimefunItemStackField);
        }

        if (armors is not YamlMappingNode armorsMappingNode) return null;
        foreach (KeyValuePair<YamlNode, YamlNode> pair in armorsMappingNode)
        {
            YamlNode key = pair.Key;
            if (key is not YamlScalarNode scalarNode) continue;
            string? armorKey = scalarNode.Value;
            if (armorKey == null) continue;

            YamlNode value = pair.Value;

            foreach (string armorType in ArmorTypes)
            {
                YamlNode armorPiece = value[armorType];
                if (armorPiece == null) continue;
                string pieceId = armorPiece.GetString("id", $"{armorKey.ToUpper()}_{armorType.ToUpper()}");
                IValue itemStack = armorPiece.ReadItem(session.Directory, generated);
                IValue slimefunItemStack = new NewInstanceAction(SlimefunItemStackClass.Class, new StringValue(pieceId.ToUpper()), itemStack);
                FieldDefinition slimefunItemStackField = new(SlimefunItemStackClass.Class, pieceId.ToUpper(), slimefunItemStack)
                {
                    IsStatic = true
                };
                generated.FieldList.Add(slimefunItemStackField);
            }
        }

        if (capacitors is not YamlMappingNode capacitorsMappingNode) return null;
        foreach (KeyValuePair<YamlNode, YamlNode> pair in capacitorsMappingNode)
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

        if (machines is not YamlMappingNode machinesMappingNode) return null;
        foreach (KeyValuePair<YamlNode, YamlNode> pair in machinesMappingNode)
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
