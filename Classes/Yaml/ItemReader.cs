using rscconventer.Classes.Utils;
using rscconventer.JavaGenerator;
using rscconventer.JavaGenerator.Actions;
using rscconventer.JavaGenerator.Bukkit;
using rscconventer.JavaGenerator.GuguSlimefunLib.Items;
using rscconventer.JavaGenerator.Interfaces;
using rscconventer.JavaGenerator.Values;
using System.Text.RegularExpressions;
using YamlDotNet.RepresentationModel;

namespace rscconventer.Classes.Yaml;

public static partial class ItemReader
{
    private readonly static Regex SkullHashRegex = SkullHashRegexGenerated();
    public static IValue ReadItem(this YamlNode yaml, DirectoryInfo directory, ClassDefinition? itemClass = null)
    {
        string type = yaml.GetString("material_type", "mc");

        if (!type.Equals("none", StringComparison.OrdinalIgnoreCase) && !yaml.Contains("material"))
        {
            throw new ArgumentException("请先设置一个材料！");
        }

        string? material = yaml.GetString("material", "");

        if (material.StartsWith("ey"))
            type = "skull";
        else if (material.StartsWith("http"))
            type = "skull_url";
        else if (SkullHashRegex.Match(material).Success)
            type = "skull_hash";

        IList<string>? lore = yaml.GetStringList("lore");
        lore ??= [];

        string name = yaml.GetString("name", "");
        bool glow = yaml.GetBoolean("glow", false);
        bool hasEnchantment = yaml.Contains("enchantments") && yaml.IsList("enchantments");
        int modelId = yaml.GetInt("modelId");
        int amount = yaml.GetInt("amount");
        if (amount <= 0) amount = 1;
        IValue itemStack;

        switch (type.ToLower())
        {
            case "none":
                itemStack = new NewInstanceAction(AdvancedCustomItemStackClass.Class, MaterialValue.Air, new NumberValue<int>(1));
                break;
            case "skull_hash":
                itemStack = AdvancedCustomItemStackClass.Class.Invoke(AdvancedCustomItemStackClass.FromHashCode, new StringValue(material), new StringValue(name), new MultipleValue(lore));
                break;
            case "skull_base64":
                itemStack = AdvancedCustomItemStackClass.Class.Invoke(AdvancedCustomItemStackClass.FromBase64, new StringValue(material), new StringValue(name), new MultipleValue(lore));
                break;
            case "skull_url":
                itemStack = AdvancedCustomItemStackClass.Class.Invoke(AdvancedCustomItemStackClass.FromURL, new StringValue(material), new StringValue(name), new MultipleValue(lore));
                break;
            case "slimefun":
                if (itemClass == null || itemClass.FieldList.FindField(material.ToUpper()) == null)
                {
                    if (name != string.Empty)
                        if (lore.Any())
                            itemStack = AdvancedCustomItemStackClass.Class.Invoke(AdvancedCustomItemStackClass.FromSlimefunItem, new StringValue(material), new StringValue(name), new MultipleValue(lore));
                        else
                            itemStack = AdvancedCustomItemStackClass.Class.Invoke(AdvancedCustomItemStackClass.FromSlimefunItem, new StringValue(material), new StringValue(name));
                    else
                        if (lore.Any())
                        itemStack = AdvancedCustomItemStackClass.Class.Invoke(AdvancedCustomItemStackClass.FromSlimefunItem, new StringValue(material), new MultipleValue(lore));
                    else
                        itemStack = AdvancedCustomItemStackClass.Class.Invoke(AdvancedCustomItemStackClass.FromSlimefunItem, new StringValue(material));
                }
                else
                {
                    RawValue slimefunItemStack = new($"{itemClass.Name}.{material.ToUpper()}");
                    slimefunItemStack.ImportList.Import(itemClass);
                    if (name != string.Empty)
                    {
                        if (lore.Any())
                        {
                            itemStack = new NewInstanceAction(AdvancedCustomItemStackClass.Class, slimefunItemStack, new StringValue(name), new MultipleValue(lore));
                        }
                        else
                        {
                            itemStack = new NewInstanceAction(AdvancedCustomItemStackClass.Class, slimefunItemStack, new StringValue(name));
                        }
                    }
                    else
                    {
                        if (lore.Any())
                        {
                            itemStack = AdvancedCustomItemStackClass.Class.Invoke(AdvancedCustomItemStackClass.FromLore, slimefunItemStack, new MultipleValue(lore));
                        }
                        else
                        {
                            itemStack = new NewInstanceAction(AdvancedCustomItemStackClass.Class, slimefunItemStack);
                        }
                    }
                }
                break;
            case "saveditem":
                FileInfo yamlFile = new(Path.Combine(directory.FullName, "saveditems", material + ".yml"));
                if (!yamlFile.Exists) throw new ArgumentException("未找到保存的物品");
                string yamlData = File.ReadAllText(yamlFile.FullName);
                itemStack = AdvancedCustomItemStackClass.Class.Invoke(AdvancedCustomItemStackClass.Parse, new StringValue(yamlData));
                break;
            default:
                itemStack = new NewInstanceAction(AdvancedCustomItemStackClass.Class, new MaterialValue(material), new StringValue(name), new MultipleValue(lore));
                break;
        }

        if (glow) itemStack = itemStack.Invoke(AdvancedCustomItemStackClass.DoGlow);
        if (modelId > 0) itemStack = itemStack.Invoke(AdvancedCustomItemStackClass.SetCustomModelData, new NumberValue<int>(modelId));
        if (amount > 64 || amount < -1) throw new ArgumentException("物品数量必须在64到0之间");
        if (amount != 1)
            itemStack = itemStack.Invoke(AdvancedCustomItemStackClass.AsQuantity, new NumberValue<int>(amount));

        return itemStack;
    }
    public static IValue ReadItem(this YamlNode yaml, string key, DirectoryInfo directory, ClassDefinition? itemClass = null)
    {
        yaml = yaml[key];

        return ReadItem(yaml, directory, itemClass);
    }

    [GeneratedRegex("^[A-Za-z0-9]{64,}$")]
    private static partial Regex SkullHashRegexGenerated();
}
