using System.Collections.Generic;
using System.IO;

namespace xidata_v2.source
{
	internal class Data
	{
		public static List<string> GetDataForContentType(string contentType)
		{
			switch (contentType)
			{
				case "Zones": return GetResourceData("xidata_zones");
				case "Armor": return GetResourceData("xidata_gear");
				case "Weapons": return GetResourceData("xidata_gear");
				case "Weapons Race": return GetResourceData("xidata_gear");
				case "NPC": return GetResourceData("xidata_npc");
				case "Animations": return GetResourceData("xidata_animations");
				default: break;
			}

			return null;
		}

		/// <summary>
		/// returns all the lines in a xidata resource data txt file.
		/// </summary>
		/// <param name="resourceDataName"></param>
		/// <returns></returns>
		private static List<string> GetResourceData(string resourceDataName)
		{
			List<string> lines = [];
			string filename = $"{Folders.Root()}\\data\\{resourceDataName}.txt";

			if (File.Exists(filename))
			{
				using StreamReader reader = new(filename);

				while (!reader.EndOfStream)
				{
					string line = reader.ReadLine();
					lines.Add(line);
				}
			}
			else
			{
				Logger.Add("xidata resource file not found: " + filename);
			}

			return lines;
		}
	}
}
