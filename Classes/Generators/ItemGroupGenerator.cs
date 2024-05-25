using rscconventer.Classes.Interfaces;
using rscconventer.Classes.Utils;
using rscconventer.Classes.Yaml;
using rscconventer.JavaGenerator;
using rscconventer.JavaGenerator.Bukkit;
using rscconventer.JavaGenerator.GuguSlimefunLib.Items;
using rscconventer.JavaGenerator.GuguSlimefunLib.Utils;
using rscconventer.JavaGenerator.Interfaces;
using rscconventer.JavaGenerator.Slimefun;
using rscconventer.JavaGenerator.System;
using System.IO;
using YamlDotNet.RepresentationModel;

namespace rscconventer.Classes.Generators;

public class ItemGroupGenerator : IGenerator
{
    public YamlNode Yaml { get; set; }
    public ClassDefinition? OnGenerate(BuildSession session)
    {
        ClassDefinition generated = new("me.ddggdd135." + session.Name + ".items", session.Name + "ItemGroups");
        if (Yaml is not YamlMappingNode mappingNode) return null;
        foreach (KeyValuePair<YamlNode, YamlNode> pair in mappingNode) {
            YamlNode key = pair.Key;
            if (key is not YamlScalarNode scalarNode) continue;
            string? stringKey = scalarNode.Value;
            if (stringKey == null) continue;

            YamlNode value = pair.Value;

            IValue itemStack = value.ReadItem("item", session.Directory);
            string type = value.GetString("type", "");
            IValue namespacedKey = new NewInstanceAction(NamespacedKeyClass.Class, new StringValue("RykenSlimefunCustomizer"), new StringValue(stringKey));

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
                        IValue parentKey = new NewInstanceAction(NamespacedKeyClass.Class, new StringValue("RykenSlimefunCustomizer"), new StringValue(parent));
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
                        IValue parentKey = new NewInstanceAction(NamespacedKeyClass.Class, new StringValue("RykenSlimefunCustomizer"), new StringValue(parent));
                        IList<string>? actions = value.GetStringList("actions");
                        actions ??= [];

                        if (!mappingNode.Contains(parent.ToLower()))
                            itemGroup = ItemGroupUtilsClass.Class.Invoke(ItemGroupUtilsClass.CreateButton, parentKey, itemStack, new NumberValue<int>(tier), new MultipleValue(actions));
                        else
                            itemGroup = new NewInstanceAction(ItemGroupButtonClass.Class, namespacedKey, new RawValue(parent.ToUpper()), itemStack, new NumberValue<int>(tier), new MultipleValue(actions));
                        itemGroupType= ItemGroupButtonClass.Class;
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

        return generated;
    }
    public ItemGroupGenerator(YamlNode yaml)
    {
        Yaml = yaml;
    }
}
