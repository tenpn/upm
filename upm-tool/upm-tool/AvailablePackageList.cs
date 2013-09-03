using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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

        public static PackageDetails FindPackageByName(
            this IEnumerable<PackageDetails> packages, string searchTerms) 
        {
            var searchRegexPattern = "^" + Regex.Escape(searchTerms)
                .Replace(@"\*", ".*")
                .Replace(@"\?", ".") 
                + "$";
            var searcher = new Regex(searchRegexPattern, RegexOptions.IgnoreCase);

            return packages.FirstOrDefault(pkg => searcher.IsMatch(pkg.Name));
        }
	}
}

