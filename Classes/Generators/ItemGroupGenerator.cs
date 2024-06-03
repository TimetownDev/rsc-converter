using Classes.Interfaces;
using Classes.Utils;
using JavaGenerator;
using JavaGenerator.Actions;
using JavaGenerator.Bukkit;
using JavaGenerator.GuguSlimefunLib.Items;
using JavaGenerator.GuguSlimefunLib.Utils;
using JavaGenerator.Interfaces;
using JavaGenerator.Slimefun;
using JavaGenerator.System;
using JavaGenerator.Values;
using rscconventer.Classes.Yaml;
using YamlDotNet.RepresentationModel;

namespace Classes.Generators;

public class ItemGroupGenerator : IClassGenerator
{
    public IList<ClassDefinition>? OnGenerate(BuildSession session)
    {
        YamlStream stream = [];
        stream.Load(new StringReader(File.ReadAllText(Path.Combine(session.Directory.FullName, "groups.yml"))));
        YamlMappingNode yaml = (YamlMappingNode)stream.Documents[0].RootNode;
        ClassDefinition generated = new($"me.ddggdd135.{session.Name}.items", $"{char.ToUpper(session.Name[0])}{session.Name[1..]}ItemGroups");
        if (yaml is not YamlMappingNode mappingNode) return null;
        foreach (KeyValuePair<YamlNode, YamlNode> pair in mappingNode)
        {
            YamlNode key = pair.Key;
            if (key is not YamlScalarNode scalarNode) continue;
            string? stringKey = scalarNode.Value;
            if (stringKey == null) continue;

            YamlNode value = pair.Value;

            IValue itemStack = value.ReadItem("item", session.Directory);
            string type = value.GetString("type", "");
            IValue namespacedKey = new NewInstanceAction(NamespacedKeyClass.Class, new StringValue(session.Name), new StringValue(stringKey.ToLower()));

            int tier = value.GetInt("tier", 3);
            if (tier <= 0)
            {
                throw new ArgumentException("tier不能小于等于0");
            }

            IValue itemGroup;
            ClassDefinition itemGroupType = ItemGroupClass.Class;
            switch (type)
            {
                case "sub":
                    {
                        string parent = value.GetString("parent", "").ToLower();
                        IValue parentKey = new NewInstanceAction(NamespacedKeyClass.Class, new StringValue(session.Name), new StringValue(parent));
                        if (!mappingNode.Contains(parent.ToLower()))
                            itemGroup = ItemGroupUtilsClass.Class.Invoke(ItemGroupUtilsClass.CreateSubItemGroup, parentKey, namespacedKey, itemStack, new NumberValue<int>(tier));
                        else
                            itemGroup = new NewInstanceAction(SubItemGroupClass.Class, namespacedKey, new RawValue(parent.ToUpper()), itemStack, new NumberValue<int>(tier));
                        itemGroupType = ItemGroupClass.Class;
                    }
                    break;
                case "locked":
                    {
                        IList<string>? parents = value.GetStringList("parents");
                        parents ??= [];
                        IList<IValue> parentNamespacedKeys = [];
                        foreach (string parent in parents)
                        {
                            parentNamespacedKeys.Add(NamespacedKeyClass.Class.Invoke(NamespacedKeyClass.FromString, new StringValue(parent)));
                        }

                        itemGroup = new NewInstanceAction(LockedItemGroupClass.Class, namespacedKey, itemStack, new NumberValue<int>(tier), new MultipleValue(parentNamespacedKeys));
                        itemGroupType = LockedItemGroupClass.Class;
                    }
                    break;
                case "nested":
                case "parent":
                    itemGroup = new NewInstanceAction(AdvancedNestedItemGroupClass.Class, namespacedKey, itemStack, new NumberValue<int>(tier));
                    itemGroupType = AdvancedNestedItemGroupClass.Class;
                    break;
                case "seasonal":
                    IValue month = MonthClass.Class.Invoke(MonthClass.Of, new NumberValue<int>(value.GetInt("month", 1)));
                    itemGroup = new NewInstanceAction(SeasonalItemGroupClass.Class, namespacedKey, month, new NumberValue<int>(tier), itemStack);
                    itemGroupType = SeasonalItemGroupClass.Class;
                    break;
                case "button":
                    {
                        string parent = value.GetString("parent", "").ToLower();
                        IValue parentKey = new NewInstanceAction(NamespacedKeyClass.Class, new StringValue(session.Name), new StringValue(parent));
                        IList<string>? actions = value.GetStringList("actions");
                        actions ??= [];

                        if (!mappingNode.Contains(parent.ToLower()))
                            itemGroup = ItemGroupUtilsClass.Class.Invoke(ItemGroupUtilsClass.CreateButton, parentKey, itemStack, new NumberValue<int>(tier), new MultipleValue(actions));
                        else
                            itemGroup = new NewInstanceAction(ItemGroupButtonClass.Class, namespacedKey, new RawValue(parent.ToUpper()), itemStack, new NumberValue<int>(tier), new MultipleValue(actions));
                        itemGroupType = ItemGroupButtonClass.Class;
                    }
                    break;
                default:
                    itemGroup = new NewInstanceAction(ItemGroupClass.Class, namespacedKey, itemStack, new NumberValue<int>(tier));
                    break;
            }
            FieldDefinition fieldDefinition = new(itemGroupType, stringKey.ToUpper())
            {
                IsStatic = true,
                DefaultValue = itemGroup
            };
            generated.FieldList.Add(fieldDefinition);
        }

        MethodDefinition onSetup = new("onSetup")
        {
            IsStatic = true,
            ParameterTypes = [SlimefunAddonClass.Class]
        };
        ActionBlock actionBlock = onSetup.Block;
        foreach (FieldDefinition field in generated.FieldList)
        {
            actionBlock.Actions.Add(new RawValue(field.Name).Invoke(ItemGroupClass.Register, new ParameterValue(0)));
        }
        generated.Methods.Add(onSetup);

        return [generated];
    }
}
