//
// PackageListFacts.cs
//
// Author:
//       Andrew <>
//
// Copyright (c) 2013 Andrew
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using NUnit.Framework;
using System.Linq;
using Moq;
using System.Collections.Generic;

namespace upmtool.tests
{
	[TestFixture()]
	public class AvailablePackageListFacts
	{
		IGithubService CreateGithubServiceStub(IEnumerable<GithubDirectoryContent> stubGithubFiles) 
		{
			var stubGithub = new Mock<IGithubService> ();
			stubGithub.Setup(
                gitService => gitService.GetDirectoryContents(It.IsAny<string>()))
				.Returns (stubGithubFiles);
			return stubGithub.Object;
		}

        public static PackageDetails CreatePackageWithName(string name) {
            return new PackageDetails  {
                Name = name,
            };
        }

        public static IEnumerable<PackageDetails> CreateAvailablePackageList(
            IGithubService githubService) {

            return AvailablePackageList.FetchPackageList(githubService);
        }

		[Test()]
		public void Count_NoPackages_EmptyList ()
		{
			var emptyGithubRepo = CreateGithubServiceStub (new GithubDirectoryContent[] { });

			var packageList = CreateAvailablePackageList(emptyGithubRepo);

			CollectionAssert.IsEmpty(packageList);
		}

		[Test()]
		public void IEnumerable_OnePackage_IsOnlyPackage() 
		{
            var stubPackage = CreatePackageWithName("example");
			var stubGithubFile = new GithubDirectoryContent {
				Type = "file",
				Name = stubPackage.Name + ".upm",
			};
			var stubGithub 
				= CreateGithubServiceStub (new GithubDirectoryContent[] { stubGithubFile });

			var packageList = CreateAvailablePackageList(stubGithub);

            var idealPackageList = new PackageDetails[] { stubPackage };
            CollectionAssert.AreEqual(packageList, idealPackageList);
		}

		[Test]
		public void IEnumerable_OneNonUPMFile_IsEmptyCollection() 
		{
			var ignoredPackage = new GithubDirectoryContent {
				Type = "file",
				Name = "ignore.me",
			};
			var stubGithub 
				= CreateGithubServiceStub (new GithubDirectoryContent[] { ignoredPackage });

			var packageList = CreateAvailablePackageList(stubGithub);

            CollectionAssert.IsEmpty(packageList);
		}

        // this is good because it means less network traffic
        [Test]
        public void IEnumerable_CalledTwice_OnlyCallsToGithubOnce() 
        {
            var mockGithub = new Mock<IGithubService> ();
			mockGithub.Setup(
                gitService => gitService.GetDirectoryContents(It.IsAny<string>()))
				.Returns (new GithubDirectoryContent[] { });
			var packageList = CreateAvailablePackageList(mockGithub.Object);
            
            packageList.GetEnumerator();
            packageList.GetEnumerator();

            mockGithub.Verify(service => service.GetDirectoryContents(It.IsAny<string>()),
                              Times.Exactly(1));
        }

        [Test]
        public void FindPackageByName_WildcardWithNoPackage_ReturnsNull() 
        {
            var packageList = new PackageDetails[] { };

            var foundPackage = packageList.FindPackageByName("*");

            Assert.IsNull(foundPackage);
        }

        private struct FindPackageByNameTester
        {
            public FindPackageByNameTester(string packageName)
            {
                TargetPackage = CreatePackageWithName(packageName);
                PackageList = new PackageDetails[] { TargetPackage };
            }

            public IEnumerable<PackageDetails> PackageList;
            public PackageDetails TargetPackage;

            public void AssertFoundPackageIsTarget(PackageDetails packageFound)
            {
                Assert.AreEqual(packageFound, TargetPackage);
            }
        }

        [Test]
        public void FindPackageByName_NameOfOnlyPackage_ReturnsPackage() 
        {
            var targetPackageName = "foo";
            var tester = new FindPackageByNameTester(targetPackageName);

            var foundPackage = tester.PackageList.FindPackageByName(targetPackageName);

            tester.AssertFoundPackageIsTarget(foundPackage);
        }

        [Test]
        public void FindPackageByName_NameOfOneOfManyPackages_ReturnsNamedPackage() 
        {
            var targetPackageName = "foo";
            var targetPackage = CreatePackageWithName(targetPackageName);
            var packageList = new PackageDetails[] { 
                CreatePackageWithName("making up the numbers"),
                targetPackage
            };

            var foundPackage = packageList.FindPackageByName(targetPackageName);

            Assert.AreEqual(targetPackage, foundPackage);
        }

        [Test]
        public void FindPackageByName_WildcardAsterix_ReturnsFirstPackage()
        {
            var targetPackageName = "foo";
            var tester = new FindPackageByNameTester(targetPackageName);
            var wildcardSearch = "*";

            var foundPackage = tester.PackageList.FindPackageByName(wildcardSearch);

            tester.AssertFoundPackageIsTarget(foundPackage);
        }

        [Test]
        public void FindPackageByName_WildcardMatchesAnyStart_FindsMatchingPackage() 
        {
            var targetPackageName = "foobarr";
            var tester = new FindPackageByNameTester(targetPackageName);
            var wildcardAtStart = "*barr";

            var foundPackage = tester.PackageList.FindPackageByName(wildcardAtStart);

            tester.AssertFoundPackageIsTarget(foundPackage);
        }

        [Test]
        public void FindPackageByName_SearchWithNameInWrongCase_FindsCorrectPackage()
        {
            var targetPackageName = "foobarr";
            var tester = new FindPackageByNameTester(targetPackageName);
            var uppercaseName = targetPackageName.ToUpper();

            var foundPackage = tester.PackageList.FindPackageByName(uppercaseName);

            tester.AssertFoundPackageIsTarget(foundPackage);
        }

        [Test]
        public void FindPackageByName_QueryCharacterMatchesSingleChar_FindsCorrectPackage() 
        {
            var targetPackageName = "foobarr";
            var tester = new FindPackageByNameTester(targetPackageName);
            var querySearch = "foo?arr";

            var foundPackage = tester.PackageList.FindPackageByName(querySearch);

            tester.AssertFoundPackageIsTarget(foundPackage);
        }
	}
}

