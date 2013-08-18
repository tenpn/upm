using System;
using ServiceStack.Text;
using System.Linq;
using System.Collections.Generic;

namespace upmtool
{
	public class GithubPackagesStore
	{
		public GithubPackagesStore ()
		{
		}

		public static IEnumerable<string> GetPackages() 
		{
			var packagesCommand = "https://api.github.com/"
				.AppendPath ("repos/tenpn/upm/contents/packages");
			var packagesData = packagesCommand.GetJsonFromUrl ();
			var packageList = packagesData.FromJson<List<GithubDirectoryContent>> ();

			return packageList.Where (package => package.Name.EndsWith (".upm"))
				.Select (package => package.Name);
		}

		private class GithubDirectoryContent 
		{
			public string Type { get; set; }
			public string Name { get; set; }
		}
	}
}

