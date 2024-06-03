using System.Globalization;
using System.Numerics;
using YamlDotNet.RepresentationModel;

namespace Classes.Utils;

public static class YamlUtils
{
    public static bool Contains(this YamlNode yaml, string key)
    {
        try
        {
            YamlNode node = yaml[key];
            return true;
        }
        catch
        {
            return false;
        }
    }

    #region get
    public static string? GetString(this YamlNode yaml, string key)
    {
        if (!yaml.Contains(key)) return null;
        try
        {
            YamlScalarNode node = (YamlScalarNode)yaml[key];
            return node.Value;
        }
        catch
        {
            return null;
        }
    }

    public static string GetString(this YamlNode yaml, string key, string @default)
    {
        if (!yaml.Contains(key)) return @default;
        try
        {
            YamlScalarNode node = (YamlScalarNode)yaml[key];
            if (node.Value == null) return @default;
            return node.Value;
        }
        catch
        {
            return @default;
        }
    }

    public static IList<string>? GetStringList(this YamlNode yaml, string key)
    {
        if (!yaml.Contains(key)) return null;
        try
        {
            YamlSequenceNode node = (YamlSequenceNode)yaml[key];
            IList<string> list = [];
            foreach (YamlNode child in node.Children)
            {
                if (child is YamlScalarNode scalarNode && scalarNode.Value != null)
                    list.Add(scalarNode.Value);
            }

            return list;
        }
        catch
        {
            return null;
        }
    }

    public static string[]? GetStringArray(this YamlNode yaml, string key)
    {
        IList<string>? list = yaml.GetStringList(key);

        return list == null ? null : [.. list];
    }

    public static IList<T>? GetNumberList<T>(this YamlNode yaml, string key) where T : INumber<T>
    {
        if (!yaml.Contains(key)) return null;
        try
        {
            YamlSequenceNode node = (YamlSequenceNode)yaml[key];
            IList<T> list = [];
            foreach (YamlNode child in node.Children)
            {
                if (child is YamlScalarNode scalarNode && scalarNode.Value != null)
                {
                    try
                    {
                        list.Add(T.Parse(scalarNode.Value, NumberStyles.Number, new NumberFormatInfo()));
                    }
                    catch { }
                }
            }

            return list;
        }
        catch
        {
            return null;
        }
    }

    public static IList<int>? GetIntList(this YamlNode yaml, string key)
    {
        return yaml.GetNumberList<int>(key);
    }

    public static IList<long>? GetLongList(this YamlNode yaml, string key)
    {
        return yaml.GetNumberList<long>(key);
    }

    public static IList<float>? GetFloatList(this YamlNode yaml, string key)
    {
        return yaml.GetNumberList<float>(key);
    }

    public static IList<double>? GetDoubleList(this YamlNode yaml, string key)
    {
        return yaml.GetNumberList<double>(key);
    }

    public static T[]? GetNumberArray<T>(this YamlNode yaml, string key) where T : INumber<T>
    {
        IList<T>? list = yaml.GetNumberList<T>(key);

        return list == null ? null : [.. list];
    }
    public static int[]? GetIntArray(this YamlNode yaml, string key)
    {
        return yaml.GetNumberArray<int>(key);
    }

    public static long[]? GetLongArray(this YamlNode yaml, string key)
    {
        return yaml.GetNumberArray<long>(key);
    }

    public static float[]? GetFloatArray(this YamlNode yaml, string key)
    {
        return yaml.GetNumberArray<float>(key);
    }

    public static double[]? GetDoubleArray(this YamlNode yaml, string key)
    {
        return yaml.GetNumberArray<double>(key);
    }

    public static T GetNumber<T>(this YamlNode yaml, string key) where T : INumber<T>
    {
        if (!yaml.Contains(key)) return T.Zero;
        try
        {
            YamlScalarNode node = (YamlScalarNode)yaml[key];
            if (node.Value == null) return T.Zero;

            return T.Parse(node.Value, new NumberFormatInfo());
        }
        catch
        {
            return T.Zero;
        }
    }

    public static T GetNumber<T>(this YamlNode yaml, string key, T @default) where T : INumber<T>
    {
        if (!yaml.Contains(key)) return @default;
        try
        {
            YamlScalarNode node = (YamlScalarNode)yaml[key];
            if (node.Value == null) return @default;

            return T.Parse(node.Value, new NumberFormatInfo());
        }
        catch
        {
            return @default;
        }
    }

    public static int GetInt(this YamlNode yaml, string key)
    {
        return yaml.GetNumber<int>(key);
    }
    public static int GetInt(this YamlNode yaml, string key, int @default)
    {
        return yaml.GetNumber(key, @default);
    }

