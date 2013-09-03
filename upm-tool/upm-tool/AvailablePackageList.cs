using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace upmtool
{
	public static class AvailablePackageList 
	{
		public static IEnumerable<PackageDetails> FetchPackageList(IGithubService github)
		{
			var contents = github.GetDirectoryContents ("packages");
            var availablePackages = contents.Where(pkg => pkg.Name.ToLower().EndsWith(".upm"));

            var cachedPackages = new List<PackageDetails>();

			foreach (var file in availablePackages) {
                string packageName = file.Name.Substring(0, file.Name.Length - 4);
				cachedPackages.Add (new PackageDetails {
                        Name = packageName,
                    });
			}

            return cachedPackages;
		}

        public static PackageDetails FindPackageByName(string searchTerms) 
        {
            return null;
        }
	}
}

