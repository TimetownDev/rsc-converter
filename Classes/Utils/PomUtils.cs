using System.Xml;

namespace Classes.Utils;

public static class PomUtils
{
    public static void AddRepository(this XmlDocument pom, string id, string url)
    {
        XmlNode projectNode = pom.GetElementsByTagName("project")[0]!;
        // 创建 <repositories> 元素
        XmlNode? repositoriesNode = pom.GetElementsByTagName("repositories")[0];
        if (repositoriesNode == null)
        {
            repositoriesNode = pom.CreateElement("repositories");
            projectNode.AppendChild(repositoriesNode);
        }

        // 创建 <repository> 元素
        XmlElement repositoryNode = pom.CreateElement("repository");

        // 添加 <id> 元素
        XmlElement idNode = pom.CreateElement("id");
        idNode.InnerText = id;
        repositoryNode.AppendChild(idNode);

        // 添加 <url>素
        XmlElement urlNode = pom.CreateElement("url");
        urlNode.InnerText = url;
        repositoryNode.AppendChild(urlNode);

        // 将 <repository> 添加到 <repositories>
        repositoriesNode.AppendChild(repositoryNode);

        // 将 <repositories> 添加到 <project>
        projectNode.AppendChild(repositoriesNode);
    }

    public static void AddDependency(this XmlDocument pom, string groupId, string artifactId, string version)
    {
        XmlNode projectNode = pom.GetElementsByTagName("project")[0]!;
        // 创建 <dependencies>素
        XmlNode? dependenciesNode = pom.GetElementsByTagName("dependencies")[0];
        if (dependenciesNode == null)
        {
            dependenciesNode = pom.CreateElement("dependencies");
            projectNode.AppendChild(dependenciesNode);
        }

        // 创建 <dependency> 元素
        XmlElement dependencyNode = pom.CreateElement("dependency");

        // 添加 <groupId> 元素
        XmlElement groupIdNode = pom.CreateElement("groupId");
        groupIdNode.InnerText = groupId;
        dependencyNode.AppendChild(groupIdNode);

        // 添加 <artifactId> 元素
        XmlElement artifactIdNode = pom.CreateElement("artifactId");
        artifactIdNode.InnerText = artifactId;
        dependencyNode.AppendChild(artifactIdNode);

        // 添加 <version> 元素
        XmlElement versionNode = pom.CreateElement("version");
        versionNode.InnerText = version;
        dependencyNode.AppendChild(versionNode);

        // 添加<scope> 元素
        XmlElement scopeNode = pom.CreateElement("scope");
        scopeNode.InnerText = "provided";
        dependencyNode.AppendChild(scopeNode);

        // 将 <dependency> 添加到 <dependencies>
        dependenciesNode.AppendChild(dependencyNode);

        // 将 <dependencies> 添加到 <project>
        projectNode.AppendChild(dependenciesNode);
    }

    public static void AddPlugin(this XmlDocument pom, string groupId, string artifactId, string version, string configuration)
    {
        XmlNode projectNode = pom.GetElementsByTagName("project")[0]!;
        XmlNode pluginsNode = pom.GetElementsByTagName("plugins")[0]!;
        // 创建 <plugin> 元素
        XmlElement pluginNode = pom.CreateElement("plugin");

        // 添加<groupId> 元素
        XmlElement groupIdNode = pom.CreateElement("groupId");
        groupIdNode.InnerText = groupId;
        pluginNode.AppendChild(groupIdNode);

        // 添加 <artifactId> 元素
        XmlElement artifactIdNode = pom.CreateElement("artifactId");
        artifactIdNode.InnerText = artifactId;
        pluginNode.AppendChild(artifactIdNode);

        // 添加 <version> 元素
        XmlElement versionNode = pom.CreateElement("version");
        versionNode.InnerText = version;
        pluginNode.AppendChild(versionNode);

        // 添加 <configuration> 元素
        XmlElement configurationNode = pom.CreateElement("configuration");
        configurationNode.InnerXml = configuration;
        pluginNode.AppendChild(configurationNode);

        // 将 <plugin> 添加到 <plugins>
        pluginsNode.AppendChild(pluginNode);
    }

    public static void AddDefaultBuild(this XmlDocument pom)
    {
        XmlNode projectNode = pom.GetElementsByTagName("project")[0]!;
        // 创建 <build> 元素
        XmlElement buildNode = pom.CreateElement("build");

        // 添加 <defaultGoal> 元素
        XmlElement defaultGoalNode = pom.CreateElement("defaultGoal");
        defaultGoalNode.InnerText = "clean package";
        buildNode.AppendChild(defaultGoalNode);

        // 添加 <plugins> 元素
        XmlElement pluginsNode = pom.CreateElement("plugins");

        // 将 <plugins> 添加到 <build>
        buildNode.AppendChild(pluginsNode);

        // 将 <build> 添加到 <project>
        projectNode.AppendChild(buildNode);

        // 添加 maven-compiler-plugin
        pom.AddPlugin("org.apache.maven.plugins", "maven-compiler-plugin", "3.12.1", @"
              <source>16</source>
              <target>16</target>
        ");

        // 添加 spotless-maven
        pom.AddPlugin("com.diffplug.spotless", "spotless-maven-plugin", "2.43.0", @"
              <java>
                  <palantirJavaFormat>
                      <version>2.38.0</version>
                      <style>PALANTIR</style>
                  </palantirJavaFormat>
                  <removeUnusedImports />
                  <formatAnnotations />
              </java>
        ");
    }
}
