using rscconventer.Classes.Interfaces;
using rscconventer.Classes.Utils;
using rscconventer.Classes.Yaml;
using rscconventer.JavaGenerator;
using rscconventer.JavaGenerator.Actions;
using rscconventer.JavaGenerator.Attributes;
using rscconventer.JavaGenerator.Bukkit;
using rscconventer.JavaGenerator.GuguSlimefunLib.Items;
using rscconventer.JavaGenerator.GuguSlimefunLib.Script;
using rscconventer.JavaGenerator.Interfaces;
using rscconventer.JavaGenerator.Slimefun;
using rscconventer.JavaGenerator.System;
using rscconventer.JavaGenerator.Values;
using System.Diagnostics.Metrics;
using YamlDotNet.RepresentationModel;

namespace rscconventer.Classes.Generators;

public class GEOResourceGenerator : IClassGenerator
{
    public IList<ClassDefinition>? OnGenerate(BuildSession session)
    {
        ClassDefinition generated = new($"me.ddggdd135.{session.Name}.geos", $"{char.ToUpper(session.Name[0])}{session.Name[1..]}GEOResources");

        YamlStream stream = [];
        stream.Load(new StringReader(File.ReadAllText(Path.Combine(session.Directory.FullName, "geo_resources.yml"))));
        YamlMappingNode items = (YamlMappingNode)stream.Documents[0].RootNode;

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

        if (items is not YamlMappingNode mappingNode) return null;
        foreach (KeyValuePair<YamlNode, YamlNode> pair in mappingNode)
        {
            YamlNode key = pair.Key;
            if (key is not YamlScalarNode scalarNode) continue;
            string? stringKey = scalarNode.Value;
            if (stringKey == null) continue;

            YamlNode value = pair.Value;

            ClassDefinition itemClass = new($"me.ddggdd135.{session.Name}.geos.implementations", $"{stringKey.ToUpper()}GEOImplementation")
            {
                Super = GuguSlimefunItemClass.Class,
                Interfaces = [NotPlaceableClass.Class, GEOResourceClass.Class]
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

            bool hasScript = value.Contains("script");
            if (hasScript)
            {
                string scriptFileName = value.GetString("script")!;
                string script = File.ReadAllText(Path.Combine(session.Directory.FullName, "scripts", scriptFileName + ".js"));
                preRegister.Block.Actions.Add(new ThisValue().Invoke(GuguSlimefunItemClass.SetEval, new NewInstanceAction(JavaScriptEvalClass.Class, new StringValue(script))));
            }

            itemClass.Methods.Add(preRegister);

            
            string geoName = value.GetString("geo_name")!;
            itemClass.FieldList.Add(new(SystemClass.StringClass, "name", new StringValue(geoName))
            {
                Access = AccessAttribute.Private
            });
            MethodDefinition getName = new("getName")
            {
                ReturnType = SystemClass.StringClass
            };
            getName.Block.Actions.Add(new ReturnAction(new RawValue("name")));
            itemClass.Methods.Add(getName);

            IValue namespacedKey = new NewInstanceAction(NamespacedKeyClass.Class, new StringValue(session.Name), new StringValue(stringKey));
            itemClass.FieldList.Add(new(NamespacedKeyClass.Class, "namespacedKey", namespacedKey)
            {
                Access = AccessAttribute.Private
            });
            MethodDefinition getKey = new("getKey")
            {
                ReturnType = NamespacedKeyClass.Class
            };
            getKey.Block.Actions.Add(new ReturnAction(new RawValue("namespacedKey")));
            itemClass.Methods.Add(getKey);

            bool isObtainableFromGEOMiner = value.GetBoolean("isObtainableFromGEOMiner", false);
            itemClass.FieldList.Add(new(new RawClassDefinition("boolean"), "isObtainableFromGEOMiner", new BoolValue(isObtainableFromGEOMiner))
            {
                Access = AccessAttribute.Private
            });
            MethodDefinition getIsObtainableFromGEOMiner = new("isObtainableFromGEOMiner")
            {
                ReturnType = new RawClassDefinition("boolean")
            };
            getIsObtainableFromGEOMiner.Block.Actions.Add(new ReturnAction(new RawValue("isObtainableFromGEOMiner")));
            itemClass.Methods.Add(getIsObtainableFromGEOMiner);

            int maxDeviation = value.GetInt("max-deviation")!;
            itemClass.FieldList.Add(new(new RawClassDefinition("int"), "maxDeviation", new NumberValue<int>(maxDeviation))
            {
                Access = AccessAttribute.Private
            });
            MethodDefinition getMaxDeviation = new("getMaxDeviation")
            {
                ReturnType = new RawClassDefinition("int")
            };
            getMaxDeviation.Block.Actions.Add(new ReturnAction(new RawValue("maxDeviation")));
            itemClass.Methods.Add(getMaxDeviation);

            
            itemClass.FieldList.Add(new(SupplyInfoClass.Class, "supplyInfo", new NewInstanceAction(SupplyInfoClass.Class))
            {
                Access = AccessAttribute.Private
            });
            if (value.Contains("supply"))
            {
                YamlNode supply = value["supply"];
                if (supply.Contains("normal"))
                {
                    YamlNode normal = supply["normal"];
                    if (normal is YamlScalarNode normalNode)
                    {
                        preRegister.Block.Actions.Add(new RawValue("supplyInfo").Invoke(SupplyInfoClass.SetDefaultNormalSupply, new NumberValue<int>(int.Parse(normalNode.Value!))));
                    }
                    else if (normal is YamlMappingNode normalMappingNode)
                    {
                        foreach (KeyValuePair<YamlNode, YamlNode> biomePair in normalMappingNode)
                        {
                            if (biomePair.Key is not YamlScalarNode biomeScalarNode) continue;
                            string? biomeKey = biomeScalarNode.Value;
                            if (biomeKey == null) continue;

                            if (biomeKey.ToLower() == "others")
                            {
                                preRegister.Block.Actions.Add(new RawValue("supplyInfo").Invoke(SupplyInfoClass.SetDefaultNormalSupply, new NumberValue<int>(normalMappingNode.GetInt(biomeKey))));
                                continue;
                            }
                            RawValue biome = new($"{BiomeClass.Class.Name}.{biomeKey.ToUpper()}");
                            biome.ImportList.Import(BiomeClass.Class);

                            preRegister.Block.Actions.Add(new RawValue("supplyInfo").Invoke(SupplyInfoClass.AddNormalSupply, biome, new NumberValue<int>(normalMappingNode.GetInt(biomeKey))));
                        }
                    }
                }
                if (supply.Contains("nether"))
                {
                    YamlNode nether = supply["nether"];
                    if (nether is YamlScalarNode netherNode)
                    {
                        preRegister.Block.Actions.Add(new RawValue("supplyInfo").Invoke(SupplyInfoClass.SetDefaultNormalSupply, new NumberValue<int>(int.Parse(netherNode.Value!))));
                    }
                    else if (nether is YamlMappingNode netherMappingNode)
                    {
                        foreach (KeyValuePair<YamlNode, YamlNode> biomePair in netherMappingNode)
                        {
                            if (biomePair.Key is not YamlScalarNode biomeScalarNode) continue;
                            string? biomeKey = biomeScalarNode.Value;
                            if (biomeKey == null) continue;

                            if (biomeKey.ToLower() == "others")
                            {
                                preRegister.Block.Actions.Add(new RawValue("supplyInfo").Invoke(SupplyInfoClass.SetDefaultNetherSupply, new NumberValue<int>(netherMappingNode.GetInt(biomeKey))));
                                continue;
                            }
                            RawValue biome = new($"{BiomeClass.Class.Name}.{biomeKey.ToUpper()}");
                            biome.ImportList.Import(BiomeClass.Class);

                            preRegister.Block.Actions.Add(new RawValue("supplyInfo").Invoke(SupplyInfoClass.AddNetherSupply, biome, new NumberValue<int>(netherMappingNode.GetInt(biomeKey))));
                        }
                    }
                }
                if (supply.Contains("the_end"))
                {
                    YamlNode theEnd = supply["the_end"];
                    if (theEnd is YamlScalarNode theEndNode)
                    {
                        preRegister.Block.Actions.Add(new RawValue("supplyInfo").Invoke(SupplyInfoClass.SetDefaultNormalSupply, new NumberValue<int>(int.Parse(theEndNode.Value!))));
                    }
                    else if (theEnd is YamlMappingNode theEndMappingNode)
                    {
                        foreach (KeyValuePair<YamlNode, YamlNode> biomePair in theEndMappingNode)
                        {
                            if (biomePair.Key is not YamlScalarNode biomeScalarNode) continue;
                            string? biomeKey = biomeScalarNode.Value;
                            if (biomeKey == null) continue;

                            if (biomeKey.ToLower() == "others")
                            {
                                preRegister.Block.Actions.Add(new RawValue("supplyInfo").Invoke(SupplyInfoClass.SetDefaultTheEndSupply, new NumberValue<int>(theEndMappingNode.GetInt(biomeKey))));
                                continue;
                            }
                            RawValue biome = new($"{BiomeClass.Class.Name}.{biomeKey.ToUpper()}");
                            biome.ImportList.Import(BiomeClass.Class);

                            preRegister.Block.Actions.Add(new RawValue("supplyInfo").Invoke(SupplyInfoClass.AddTheEndSupply, biome, new NumberValue<int>(theEndMappingNode.GetInt(biomeKey))));
                        }
                    }
                }
            }
            MethodDefinition getDefaultSupply = new("getDefaultSupply")
            {
                ReturnType = new RawClassDefinition("int"),
                ParameterTypes = [WorldEnvironmentClass.Class, BiomeClass.Class]
            };
            getDefaultSupply.Block.Actions.Add(new ReturnAction(new RawValue("supplyInfo").Invoke(SupplyInfoClass.GetDefaultSupply, new ParameterValue(0), new ParameterValue(1))));
            itemClass.Methods.Add(getDefaultSupply);

            itemClasses.Add(itemClass);
            onSetup.Block.Actions.Add(new NewInstanceAction(itemClass, itemGroup, slimefunItemStackValue, recipeType, new MultipleValue(recipe)).Invoke(GuguSlimefunItemClass.Register, new ParameterValue(0)));
        }

        itemClasses.Add(generated);

        return itemClasses;
    }
}
