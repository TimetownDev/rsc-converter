namespace rscconventer.Classes;

public class FileData
{
    public string Name { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public byte[] Data { get; set; } = [];
    public FileData(string name, string path)
    {
        Name = name;
        Path = path;
    }

    public FileData(string name, string path, byte[] data)
    {
        Name = name;
        Path = path;
        Data = data;
    }
}
