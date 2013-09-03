using System.IO;
using NUnit.Framework;
using System.Linq;

namespace upmtool.tests
{
    [TestFixture]
    public class PackageFileParserFacts
    {
        TextReader CreatePackageFile(string name)
        {
            return CreatePackageFile(name, "version", "comment");
        }

        TextReader CreatePackageFile(string name, string version)
        {
            return CreatePackageFile(name, version, "comment");
        }

        TextReader CreatePackageFile(string name, string version, string summary)
        {
            string file = "name: " + name + "\n"
                + "version: " + version + "\n"
                // summary is a literal indented  yaml block
                + "summary: |\n" 
                + string.Join("\n", 
                              summary.Split('\n').Select(summaryLine => " " + summaryLine));
            return new StringReader(file);
        }

        [Test]
        public void Parse_ValidPackageFile_StoresName()
        {
            string packageName = "foo";
            var packageFile = CreatePackageFile(packageName);
            
            var parsedPackage = PackageFileParser.Parse(packageFile);

            Assert.AreEqual(packageName, parsedPackage.Name);
        }

        [Test]
        public void Parse_ValidPackageFile_StoresVersion()
        {
            var targetVersionString = "1";
            var packageFile = CreatePackageFile("name", targetVersionString);

            var parsedPackage = PackageFileParser.Parse(packageFile);

            Assert.AreEqual(targetVersionString, parsedPackage.VersionString);
        }

        [Test]
        public void Parse_ValidPackageFileWithSingleLineSummary_StoresSummary() 
        {
            var targetSummary = "summarising our package";
            var packageFile = CreatePackageFile("name", "version", targetSummary);

            var parsedPackage = PackageFileParser.Parse(packageFile);

            Assert.AreEqual(targetSummary, parsedPackage.Summary);
        }

        [Test]
        public void Parse_ValidPackageFileWithMultiLineSummary_StoresSummary() 
        {
            var targetSummary = "summarising our package\nacross multiple lines\nhere";
            var packageFile = CreatePackageFile("name", "version", targetSummary);

            var parsedPackage = PackageFileParser.Parse(packageFile);

            Assert.AreEqual(targetSummary, parsedPackage.Summary);
        }
        
    }
}