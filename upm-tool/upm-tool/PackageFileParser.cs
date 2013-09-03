using System.IO;
using YamlDotNet.RepresentationModel;
using System.Linq;

namespace upmtool 
{
    public class PackageFileParser
    {
        public static PackageDetails Parse(TextReader file)
        {
            var yamlStream = new YamlStream();
            yamlStream.Load(file);
            
            var root = yamlStream.Documents[0].RootNode as YamlMappingNode;
            var name = root.Children[new YamlScalarNode("name")].ToString();
            var version = root.Children[new YamlScalarNode("version")].ToString();
            
            var summary = root.Children[new YamlScalarNode("summary")].ToString()
                .Trim('\0').Trim();
            
            return new PackageDetails {
                Name = name,
                VersionString = version,
                Summary = summary,
            };
        }
    }
}