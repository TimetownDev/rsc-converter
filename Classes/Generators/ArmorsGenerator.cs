using rscconventer.Classes.Interfaces;
using rscconventer.Classes.Utils;
using rscconventer.Classes.Yaml;
using rscconventer.JavaGenerator;
using rscconventer.JavaGenerator.Actions;
using rscconventer.JavaGenerator.Bukkit;
using rscconventer.JavaGenerator.GuguSlimefunLib.Items;
using rscconventer.JavaGenerator.GuguSlimefunLib.Utils;
using rscconventer.JavaGenerator.Interfaces;
using rscconventer.JavaGenerator.Slimefun;
using rscconventer.JavaGenerator.Values;
using YamlDotNet.RepresentationModel;

namespace rscconventer.Classes.Generators;

public class ArmorsGenerator : IClassGenerator
{
    public static IList<string> ArmorTypes { get; } = ["helmet", "chestplate", "leggings", "boots"];
    public IList<ClassDefinition>? OnGenerate(BuildSession session)
    {
        ClassDefinition generated = new($"me.ddggdd135.{session.Name}.armors", $"{char.ToUpper(session.Name[0])}{session.Name[1..]}Armors");

        YamlStream stream = [];
        stream.Load(new StringReader(File.ReadAllText(Path.Combine(session.Directory.FullName, "armors.yml"))));
        YamlNode armors = stream.Documents[0].RootNode;

        IList<ClassDefinition> itemClasses = [];
        ClassDefinition itemGroupClass = session.GetClassDefinition($"{char.ToUpper(session.Name[0])}{session.Name[1..]}ItemGroups")!;
        ClassDefinition recipeTypeClass = session.GetClassDefinition($"{char.ToUpper(session.Name[0])}{session.Name[1..]}RecipeTypes")!;
        ClassDefinition itemsClass = session.GetClassDefinition($"{char.ToUpper(session.Name[0])}{session.Name[1..]}Items")!;

        MethodDefinition onSetup = new("onSetup")
        {
            ParameterTypes = [SlimefunAddonClass.Class],
            IsStatic = true
        };
        generated.Methods.Add(onSetup);

        if (armors is not YamlMappingNode armorsMappingNode) return null;
        foreach (KeyValuePair<YamlNode, YamlNode> pair in armorsMappingNode)
        {
            YamlNode key = pair.Key;
            if (key is not YamlScalarNode scalarNode) continue;
            string? armorKey = scalarNode.Value;
            if (armorKey == null) continue;

            YamlNode value = pair.Value;

            bool fullSet = value.GetBoolean("fullSet", false);
            FieldDefinition fullSetField = new(new RawClassDefinition("boolean"), "fullSet", new BoolValue(fullSet));
            MethodDefinition isFullSetRequired = new("isFullSetRequired")
            {
                ReturnType = new RawClassDefinition("boolean")
            };
            isFullSetRequired.Block.Actions.Add(new ReturnAction(new RawValue("fullSet")));
            IValue itemGroup = value.ReadItemGroup(itemGroupClass);
            IList<string>? protectionTypes = value.GetStringList("protectionTypes");
            protectionTypes ??= [];

            IList<IValue> protectionTypeValues = [];
            foreach (string protectionType in protectionTypes)
            {
                protectionTypeValues.Add(new ProtectionTypeValue(protectionType.ToUpper()));
            }
            FieldDefinition protectionTypeField = new(new ArrayClassDefinition(ProtectionTypeClass.Class), "protectionType", new ArrayValue(ProtectionTypeClass.Class, protectionTypeValues));
            MethodDefinition getProtectionTypes = new("getProtectionTypes")
            {
                ReturnType = new ArrayClassDefinition(ProtectionTypeClass.Class)
            };
            getProtectionTypes.Block.Actions.Add(new ReturnAction(new RawValue("protectionType")));

            IValue namespacedKey = new NewInstanceAction(NamespacedKeyClass.Class, new StringValue(session.Name), new StringValue(armorKey.ToLower()));
            FieldDefinition armorSetIdField = new(NamespacedKeyClass.Class, "armorSetId", namespacedKey);
            MethodDefinition getArmorSetId = new("getArmorSetId")
            {
                ReturnType = NamespacedKeyClass.Class
            };
            getArmorSetId.Block.Actions.Add(new ReturnAction(new RawValue("armorSetId")));

            foreach (string armorType in ArmorTypes)
            {
                YamlNode armorPiece = value[armorType];
                if (armorPiece == null) continue;
                string pieceId = armorPiece.GetString("id", $"{armorKey.ToUpper()}_{armorType.ToUpper()}");

                ClassDefinition itemClass = new($"me.ddggdd135.{session.Name}.armors.implementations", $"{pieceId.ToUpper()}ItemImplementation")
                {
                    Super = GuguSlimefunArmorPieceClass.Class,
                    Interfaces = [ProtectiveArmorClass.Class]
                };
                CtorMethodDefinition ctor = new(ItemGroupClass.Class, SlimefunItemStackClass.Class, RecipeTypeClass.Class, new ArrayClassDefinition(ItemStackClass.Class), new ArrayClassDefinition(PotionEffectClass.Class));
                ctor.Block.Actions.Add(new SuperInvokeAction(new ParameterValue(0), new ParameterValue(1), new ParameterValue(2), new ParameterValue(3), new ParameterValue(4)));
                itemClass.Ctors.Add(ctor);

                IList<string>? effects = armorPiece.GetStringList("potion_effects");
                effects ??= [];

                itemClass.FieldList.Add(fullSetField);
                itemClass.Methods.Add(isFullSetRequired);
                itemClass.FieldList.Add(protectionTypeField);
                itemClass.Methods.Add(getProtectionTypes);
                itemClass.FieldList.Add(armorSetIdField);
                itemClass.Methods.Add(getArmorSetId);

                MethodDefinition preRegister = new("preRegister");
                RawValue slimefunItemStackValue = new($"{itemsClass.Name}.{pieceId.ToUpper()}");
                slimefunItemStackValue.ImportList.Import(itemsClass);

                IValue recipeType = armorPiece.ReadRecipeType(recipeTypeClass);
                IValue[] recipe = armorPiece.ReadRecipe(session.Directory, itemsClass);

                itemClass.Methods.Add(preRegister);

                itemClasses.Add(itemClass);
                onSetup.Block.Actions.Add(new NewInstanceAction(itemClass, itemGroup, slimefunItemStackValue, recipeType, new ArrayValue(ItemStackClass.Class, recipe), PotionUtilsClass.Class.Invoke(PotionUtilsClass.ParseAll, new MultipleValue(effects))).Invoke(GuguSlimefunItemClass.Register, new ParameterValue(0)));
            }
        }
        itemClasses.Add(generated);

        return itemClasses;
    }
}
