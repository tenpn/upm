using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace upmtool
{
	public class AvailablePackageList : IEnumerable<object>
	{
		public AvailablePackageList (IGithubService github)
		{
			m_githubService = github;
		}

		public IEnumerator<object> GetEnumerator()
		{
			var contents = m_githubService.GetDirectoryContents ("packages");
			var res = new List<object> ();
            var availablePackages = contents.Where(pkg => pkg.Name.ToLower().EndsWith(".upm"));
			foreach (var file in availablePackages) {
				res.Add (null);
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

