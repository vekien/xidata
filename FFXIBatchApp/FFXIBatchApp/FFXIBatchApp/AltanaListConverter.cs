using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic.FileIO;
using System.Security.Cryptography;
using System.Text;

namespace FFXIBatchApp
{
	internal class AltanaListConverter
	{
		string savepath = "ToolData";
		Random random = new Random();

		public AltanaListConverter() { }

		public void init()
		{
			ConsoleLog("Building AltanaView Data");

			CheckDataFolderExists();

			// Build everything to make batch exports easier!
			BuildNPCList();
			BuildAnimationList();
			BuildArmorList();
			BuildWeaponList();
			BuildZoneList();
			BuildNpcLookList();
		}

		/// <summary>
		/// Basic Console Log method
		/// </summary>
		/// <param name="message"></param>
		private void ConsoleLog(string message)
		{
			Logger.Add(message);
		}

		/// <summary>
		/// Check that the AltanaData folder exists which is where we will save all our converted stuff
		/// </summary>
		private void CheckDataFolderExists()
		{
			string directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, savepath);

			if (!Directory.Exists(directoryPath))
			{
				Directory.CreateDirectory(directoryPath);
			}
		}

		/// <summary>
		/// Goes through the AltanaList and breaks out all the NPCs into a single big long list
		/// with incremented numbering for each set.
		/// </summary>
		private void BuildNPCList()
		{
			string filename = $"{savepath}/npc_1.json";
			int total = 0;

			// Already built, skip.
			if (File.Exists(filename))
			{
				return;
			}

			ConsoleLog("- BuildNPCList");

			// Hard coded skip list because these do not exist, but are in AltanaViewer
			string[] skipnames =
			{
				"Orcish_Warmachine_4",
				"Orcish_Warmachine_5",
				"Orcish_Warmachine_7",
				"Orcish_Warmachine_8"
			};

			// Create a dictionary to store the Data
			var results = new Dictionary<string, List<string>>();

			// Loop through resource names
			Assembly assembly = Assembly.GetExecutingAssembly();
			string[] resourceNames = assembly.GetManifestResourceNames();

			foreach (string resourceName in resourceNames)
			{
				if (resourceName.StartsWith("FFXIBatchApp.Resources.AltanaList.NPC"))
				{
					// Get the file name by removing the "FFXIBatchApp.Resources.AltanaList.NPC." prefix
					string fileName = resourceName.Substring("FFXIBatchApp.Resources.AltanaList.NPC.".Length);
					string npctype = fileName.Replace(".csv", "").ToUpper().Replace("_", "");

					// Do something with the file name, like print it to the console
					ConsoleLog($"-- {npctype}");

					using (Stream stream = assembly.GetManifestResourceStream(resourceName))
					using (StreamReader reader = new StreamReader(stream))
					{
						string line;
						while ((line = reader.ReadLine()) != null)
						{
							if (line.Length == 0)
							{
								continue;
							}

							string[] newline = line.Split(',');
							string name = FinalizeName(newline[1]);
							string[] dats = SplitDatRange(newline[0]);

							for (int i = 0; i < dats.Length; i++)
							{
								if (!results.ContainsKey(npctype))
								{
									results[npctype] = new List<string>();
								}

								string datpath = FinalizeDatPath(dats[i]);
								string datpath_folder = datpath.Replace("\\", "-");

								if (skipnames.Contains($"{name}_{i}"))
								{
									continue;
								}

								results[npctype].Add($"{name}_{i + 1}_{datpath_folder}|{datpath}".Trim());
								total++;
							}
						}
					}
				}
			}

			string json = JsonConvert.SerializeObject(results, Formatting.Indented);
			string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
			File.WriteAllText(filePath, json);
			File.WriteAllText(filePath + ".total", $"{total}");

			ConsoleLog("- BuildNPCList Complete!");
		}

