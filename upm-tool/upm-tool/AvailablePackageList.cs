using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace upmtool
{
	public class AvailablePackageList : IEnumerable<PackageDetails>
	{
		public AvailablePackageList (IGithubService github)
		{
			m_githubService = github;
		}

		public IEnumerator<PackageDetails> GetEnumerator()
		{
			var contents = m_githubService.GetDirectoryContents ("packages");
			var res = new List<PackageDetails> ();
            var availablePackages = contents.Where(pkg => pkg.Name.ToLower().EndsWith(".upm"));

			foreach (var file in availablePackages) {
                string packageName = file.Name.Substring(0, file.Name.Length - 4);
				res.Add (new PackageDetails {
                        Name = packageName,
                    });
			}
			return res.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator ();
		}

		IGithubService m_githubService;
	}
}

