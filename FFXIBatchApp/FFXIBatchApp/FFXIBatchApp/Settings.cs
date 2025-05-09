using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFXIBatchApp
{
	internal class Settings
	{
		public Settings() { }

		public static string GetSetting(string name)
		{
			return (string)Properties.Settings.Default[name];
		}

		public static void SaveSetting(string name, string value)
		{
			Properties.Settings.Default[name] = value;
			Properties.Settings.Default.Save();
		}
	}
}