		/// <summary>
		/// Goes through the AltanaList PC folder and grabs the "Action.csv" and builds
		/// a JSON with each action split by race
		/// </summary>
		private void BuildAnimationList()
		{
			string filename = $"{savepath}/anims_1.txt";

			// Already built, skip.
			if (File.Exists(filename))
			{
				return;
			}

			ConsoleLog("- BuildAnimationList");

			// lol this is a bit bonkers, ChatGPT made it...
			List<string> results = new List<string>();

			// for resource fetching
			Assembly assembly = Assembly.GetExecutingAssembly();

			// Loop through resource names by races
			foreach (string raceName in FF11Race.GetRaces())
			{
				string altanaRace = FF11Race.GetAltanaRace(raceName);
				string resourceName = $"FFXIBatchApp.Resources.AltanaList.PC.{altanaRace}.Action.csv";

				ConsoleLog($"-- {raceName}");

				using (Stream stream = assembly.GetManifestResourceStream(resourceName))
				using (StreamReader reader = new StreamReader(stream))
				{
					string line;
					string actionName = "DefaultAction";

					while ((line = reader.ReadLine()) != null)
					{
						if (line.Length == 0)
						{
							continue;
						}

						if (line[0] == '@')
						{
							actionName = ExtractGroupName(line);
							continue;
						}

						string[] newline = line.Split(',');
						string name = FinalizeName(newline[1]);

						string[] dats = SplitDatRange(newline[0]);

						for (int i = 0; i < dats.Length; i++)
						{
							string datpath = FinalizeDatPath(dats[i]);
							results.Add($"{raceName}|{actionName}|{name}|{datpath}");
						}
					}
				}
			}

			string fileData = String.Join("\n", results.ToArray());
			string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
			File.WriteAllText(filePath, fileData);
			File.WriteAllText(filePath + ".total", $"{results.Count}");

			ConsoleLog("- BuildAnimationList Complete!");
		}

