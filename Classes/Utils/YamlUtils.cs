using System.Globalization;
using System.Numerics;
using YamlDotNet.RepresentationModel;

namespace rscconventer.Classes.Utils;

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

    public static string Geting(this YamlNode yaml, string key, string @default)
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
        IList<string>? list = GetStringList(yaml, key);

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
                        list.Add(T.Parse(scalarNode.Value, System.Globalization.NumberStyles.Number, new NumberFormatInfo()));
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
        return GetNumberList<int>(yaml, key);
    }

    public static IList<long>? GetLongList(this YamlNode yaml, string key)
    {
        return GetNumberList<long>(yaml, key);
    }

    public static IList<float>? GetFloatList(this YamlNode yaml, string key)
    {
        return GetNumberList<float>(yaml, key);
    }

    public static IList<double>? GetDoubleList(this YamlNode yaml, string key)
    {
        return GetNumberList<double>(yaml, key);
    }

    public static T[]? GetNumberArray<T>(this YamlNode yaml, string key) where T : INumber<T>
    {
        IList<T>? list = GetNumberList<T>(yaml, key);

        return list == null ? null : [.. list];
    }
    public static int[]? GetIntArray(this YamlNode yaml, string key)
    {
        return GetNumberArray<int>(yaml, key);
    }

    public static long[]? GetLongArray(this YamlNode yaml, string key)
    {
        return GetNumberArray<long>(yaml, key);
    }

    public static float[]? GetFloatArray(this YamlNode yaml, string key)
    {
        return GetNumberArray<float>(yaml, key);
    }

    public static double[]? GetDoubleArray(this YamlNode yaml, string key)
    {
        return GetNumberArray<double>(yaml, key);
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

    public static int GetInt(this YamlNode yaml, string key)
    {
        return yaml.GetNumber<int>(key);
    }

    public static bool GetBoolean(this YamlNode yaml, string key)
    {
        string data = yaml.Geting(key, "false");

        return bool.Parse(data);
    }

    public static bool GetBoolean(this YamlNode yaml, string key, bool @default)
    {
        string data = yaml.Geting(key, @default.ToString());

        return bool.Parse(data);
    }

    public static bool IsList(this YamlNode yaml, string key)
    {
        return yaml[key] is YamlSequenceNode;
    }
}