    public static long GetLong(this YamlNode yaml, string key)
    {
        return yaml.GetNumber<long>(key);
    }

    public static long GetLong(this YamlNode yaml, string key, long @default)
    {
        return yaml.GetNumber(key, @default);
    }

    public static float GetFloat(this YamlNode yaml, string key)
    {
        return yaml.GetNumber<float>(key);
    }

    public static float GetFloat(this YamlNode yaml, string key, float @default)
    {
        return yaml.GetNumber(key, @default);
    }

    public static double GetDouble(this YamlNode yaml, string key)
    {
        return yaml.GetNumber<double>(key);
    }

    public static double GetDouble(this YamlNode yaml, string key, double @default)
    {
        return yaml.GetNumber(key, @default);
    }

    public static bool GetBoolean(this YamlNode yaml, string key)
    {
        string data = yaml.GetString(key, "false");

        return bool.Parse(data);
    }

    public static bool GetBoolean(this YamlNode yaml, string key, bool @default)
    {
        string data = yaml.GetString(key, @default.ToString());

        return bool.Parse(data);
    }

    #endregion

    #region set
    public static void SetString(this YamlNode yaml, string key, string value)
    {
        if (yaml is YamlMappingNode mappingNode) mappingNode.Add(key, value);
    }

    public static void SetStringList(this YamlNode yaml, string key, IList<string> values)
    {
        if (yaml is YamlMappingNode mappingNode) mappingNode.Add(key, new YamlSequenceNode(values.Select(x => new YamlScalarNode(x))));
    }

    public static void SetStringArray(this YamlNode yaml, string key, string[] values)
    {
        if (yaml is YamlMappingNode mappingNode) mappingNode.Add(key, new YamlSequenceNode(values.Select(x => new YamlScalarNode(x))));
    }

    public static void SetNumberList<T>(this YamlNode yaml, string key, IList<T> values) where T : INumber<T>
    {
        if (yaml is YamlMappingNode mappingNode) mappingNode.Add(key, new YamlSequenceNode(values.Select(x => new YamlScalarNode(x.ToString(null, new NumberFormatInfo())))));
    }

    public static void SetIntList(this YamlNode yaml, string key, IList<int> values)
    {
        yaml.SetNumberList(key, values);
    }

    public static void SetLongList(this YamlNode yaml, string key, IList<long> values)
    {
        yaml.SetNumberList(key, values);
    }

    public static void SetFloatList(this YamlNode yaml, string key, IList<float> values)
    {
        yaml.SetNumberList(key, values);
    }

    public static void SetDoubleList(this YamlNode yaml, string key, IList<double> values)
    {
        yaml.SetNumberList(key, values);
    }

    public static void SetNumberArray<T>(this YamlNode yaml, string key, T[] values) where T : INumber<T>
    {
        yaml.SetNumberList(key, values);
    }
    public static void SetIntArray(this YamlNode yaml, string key, int[] values)
    {
        yaml.SetNumberArray(key, values);
    }

    public static void SetLongArray(this YamlNode yaml, string key, long[] values)
    {
        yaml.SetNumberArray(key, values);
    }

    public static void SetFloatArray(this YamlNode yaml, string key, float[] values)
    {
        yaml.SetNumberArray(key, values);
    }

    public static void SetDoubleArray(this YamlNode yaml, string key, double[] values)
    {
        yaml.SetNumberArray(key, values);
    }

    public static void SetNumber<T>(this YamlNode yaml, string key, T value) where T : INumber<T>
    {
        if (yaml is YamlMappingNode mappingNode) mappingNode.Add(key, value.ToString(null, new NumberFormatInfo()));
    }

    public static void SetInt(this YamlNode yaml, string key, int value)
    {
        yaml.SetNumber(key, value);
    }

    public static void SetLong(this YamlNode yaml, string key, long value)
    {
        yaml.SetNumber(key, value);
    }

    public static void SetFloat(this YamlNode yaml, string key, float value)
    {
        yaml.SetNumber(key, value);
    }

    public static void SetDouble(this YamlNode yaml, string key, double value)
    {
        yaml.SetNumber(key, value);
    }

    public static void SetBoolean(this YamlNode yaml, string key, bool value)
    {
        if (yaml is YamlMappingNode mappingNode) mappingNode.Add(key, value.ToString());
    }

    #endregion

    public static bool IsList(this YamlNode yaml, string key)
    {
        return yaml[key] is YamlSequenceNode;
    }
}