		/// <summary>
		/// Goes through the Altana List PC folder (Specific ones) and builds a list of gear
		/// per race, including the skeleton.
		/// </summary>
		private void BuildArmorList()
		{
			string filename = $"{savepath}/gear_1.json";
			int total = 0;

			// DB Face
			string ue4FaceDB = $"{savepath}/FF11_DB_Face.csv";
			string ue4GearDB = $"{savepath}/FF11_DB_Gear.csv";

			// Already built, skip.
			if (File.Exists(filename))
			{
				return;
			}

			ConsoleLog("- BuildArmorList");

			// another blast from ChatGPT
			Dictionary<string, Dictionary<string, object>> results = new Dictionary<string, Dictionary<string, object>>();
			List<string> dbFace = new List<string>();
			List<string> dbGear = new List<string>();

			dbFace.Add("---,Name,Mesh,Race");
			dbGear.Add("---,Level,Name,Description,Slot,Mesh,Race");

			// for resource fetching
			Assembly assembly = Assembly.GetExecutingAssembly();

			// list of resources to fetch
			string[] slots =
			{
				"Body",
				"Face",
				"Feet",
				"Hands",
				"Head",
				"Legs"
			};

			// Loop through resource names by races
			foreach (string raceName in FF11Race.GetRaces())
			{
				string skeleton = FF11Race.GetSkeleton(raceName);
				string altanaRace = FF11Race.GetAltanaRace(raceName);

				ConsoleLog($"-- {raceName}");

				if (!results.ContainsKey(raceName))
				{
					results[raceName] = new Dictionary<string, object>();
				}

				// Set skeleton
				results[raceName]["Skeleton"] = skeleton;

				// Add gear dictionary
				Dictionary<string, object> gearDictionary = new Dictionary<string, object>();
				results[raceName]["Gear"] = gearDictionary;

				// loop through each slot
				foreach (string slot in slots)
				{
					string resourceName = $"FFXIBatchApp.Resources.AltanaList.PC.{altanaRace}.{slot}.csv";

					List<string> slotList = new List<string>();
					

					using (Stream stream = assembly.GetManifestResourceStream(resourceName))
					using (StreamReader reader = new StreamReader(stream))
					{
						string line;

						int gearCount = 0;
						while ((line = reader.ReadLine()) != null)
						{
							if (line.Length == 0)
							{
								continue;
							}

							string[] newline = line.Split(',');
							string name = FinalizeName(newline[1]);
							string[] dats = SplitDatRange(newline[0]);
							gearCount++;

							// The name "None" is often a keyword in game engines, we will rename to "Default".
							if (name == "None") { 
								name = "Default"; 
							}

							// Gear will always be 1
							string datpath = FinalizeDatPath(dats[0]);

							total++;
							slotList.Add($"{name}|{datpath}".Trim());

							// if face, add to
							if (name.Length > 0)
							{
								if (slot == "Face")
								{
									dbFace.Add($"{raceName}_{name},\"{name}\",\"SkeletalMesh'/Game/FF11/Characters/Gear/{raceName}/Faces/{name}/{name}.{name}'\",\"{raceName}\"");
								}
								else
								{
									dbGear.Add($"{raceName}_{slot}_{name},\"1\",\"{newline[1]}\",\"\",\"{slot}\",\"SkeletalMesh'/Game/FF11/Characters/Gear/{raceName}/{slot}/{name}/{name}.{name}'\",\"{raceName}\"");
								}
							}
						}
					}

					gearDictionary[slot] = slotList;
				}
			}

			string json = JsonConvert.SerializeObject(results, Formatting.Indented);
			string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
			File.WriteAllText(filePath, json);
			File.WriteAllText(filePath + ".total", $"{total}");

			// save unreal engine db files
			string filePathDbFace = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ue4FaceDB);
			string filePathDbGear = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ue4GearDB);
			File.WriteAllText(filePathDbFace, string.Join("\n", dbFace));
			File.WriteAllText(filePathDbGear, string.Join("\n", dbGear));

			ConsoleLog("- BuildArmorList Complete!");
		}

		/// <summary>
		/// Goes through the Altanalist and grabs all the weapons and dats, we only care about
		/// weapons for a single race and without skeletons, this is because in newer game engines
		/// you will often "socket" these to the skeleton, make's it much easier to handle.
		/// 
		/// This exports Main, Sub, and Ranged weapons
		/// </summary>
		private void BuildWeaponList()
		{
			string filename = $"{savepath}/weapons_1.json";
			int total = 0;

			// Already built, skip.
			if (File.Exists(filename))
			{
				return;
			}

			ConsoleLog("- BuildWeaponList");

			// GPT ftw
			Dictionary<string, Dictionary<string, List<string>>> results = new Dictionary<string, Dictionary<string, List<string>>>();

			// for resource fetching
			Assembly assembly = Assembly.GetExecutingAssembly();

			// list of resources to fetch
			string[] slots =
			{
				"Main",
				"Range",
				"Sub"
			};

			// loop through each slot
			foreach (string slot in slots)
			{
				ConsoleLog($"-- {slot}");
				string resourceName = $"FFXIBatchApp.Resources.AltanaList.PC.HumeM.{slot}.csv";

				using (Stream stream = assembly.GetManifestResourceStream(resourceName))
				using (StreamReader reader = new StreamReader(stream))
				{
					string line;
					string typeName = "";

					while ((line = reader.ReadLine()) != null)
					{
						if (line.Length == 0)
						{
							continue;
						}

						if (line[0] == '@')
						{
							typeName = ExtractGroupName(line);
							continue;
						}

						if (!results.ContainsKey(slot))
						{
							results[slot] = new Dictionary<string, List<string>>();
						}

						if (!results[slot].ContainsKey(typeName))
						{
							results[slot][typeName] = new List<string>();
						}

						string[] newline = line.Split(',');
						string name = FinalizeName(newline[1]);
						string[] dats = SplitDatRange(newline[0]);

						// Weapons will always be 1
						string datpath = FinalizeDatPath(dats[0]);

						total++;
						results[slot][typeName].Add($"{name}|{datpath}".Trim());
					}
				}
			}

			string json = JsonConvert.SerializeObject(results, Formatting.Indented);
			string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
			File.WriteAllText(filePath, json);
			File.WriteAllText(filePath + ".total", $"{total}");

			ConsoleLog("- BuildWeaponList Complete!");
		}

		/// <summary>
		/// Slightly different than the above, I have a list of zones in the ZoneList folder,
		/// this simply goes through and combines them into one list and restructures the name.
		/// </summary>
		private void BuildZoneList()
		{
			string filename = $"{savepath}/zones_1.json";
			int total = 0;

			// Already built, skip.
			if (File.Exists(filename))
			{
				return;
			}

			ConsoleLog("- BuildZoneList");

			// results
			Dictionary<string, List<string>> results = new Dictionary<string, List<string>>();

			// for resource fetching
			Assembly assembly = Assembly.GetExecutingAssembly();

			// list of resource names
			string[] expansions =
			{
				"Abyssea",
				"BaseGame",
				"ChainsOfPromathia",
				"Rhapsodies",
				"RizeOfZilart",
				"SeekersOfAdoulin",
				"TreasuresOfAhtUrhgan",
				"WingsOfTheGoddess"
			};

			foreach (string expansion in expansions)
			{
				ConsoleLog($"-- {expansion}");
				string resourceName = $"FFXIBatchApp.Resources.ZoneList.Zones_{expansion}.txt";

				using (Stream stream = assembly.GetManifestResourceStream(resourceName))
				using (StreamReader reader = new StreamReader(stream))
				{
					string line;

					while ((line = reader.ReadLine()) != null)
					{
						if (line.Length == 0)
						{
							continue;
						}

						if (!results.ContainsKey(expansion))
						{
							results[expansion] = new List<string>();
						}

						string[] newline = line.Split(',');
						string name = FinalizeName(newline[0]);
						string datpath = FinalizeDatPath(newline[1]);

						total++;
						results[expansion].Add($"{name}|{datpath}".Trim());
					}
				}
			}

			string json = JsonConvert.SerializeObject(results, Formatting.Indented);
			string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
			File.WriteAllText(filePath, json);

			ConsoleLog("- BuildZoneList Complete!");
			File.WriteAllText(filePath + ".total", $"{total}");
		}

		/// <summary>
		/// This is the most complex one of all. NPC Look simply refers to all the various NPCs
		/// that you see dotted about the game, the Sandoria Guards, the Limbus NPC, the Mail Delivery NPC, and so on...
		/// As these use the default games races and gear they do not have a unique DAT file and thus have a "look" string
		/// that states what they're wearing.
		/// 
		/// This has been decoded from LandBoatSea server. The SQL to get the file seen in Resources > Look Data > LookNpcDBData.csv
		/// can be found in LookQuery.sql
		/// 
		/// The LookTable.json is simply a combined list from LookDataJsons.
		/// 
		/// I want to give a HUGE thank you to these 2 people for helping with this:
		/// - xenonsmurf: For building the initial Dat JSONs: https://github.com/MurphyCodes/FFXI-DATS
		/// - StarlitGhost: For building the NPC Decoder: https://github.com/StarlitGhost/NPCDecoder
		/// 
		/// The "Look" structure can be quite complicated so I recommend checking out my Github
		/// - https://github.com/vekien/xidata/tree/main/NoesisBatchScripts/NPC2_Look
		/// 
		/// I also have a PHP decoder if that is more readable to you!
		/// 
		/// The actual decoding is in FF11LookDecoder.cs - I just have this here because all other Build functions are in this class.
		/// It does not interact with AltanaViewer Data Lists.
		/// </summary>
		private void BuildNpcLookList()
		{
			string filename = $"{savepath}/npc_2.json";
			int total = 0;

			// Already built, skip.
			if (File.Exists(filename))
			{
				return;
			}

			ConsoleLog("- BuildNpcLookList");

			// Preload the look table
			FF11LookDecoder.PreloadLookData();

			// results
			List<object> results = new List<object>();

			// for resource fetching
			Assembly assembly = Assembly.GetExecutingAssembly();

			string resourceName = $"FFXIBatchApp.Resources.LookData.LookNpcData.csv";

			using (Stream stream = assembly.GetManifestResourceStream(resourceName))
			using (StreamReader reader = new StreamReader(stream))
			{
				string line;

				while ((line = reader.ReadLine()) != null)
				{
					if (line.Length == 0)
					{
						continue;
					}

					// parse the npc database csv entry
					string[] npcdata = ExtractLookDataCsv(line);

					// decode the npc look data
					Dictionary<string, int> lookdata = FF11LookDecoder.DecodeLook(npcdata[6]);
					Dictionary<string, string> looknpc = new Dictionary<string, string>();

					// add the look data labeled
					looknpc["ID"] = npcdata[0];
					looknpc["Name"] = npcdata[1];
					looknpc["UEID"] = $"{npcdata[0]}_{FinalizeName(npcdata[1])}";
					looknpc["PosRot"] = npcdata[2];
					looknpc["Content"] = npcdata[7];
					looknpc["Zone"] = FinalizeName(npcdata[8]);

					// this is intentionally 3,5,4 because for some reason LandSandBoat (and I assume the XI engine) uses Y for up/down but 
					// most 3d packages and game engines these days use Z for up/down... So I've intentionally swapped these so they match
					// better in a game engine as well as things like Windower.
					looknpc["PosX"] = npcdata[3];
					looknpc["PosY"] = npcdata[5];
					looknpc["PosZ"] = npcdata[4];
					

					if (looknpc["Name"].Length == 0)
					{
						looknpc["Name"] = "Unknown";
					}

					// if we have no race, skip
					if (lookdata["Race"] == 0)
					{
						continue;
					}

					// If the race is outside 1-8 (the main races) we'll skip for now
					if (lookdata["Race"] < 1 || lookdata["Race"] > 8)
					{
						continue;
					}

					// Grab the correct race
					string racedata = FF11Race.GetRacesLook()[lookdata["Race"]];
					string racename = racedata.Split('|')[0];

					// add race omfp
					looknpc["Race"] = racedata;

					// find the models!
					List<string> slots = new List<string>() { "Face", "Head", "Body", "Hands", "Legs", "Feet", "Main", "Sub", "Range" };

					foreach (string slot in slots)
					{
						int slotModelId = lookdata[slot];

						string[] modelData = FF11LookDecoder.GetModelDataFromModelId(racename, slot, slotModelId);

						if (modelData == null)
						{
							looknpc[slot] = "";
							continue;
						}

						string fileId = modelData[0];
						string modelID = modelData[1];
						string path = FinalizeDatPath(modelData[2]);

						looknpc[slot] = $"{fileId}|{modelID}|{path}";
					}

					total++;
					results.Add(looknpc);
				}
			}

			// save
			string json = JsonConvert.SerializeObject(results, Formatting.Indented);
			string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
			File.WriteAllText(filePath, json);
			File.WriteAllText(filePath + ".total", $"{total}");

			ConsoleLog("- BuildNpcLookList Complete!");
		}

		///---------------------------------------------------------------------------------------------------------

		/// <summary>
		/// This function handles the various weird dat ranges in the AltanaView
		/// </summary>
		/// <param name="line"></param>
		/// <returns></returns>
		private string[] SplitDatRange(string line)
		{
			var segments = line.Split(';');
			var dats = new List<string>();

			foreach (var seg in segments)
			{
				//
				// X/Y-Z
				//
				if (seg.Contains("/") && seg.Contains("-"))
				{
					var parts = seg.Split('/');

					if (parts.Length == 3)
					{
						var rom = parts[0];
						var first = parts[1];
						var range = parts[2].Split('-');

						foreach (var i in Enumerable.Range(int.Parse(range[0]), int.Parse(range[1]) - int.Parse(range[0]) + 1))
						{
							dats.Add($"{rom}/{first}/{i}");
						}

					}
					else
					{
						var first = parts[0];
						var range = parts[1].Split('-');

						foreach (var i in Enumerable.Range(int.Parse(range[0]), int.Parse(range[1]) - int.Parse(range[0]) + 1))
						{
							dats.Add($"{first}/{i}");
						}
					}
				}

				//
				// X/Y
				//
				if (seg.Contains("/") && !seg.Contains("-"))
				{
					dats.Add($"{seg}");
				}

				//
				// Y-Z
				//
				if (!seg.Contains("/") && seg.Contains("-"))
				{
					var first = segments[0].Split('/')[0];
					var range = seg.Split('-');

					foreach (var i in Enumerable.Range(int.Parse(range[0]), int.Parse(range[1]) - int.Parse(range[0]) + 1))
					{
						dats.Add($"{first}/{i}");
					}
				}
			}

			// Replace "1/" with "/" for ROM 1
			for (int i = 0; i < dats.Count; i++)
			{
				if (dats[i].StartsWith("1/"))
				{
					dats[i] = "/" + dats[i].Substring(2);
				}
			}

			return dats.ToArray();
		}

		/// <summary>
		/// This simply does a quick check for slashes and also converts all / to \\
		/// </summary>
		/// <param name="datpath"></param>
		/// <returns></returns>
		private string FinalizeDatPath(string datpath)
		{
			// Remove ROM and .DAT from any paths
			datpath = datpath.Replace("ROM", "").Replace(".DAT", "");

			// Puts a slashes infront of ROM 1 (which has no number)
			if (datpath.Count(c => c == '/') < 2)
			{
				datpath = "\\" + datpath;
			}

			datpath = datpath.Replace("/", "\\");

			return datpath;
		}

		/// <summary>
		/// This converts names into more filename safe format with just A-Z, Numbers and _ for spaces.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		private string FinalizeName(string name)
		{
			return Regex.Replace(name.Trim(), "[^a-zA-Z0-9-_ ]+", "").Replace(" ", "_").Replace("__", "_");
		}

		/// <summary>
		/// Extracts the group name and formats it.
		/// </summary>
		/// <param name="groupName"></param>
		/// <returns></returns>
		private string ExtractGroupName(string groupName)
		{
			groupName = groupName.Substring(1);
			groupName = groupName.Replace(" ", "_");
			groupName = groupName.Replace(".", "_");
			groupName = groupName.Replace(":", "");
			groupName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(groupName);

			return FinalizeName(groupName);
		}

		/// <summary>
		/// Parses a CSV line for the extract look data
		/// </summary>
		/// <param name="lookcsvline"></param>
		/// <returns></returns>
		private string[] ExtractLookDataCsv(string lookcsvline)
		{
			using (TextFieldParser parser = new TextFieldParser(new StringReader(lookcsvline)))
			{
				parser.TextFieldType = FieldType.Delimited;
				parser.SetDelimiters(",");
				parser.HasFieldsEnclosedInQuotes = true;

				while (!parser.EndOfData)
				{
					return parser.ReadFields();
				}
			}

			return null;
		}
	}
}
