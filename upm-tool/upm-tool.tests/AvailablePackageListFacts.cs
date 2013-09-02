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

        PackageDetails CreateStubPackage(string name) {
            return new PackageDetails  {
                Name = name,
            };
        }

		[Test()]
		public void Count_NoPackages_EmptyList ()
		{
			var emptyGithubRepo = CreateGithubServiceStub (new GithubDirectoryContent[] { });

			var packageList = new AvailablePackageList(emptyGithubRepo);

			Assert.AreEqual(packageList.Count(), 0);
		}

		[Test()]
		public void IEnumerable_OnePackage_IsOnlyPackage() 
		{
            var stubPackage = CreateStubPackage("example");
			var stubGithubFile = new GithubDirectoryContent {
				Type = "file",
				Name = stubPackage.Name + ".upm",
			};
			var stubGithub 
				= CreateGithubServiceStub (new GithubDirectoryContent[] { stubGithubFile });

			var packageList = new AvailablePackageList(stubGithub);

            CollectionAssert.AreEqual(packageList as IEnumerable<PackageDetails>, 
                                      new PackageDetails[] { stubPackage });
		}

		[Test]
		public void Count_OneNonUPMFile_CountOf0() 
		{
			var ignoredPackage = new GithubDirectoryContent {
				Type = "file",
				Name = "ignore.me",
			};
			var stubGithub 
				= CreateGithubServiceStub (new GithubDirectoryContent[] { ignoredPackage });

			var packageList = new AvailablePackageList(stubGithub);

			Assert.AreEqual(packageList.Count(), 0);
		}
	}
}

