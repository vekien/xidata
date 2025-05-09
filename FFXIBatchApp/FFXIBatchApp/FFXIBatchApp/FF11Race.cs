using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFXIBatchApp
{
	internal class FF11Race
	{
		static string[] races =
		{
			"Elvaan_Female",
			"Elvaan_Male",
			"Galka",
			"Hume_Female",
			"Hume_Male",
			"Mithra",
			"Tarutaru"
		};

		// These are the base skeletons for each race
		// They match the races index 1:1, so the 1st is Elvaan Female.
		static string[] skeletons =
		{
			"\\42\\4",
			"\\37\\31",
			"\\56\\59",
			"\\32\\58",
			"\\27\\82",
			"\\51\\89",
			"\\46\\93"
		};

		// These are the race names of the AltanaPC Folder
		// it is important they match index 1:1 of the races list.
		static string[] racesAltana =
		{
			"ElvaanF",
			"ElvaanM",
			"Galka",
			"HumeF",
			"HumeM",
			"Mithra",
			"Tarutaru"
		};

		// This is the struct for lookdata races, with their respective skeletons
		static string[] racesLook =
		{
			"",
			"Hume_Male|\\27\\82",
			"Hume_Female|\\32\\58",
			"Elvaan_Male|\\37\\31",
			"Elvaan_Female|\\42\\4",
			"Tarutaru|\\46\\93", // M
			"Tarutaru|\\46\\93", // F
			"Mithra|\\51\\89",
			"Galka|\\56\\59",

			// there is nothing for a bit here

			// 29 = Mithra Child
			// 30 = Hume/Elvaan Child F
			// 31 = Hume/Elvaan child M

			// 32,33,34,35,36 = Chocobo
		};

		public static string[] GetRaces()
		{
			return races;
		}

		public static string[] GetRacesLook() 
		{ 
			return racesLook; 
		}

		public static string GetSkeleton(string racename)
		{
			int index = Array.IndexOf(races, racename);

			if (index >= 0 && index < skeletons.Length)
			{
				return skeletons[index];
			}
			else
			{
				// Handle invalid racename
				return null;
			}
		}

		public static string GetAltanaRace(string racename)
		{
			int index = Array.IndexOf(races, racename);

			if (index >= 0 && index < racesAltana.Length)
			{
				return racesAltana[index];
			}
			else
			{
				// Handle invalid racename
				return null;
			}
		}
	}
}
