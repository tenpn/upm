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

            var contents = m_githubService.GetDirectoryContents ("packages");
            var availablePackages = contents.Where(pkg => pkg.Name.ToLower().EndsWith(".upm"));

			foreach (var file in availablePackages) {
                string packageName = file.Name.Substring(0, file.Name.Length - 4);
				m_cachedPackages.Add (new PackageDetails {
                        Name = packageName,
                    });
			}
		}

		public IEnumerator<PackageDetails> GetEnumerator()
		{
            return m_cachedPackages.GetEnumerator(); 
        }

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator ();
		}

		IGithubService m_githubService;
        List<PackageDetails> m_cachedPackages = new List<PackageDetails>();
	}
}

