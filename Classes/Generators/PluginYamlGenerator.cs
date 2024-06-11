using rsc_converter.Classes;
using rsc_converter.Classes.Interfaces;
using rsc_converter.Classes.Utils;
using System.Text;
using System.Xml;
using YamlDotNet.RepresentationModel;

namespace rsc_converter.Classes.Generators;

public class PluginYamlGenerator : IFileGenerator
{
    public IList<FileData>? OnGenerate(BuildSession session)
    {
        IList<FileData> result = [];
        #region plugin.yml生成
        YamlMappingNode resultNode = [];
        YamlStream stream = [];
        stream.Load(new StringReader(File.ReadAllText(Path.Combine(session.Directory.FullName, "info.yml"))));
        YamlMappingNode yaml = (YamlMappingNode)stream.Documents[0].RootNode;
        string name = yaml.GetString("id", "");
        if (name.Trim() == string.Empty) throw new ArgumentException("id不能为空");

        string displayName = yaml.GetString("name", name);
        IList<string> dependencies = ["GuguSlimefunLib", "Slimefun"];
        IList<string>? depends = yaml.GetStringList("depends");
        if (depends != null)
        {
            foreach (string depend in depends)
            {
                dependencies.Add(depend);
            }
        }
        IList<string>? pluginDepends = yaml.GetStringList("pluginDepends");
        if (pluginDepends != null)
        {
            foreach (string pluginDepend in pluginDepends)
            {
                dependencies.Add(pluginDepend);
            }
        }

        string version = yaml.GetString("version", "1.0");
        string? description = yaml.GetString("description");
        IList<string>? authors = yaml.GetStringList("authors");
        authors ??= [];
        authors.Add("rsc-generator");

        resultNode.SetString("name", name);
        resultNode.SetStringList("depend", dependencies);
        resultNode.SetString("version", version);
        if (description != null)
            resultNode.SetString("description", description);
        resultNode.SetString("api-version", "1.16");
        resultNode.SetStringList("authors", authors);
        resultNode.SetString("main", $"me.ddggdd135.{name}.{char.ToUpper(session.Name[0]) + session.Name[1..]}Main");

        stream = [];
        stream.Documents.Add(new(resultNode));
        using StringWriter pluginYmlWriter = new();
        stream.Save(pluginYmlWriter);
        result.Add(new("plugin.yml", "src/main/resources", Encoding.UTF8.GetBytes(pluginYmlWriter.ToString())));
        #endregion

        #region pom.xml生成
        XmlDocument pom = new();

        // 创建根节点 <project>
        XmlElement projectNode = pom.CreateElement("project");
        pom.AppendChild(projectNode);

        // 添加 <modelVersion> 元素
        XmlElement modelVersionNode = pom.CreateElement("modelVersion");
        modelVersionNode.InnerText = "4.0.0";
        projectNode.AppendChild(modelVersionNode);

        // 添加 <groupId> 元素
        XmlElement groupIdNode = pom.CreateElement("groupId");
        groupIdNode.InnerText = $"me.ddggdd135.{name}";
        projectNode.AppendChild(groupIdNode);

        // 添加 <artifactId> 元素
        XmlElement artifactIdNode = pom.CreateElement("artifactId");
        artifactIdNode.InnerText = name;
        projectNode.AppendChild(artifactIdNode);

        // 添加 <version> 元素
        XmlElement versionNode = pom.CreateElement("version");
        versionNode.InnerText = version;
        projectNode.AppendChild(versionNode);

        // 添加 <name> 元素
        XmlElement nameNode = pom.CreateElement("name");
        nameNode.InnerText = name;
        projectNode.AppendChild(nameNode);

        // 创建 <properties>
        XmlElement propertiesNode = pom.CreateElement("properties");
        projectNode.AppendChild(propertiesNode);

        // 添加属性
        XmlElement javaVersionNode = pom.CreateElement("java.version");
        javaVersionNode.InnerText = "16";
        propertiesNode.AppendChild(javaVersionNode);

        // 添加编码属性
        XmlElement encodingNode = pom.CreateElement("project.build.sourceEncoding");
        encodingNode.InnerText = "UTF-8";
        propertiesNode.AppendChild(encodingNode);

        pom.AddDefaultBuild();
        pom.AddRepository("spigot-repo", "https://hub.spigotmc.org/nexus/content/repositories/snapshots");
        pom.AddRepository("papermc-repo", "https://repo.papermc.io/repository/maven-public");
        pom.AddRepository("sonatype", "https://oss.sonatype.org/content/groups/public");
        pom.AddRepository("jitpack", "https://jitpack.io");
        pom.AddRepository("codemc-repo", "https://repo.codemc.org/repository/maven-public");

        pom.AddDependency("io.papermc.paper", "paper-api", "1.20.1-R0.1-SNAPSHOT");
        pom.AddDependency("com.github.SlimefunGuguProject", "Slimefun4", "dev-SNAPSHOT");
        pom.AddDependency("com.github.TimetownDev", "GuguSlimefunLib", "db5cd4152e");

        using MemoryStream pomStream = new();
        pom.Save(pomStream);
        result.Add(new("pom.xml", "", pomStream.ToArray()));
        #endregion

        return result;
    }
}
