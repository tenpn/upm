using System;
using System.Collections.Generic;

namespace upmtool
{
	public struct GithubDirectoryContent 
	{
		public string Type { get; set; }
		public string Name { get; set; }
	}

	public interface IGithubService
	{
		IEnumerable<GithubDirectoryContent> GetDirectoryContents(string dirName);
	}
}

