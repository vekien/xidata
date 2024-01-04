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

		/// <summary>
		/// Create a directory if it does not existing.
		/// </summary>
		/// <param name="folder"></param>
		public static void CreateDirectoryIfNotExists(string folder)
		{
			if (!Directory.Exists(folder))
			{
				// Get the individual directories from the path
				string[] directories = folder.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

				// Initialize the current path to the root directory
				string currentPath = directories[0];

				// Loop through each directory and create it if it doesn't exist
				for (int i = 1; i < directories.Length; i++)
				{
					currentPath = Path.Combine(currentPath, directories[i]);

					if (!Directory.Exists(currentPath))
					{
						Directory.CreateDirectory(currentPath);
					}
				}
			}
		}
	}
}
