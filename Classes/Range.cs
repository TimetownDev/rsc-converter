namespace rsc_converter.Classes;

public class Range
{
    public int Start { get; set; }
    public int End { get; set; }
    public Range(int start, int end)
    {
        Start = start;
        End = end;
    }
    public Range(int number)
    {
        Start = number;
        End = number;
    }
    public int[] ToArray()
    {
        IList<int> ints = [];
        int i = Start;
        while (i <= End)
        {
            ints.Add(i);
            i++;
        }

        return ints.ToArray();
    }

    public IList<int> ToList()
    {
        IList<int> ints = [];
        int i = Start;
        while (i <= End)
        {
            ints.Add(i);
            i++;
        }

        return ints;
    }

    public static Range Parse(string s)
    {
        string[] strings = s.Split("-");
        if (strings.Length == 2)
            return new Range(int.Parse(strings[0]), int.Parse(strings[1]));
        else if (strings.Length == 1)
            return new Range(int.Parse(strings[0]), int.Parse(strings[0]));

        throw new ArgumentException($"Invalid range {s}");
    }

    public static bool IsRange(string? s)
    {
        if (s == null) return false;
        string[] strings = s.Split("-");
        if (strings.Length == 1 || strings.Length == 2)
            return true;

        return false;
    }
}
