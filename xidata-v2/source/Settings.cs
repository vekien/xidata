using IniParser;
using IniParser.Model;
using System.IO;

namespace xidata_v2.source
{
	internal class Settings
	{
		private static readonly string FileSettings = $"{Folders.Root()}\\settings.ini";
		private static IniData IniSettings;

		/// <summary>
		/// Initialize App Settings
		/// </summary>
		public static void InitSettings()
		{
			if (!File.Exists(FileSettings))
			{
				File.WriteAllText(FileSettings, "; xidata\n; Do not add trailing slashes on paths.\n[xidata]");
			}

			// Parse Settings
			var parser = new FileIniDataParser();
			IniData data = parser.ReadFile(FileSettings);

			// Add missing values
			data = AddMissingIniValues(data);

			// Save ini settings
			parser.WriteFile(FileSettings, data);

			// Reload them
			IniSettings = parser.ReadFile(FileSettings);
		}

		/// <summary>
		/// This adds any missing values and sets them to their defaults.
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		private static IniData AddMissingIniValues(IniData data)
		{
			if (data["xidata"]["noesis_path"] == null)
			{
				data["xidata"]["noesis_path"] = "D:\\path\\to\\noesis.exe";
			}

			if (data["xidata"]["noesis_args"] == null)
			{
				data["xidata"]["noesis_args"] = "-ff11bumpdir normals -ff11noshiny -ff11hton 16 -ff11optimizegeo -ff11keepnames -fbxtexrelonly -fbxtexext .png -rotate 180 0 270 -scale 120";
			}

			if (data["xidata"]["ffxi_path"] == null)
			{
				data["xidata"]["ffxi_path"] = "D:\\path\\to\\FINAL FANTASY XI";
			}

			if (data["xidata"]["sendkey_delay"] == null)
			{
				data["xidata"]["sendkey_delay"] = "400";
			}

			if (data["xidata"]["debug_race"] == null)
			{
				data["xidata"]["debug_race"] = "";
			}

			if (data["xidata"]["debug_anim"] == null)
			{
				data["xidata"]["debug_anim"] = "";
			}

			return data;
		}

		public static string GetNoesisPath()
		{
			return IniSettings["xidata"]["noesis_path"];
		}

		public static string GetNoesisArgs()
		{
			return IniSettings["xidata"]["noesis_args"];
		}

		public static string GetFFxiPath()
		{
			return IniSettings["xidata"]["ffxi_path"];
		}

		public static int GetSendKeyDelay()
		{
			return int.Parse(IniSettings["xidata"]["sendkey_delay"]);
		}

		public static string[] GetDebug()
		{
			return
			[
				IniSettings["xidata"]["debug_race"],
				IniSettings["xidata"]["debug_anim"]
			];
		}
	}
}
