using rsc_converter.Classes.Interfaces;
using rsc_converter.Classes.Utils;
using rsc_converter.Classes.Yaml;
using rsc_converter.JavaGenerator;
using rsc_converter.JavaGenerator.Actions;
using rsc_converter.JavaGenerator.Attributes;
using rsc_converter.JavaGenerator.Bukkit;
using rsc_converter.JavaGenerator.GuguSlimefunLib.Items;
using rsc_converter.JavaGenerator.GuguSlimefunLib.Script;
using rsc_converter.JavaGenerator.Interfaces;
using rsc_converter.JavaGenerator.Slimefun;
using rsc_converter.JavaGenerator.Values;
using YamlDotNet.RepresentationModel;

namespace rsc_converter.Classes.Generators;

public class SimpleItemsGenerator : IClassGenerator
{
    public IList<ClassDefinition>? OnGenerate(BuildSession session)
    {
        YamlStream stream = [];
        stream.Load(new StringReader(File.ReadAllText(Path.Combine(session.Directory.FullName, "items.yml"))));
        YamlMappingNode items = (YamlMappingNode)stream.Documents[0].RootNode;

        IList<ClassDefinition> itemClasses = [];
        ClassDefinition itemGroupClass = session.GetClassDefinition($"{char.ToUpper(session.Name[0])}{session.Name[1..]}ItemGroups")!;
        ClassDefinition recipeTypeClass = session.GetClassDefinition($"{char.ToUpper(session.Name[0])}{session.Name[1..]}RecipeTypes")!;
        ClassDefinition itemsClass = session.GetClassDefinition($"{char.ToUpper(session.Name[0])}{session.Name[1..]}Items")!;

        MethodDefinition onSetup = itemsClass.FindMethod("onSetup")!;

        if (items is not YamlMappingNode mappingNode) return null;
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
            CtorMethodDefinition ctor = new(ItemGroupClass.Class, SlimefunItemStackClass.Class, RecipeTypeClass.Class, new ParamsClassDefinition(ItemStackClass.Class));
            ctor.Block.Actions.Add(new SuperInvokeAction(new ParameterValue(0), new ParameterValue(1), new ParameterValue(2), new ParameterValue(3)));
            itemClass.Ctors.Add(ctor);

            MethodDefinition preRegister = new("preRegister");
            RawValue slimefunItemStackValue = new($"{itemsClass.Name}.{stringKey.ToUpper()}");
            slimefunItemStackValue.ImportList.Import(itemsClass);

            IValue itemGroup = value.ReadItemGroup(itemGroupClass);
            IValue recipeType = value.ReadRecipeType(recipeTypeClass);
            IValue[] recipe = value.ReadRecipe(session.Directory, itemsClass);

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
                        ReturnType = new RawClassDefinition("float"),
                        ParameterTypes = [ItemStackClass.Class]
                    };
                    getMaxItemCharge.Block.Actions.Add(new ReturnAction(new RawValue("capacity")));
                    itemClass.Methods.Add(getMaxItemCharge);
                }
            }

            bool isRadioactive = value.Contains("radiation");
            if (isRadioactive)
            {
                string radiationId = value.GetString("radiation")!;
                RawValue radioactivity = new($"{RadioactivityClass.Class.FullName}.{radiationId.ToUpper()}");
                radioactivity.ImportList.Import(RadioactivityClass.Class);
                itemClass.Interfaces.Add(RadioactiveClass.Class);
                itemClass.FieldList.Add(new(RadioactivityClass.Class, "radioactivity", radioactivity)
                {
                    Access = AccessAttribute.Private
                });
                MethodDefinition getRadioactivity = new("getRadioactivity")
                {
                    ReturnType = RadioactivityClass.Class
                };
                getRadioactivity.Block.Actions.Add(new ReturnAction(new RawValue("radioactivity")));
                itemClass.Methods.Add(getRadioactivity);
            }

            bool isSoulbound = value.GetBoolean("soulbound", false);
            if (isSoulbound)
            {
                itemClass.Interfaces.Add(SoulboundClass.Class);
            }

            bool isRainbow = value.Contains("rainbow");
            if (isRainbow)
            {
                string rainbowType = value.GetString("rainbow")!.ToUpper();
                if (rainbowType == "CUSTOM")
                {
                    IList<string>? rainbowMaterials = value.GetStringList("rainbow_materials") ?? throw new ArgumentException("彩虹材料不能为空");
                    MaterialValue[] materials = rainbowMaterials.Select(x => new MaterialValue(x)).ToArray();
                    preRegister.Block.Actions.Add(new ThisValue().Invoke(GuguSlimefunItemClass.SetRainbowType, new NewInstanceAction(RainbowTypeClass.Class, materials)));
                }
                else
                {
                    RawValue rainbowTypeValue = new($"{RainbowTypeClass.Class.Name}.{rainbowType}");
                    rainbowTypeValue.ImportList.Import(RainbowTypeClass.Class);
                    preRegister.Block.Actions.Add(new ThisValue().Invoke(GuguSlimefunItemClass.SetRainbowType, rainbowTypeValue));
                }
            }

            bool antiWither = value.GetBoolean("anti_wither", false);
            preRegister.Block.Actions.Add(new ThisValue().Invoke(GuguSlimefunItemClass.SetAntiWither, new BoolValue(antiWither)));

            int piglinTradeChance = value.GetInt("piglin_trade_chance", 0);
            preRegister.Block.Actions.Add(new ThisValue().Invoke(GuguSlimefunItemClass.SetPiglinTradeChance, new NumberValue<int>(piglinTradeChance)));

            bool canDropFrom = value.Contains("drop_from");
            if (canDropFrom)
            {
                string dropFrom = value.GetString("drop_from")!.ToUpper();
                int dropChance = value.GetInt("drop_chance", 100);
                int dropAmount = value.GetInt("drop_amount", 1);
                preRegister.Block.Actions.Add(new ThisValue().Invoke(GuguSlimefunItemClass.SetDropFrom, new StringValue(dropFrom)));
                preRegister.Block.Actions.Add(new ThisValue().Invoke(GuguSlimefunItemClass.SetDropChance, new NumberValue<int>(dropChance)));
                preRegister.Block.Actions.Add(new ThisValue().Invoke(GuguSlimefunItemClass.SetDropAmount, new NumberValue<int>(dropAmount)));
            }

            bool isVanilla = value.GetBoolean("vanilla", false);
            if (isVanilla)
            {
                preRegister.Block.Actions.Add(new ThisValue().Invoke(GuguSlimefunItemClass.SetVanilla, new BoolValue(isVanilla)));
            }

            bool isHidden = value.GetBoolean("hidden", false);
            if (isHidden)
            {
                preRegister.Block.Actions.Add(new ThisValue().Invoke(GuguSlimefunItemClass.SetHidden, new BoolValue(isHidden)));
            }

            bool hasScript = value.Contains("script");
            if (hasScript)
            {
                string scriptFileName = value.GetString("script")!;
                string script = File.ReadAllText(Path.Combine(session.Directory.FullName, "scripts", scriptFileName + ".js"));
                preRegister.Block.Actions.Add(new ThisValue().Invoke(GuguSlimefunItemClass.SetEval, new NewInstanceAction(JavaScriptEvalClass.Class, new StringValue(script))));
            }

            itemClass.Methods.Add(preRegister);

            itemClasses.Add(itemClass);
            onSetup.Block.Actions.Add(new NewInstanceAction(itemClass, itemGroup, slimefunItemStackValue, recipeType, new MultipleValue(recipe)).Invoke(GuguSlimefunItemClass.Register, new ParameterValue(0)));
        }

        return itemClasses;
    }
}
