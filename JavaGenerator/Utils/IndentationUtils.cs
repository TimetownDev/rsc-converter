using System.Text;

namespace rscconventer.JavaGenerator.Utils;

public static class IndentationUtils
{
    public static string Indente(string text, int count = 1, int spaceCount = 4)
    {
        string spaces = string.Empty;
        for (int i = 0; i < spaceCount; i++)
        {
            spaces += " ";
        }

        IList<string> lines = text.Split("\n");
        StringBuilder sb = new();
        int x = 0;
        foreach (string line in lines)
        {
            if (line.Trim().Length > 0)
            {
                sb.Append(spaces);
                sb.Append(line.TrimEnd());
                if (x + 1 < lines.Count)
                {
                    sb.Append('\n');
                }
            }
            x++;
        }

        return sb.ToString();
    }
}
