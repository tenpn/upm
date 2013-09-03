using System.IO;
using NUnit.Framework;

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
                + "summary: |\n" + summary;
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
    }
}