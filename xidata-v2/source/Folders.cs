using System.Diagnostics;
using System.IO;

namespace xidata_v2.source
{
	internal class Folders
	{
		public static string Root()
		{
			return Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
		}
	}
}
