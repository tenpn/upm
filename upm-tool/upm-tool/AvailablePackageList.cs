using System;
using System.Collections;
using System.Collections.Generic;

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
			foreach (var file in contents) {
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

