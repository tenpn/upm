using System.IO;
using NUnit.Framework;

namespace upmtool.tests
{
    [TestFixture]
    public class PackageFileParserFacts
    {
        TextReader CreatePackageFile(string name, string version)
        {
            string file = "name: " + name + "\n"
                + "version: " + version;
            return new StringReader(file);
        }

        [Test]
        public void Parse_ValidPackageFile_StoresName()
        {
            string packageName = "foo";
            var packageFile = CreatePackageFile(packageName, "1");
            
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