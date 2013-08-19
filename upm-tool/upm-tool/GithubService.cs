using System;
using ServiceStack.Text;
using System.Linq;
using System.Collections.Generic;

namespace upmtool
{
	public class GithubService : IGithubService
	{
		public IEnumerable<GithubDirectoryContent> GetDirectoryContents(string dirName) 
		{
			var contentsCommand = "https://api.github.com/"
					.AppendPath ("repos/tenpn/upm/contents/")
					.AppendPath (dirName);
			var contentsData = contentsCommand.GetJsonFromUrl ();
			var contentsList = contentsData.FromJson<List<GithubDirectoryContent>> ();
			return contentsList;
		}
	}
}

