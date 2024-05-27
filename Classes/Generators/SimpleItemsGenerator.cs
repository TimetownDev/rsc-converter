using rscconventer.Classes.Interfaces;
using rscconventer.Classes.Utils;
using rscconventer.Classes.Yaml;
using rscconventer.JavaGenerator;
using rscconventer.JavaGenerator.Actions;
using rscconventer.JavaGenerator.Attributes;
using rscconventer.JavaGenerator.Bukkit;
using rscconventer.JavaGenerator.GuguSlimefunLib.Items;
using rscconventer.JavaGenerator.Interfaces;
using rscconventer.JavaGenerator.Slimefun;
using rscconventer.JavaGenerator.Values;
using YamlDotNet.RepresentationModel;

namespace rscconventer.Classes.Generators;

public class SimpleItemsGenerator : IClassGenerator
{
    public IList<ClassDefinition>? OnGenerate(BuildSession session)
    {
        YamlStream stream = [];
        stream.Load(new StringReader(File.ReadAllText(Path.Combine(session.Directory.FullName, "items.yml"))));
        YamlMappingNode yaml = (YamlMappingNode)stream.Documents[0].RootNode;
        ClassDefinition generated = new($"me.ddggdd135.{session.Name}.items", $"{char.ToUpper(session.Name[0])}{session.Name[1..]}Items");
        MethodDefinition onSetup = new("onSetup")
        {
            ParameterTypes = [SlimefunAddonClass.Class]
        };
        generated.Methods.Add(onSetup);
        IList<ClassDefinition> itemClasses = [];
        ClassDefinition itemGroupClass = session.GetClassDefinition($"{char.ToUpper(session.Name[0])}{session.Name[1..]}ItemGroups")!;
        ClassDefinition recipeTypeClass = session.GetClassDefinition($"{char.ToUpper(session.Name[0])}{session.Name[1..]}RecipeTypes")!;
        if (yaml is not YamlMappingNode mappingNode) return null;
        foreach (KeyValuePair<YamlNode, YamlNode> pair in mappingNode)
        {
            YamlNode key = pair.Key;
            if (key is not YamlScalarNode scalarNode) continue;
            string? stringKey = scalarNode.Value;
            if (stringKey == null) continue;

            YamlNode value = pair.Value;

            ClassDefinition itemClass = new($"me.ddggdd135.{session.Name}.items.implementations", $"{stringKey.ToUpper()}ItemImplementation")
            {
                Super = GuguSlimefunItemClass.Class
            };
            itemClass.Ctors.Add(new(ItemGroupClass.Class, SlimefunItemStackClass.Class, RecipeTypeClass.Class, new ParamsClassDefinition(ItemStackClass.Class)));

            IValue itemStack = value.ReadItem("item", session.Directory);

            IValue slimefunItemStack = new NewInstanceAction(SlimefunItemStackClass.Class, new StringValue(stringKey.ToUpper()), itemStack);
            FieldDefinition slimefunItemStackField = new(SlimefunItemStackClass.Class, stringKey.ToUpper(), slimefunItemStack);
            generated.FieldList.Add(slimefunItemStackField);

            string? itemGroupId = value.GetString("item_group");
            if (itemGroupId == null) throw new ArgumentException("item_group不能为空");
            RawValue itemGroup = new($"{char.ToUpper(session.Name[0])}{session.Name[1..]}ItemGroups.{itemGroupId.ToUpper()}");
            itemGroup.ImportList.Import(itemGroupClass);
            string recipeTypeId = value.GetString("recipe_type", "NULL").ToUpper();
            if (recipeTypeId == "NULL")
                recipeTypeId = "NONE";
            RawValue recipeType;
            FieldDefinition? recipeTypeField = recipeTypeClass.FieldList.FindField(recipeTypeId);
            if (recipeTypeField == null)
            {
                recipeType = new RawValue($"{RecipeTypeClass.Class.FullName}.{recipeTypeField}");
                recipeType.ImportList.Import(RecipeTypeClass.Class);
            }
            else
            {
                recipeType = new RawValue($"{recipeTypeClass.FullName}.{recipeTypeField}");
                recipeType.ImportList.Import(recipeTypeClass);
            }
            bool placeable = value.GetBoolean("placeable", true);
            if (!placeable)
                itemClass.Interfaces.Add(NotPlaceableClass.Class);
            bool isRechargeable = value.Contains("energy_capacity");
            if (isRechargeable)
            {
                int energyCapacity = value.GetInt("energy_capacity");
                if (energyCapacity > 0)
                {
                    itemClass.Interfaces.Add(RechargeableClass.Class);
                    itemClass.FieldList.Add(new(new RawClassDefinition("float"), "capacity", new NumberValue<int>(energyCapacity))
                    {
                        Access = AccessAttribute.Private
                    });
                    MethodDefinition getMaxItemCharge = new("getMaxItemCharge")
                    {
                        ReturnType = new RawClassDefinition("float")
                    };
                    getMaxItemCharge.Block.Actions.Add(new ReturnAction(new RawValue("capacity")));
                }
            }
            bool isRadioactive = value.Contains("radiation");
            if (isRadioactive)
            {
                string radiationId = value.GetString("radiation")!;
                RawValue radioactivity = new($"{RadioactivityClass.Class.FullName}.{radiationId.ToUpper()}");
                radioactivity.ImportList.Import(RadioactivityClass.Class);
                itemClass.Interfaces.Add(RadioactivityClass.Class);
                itemClass.FieldList.Add(new(RadioactivityClass.Class, "radioactivity", radioactivity)
                {
                    Access = AccessAttribute.Private
                });
                MethodDefinition getMaxItemCharge = new("getRadioactivity")
                {
                    ReturnType = RadioactivityClass.Class
                };
                getMaxItemCharge.Block.Actions.Add(new ReturnAction(new RawValue("radioactivity")));
            }
            bool isSoulbound = value.GetBoolean("soulbound", false);
            if (isSoulbound)
            {
                itemClass.Interfaces.Add(SoulboundClass.Class);
            }
            //TODO 添加剩下的项目


        }
        IList<ClassDefinition> result = [];
        foreach (ClassDefinition classDefinition in itemClasses)
        {
            result.Add(classDefinition);
        }
        result.Add(generated);

        return result;
    }
}
