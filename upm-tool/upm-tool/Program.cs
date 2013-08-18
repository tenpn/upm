using System;

namespace upmtool
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Hello World!");
			foreach (var packageName in GithubPackagesStore.GetPackages ()) {
				Console.WriteLine (packageName);
			}
		}
	}
}
