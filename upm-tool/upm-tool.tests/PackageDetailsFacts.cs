using NUnit.Framework;

namespace upmtool.tests
{
	[TestFixture]
	public class PackageDetailsFacts 
    {
        [Test]
        public void Equals_Null_ReturnsFalse()
        {
            var exampleDetails = new PackageDetails  {
                Name = "foo",
            };

            Assert.AreNotEqual(exampleDetails, null);
        }

        [Test]
        public void Equals_OtherPackageWithSameName_ReturnsTrue() 
        {
            string packageName = "foo";
            var exampleDetails = new PackageDetails  {
                Name = packageName,
            };
            var equalDetails = new PackageDetails  {
                Name = packageName,
            };

            Assert.AreEqual(exampleDetails, equalDetails);
        }

        [Test]
        public void Equals_OtherPackageWithDifferentName_ReturnsFalse()
        {
            var exampleDetails = new PackageDetails  {
                Name = "foo",
            };
            var equalDetails = new PackageDetails  {
                Name = "bar",
            };

            Assert.AreNotEqual(exampleDetails, equalDetails);
        }
        
    }
}
