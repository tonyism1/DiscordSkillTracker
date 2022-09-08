using BepInEx;
using DiscordMessenger;
using System;
using HarmonyLib;
using BepInEx.Configuration;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;


namespace DiscordSkillTracker
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]


    public class SkillTracker : BaseUnityPlugin
    {

        private const string PluginGUID = "com.tonyism1.discordSkillTracker";
        private const string PluginName = "DiscordSkillTracker";
        private const string PluginVersion = "1.0.0";

        private static Harmony harmony = new(PluginGUID);

        private static string ConfigFileName = PluginGUID + ".cfg";
        private static string ConfigFileFullPath = Paths.ConfigPath + Path.DirectorySeparatorChar + ConfigFileName;

        private static ConfigEntry<string> webhookAddress = null!;
        private static ConfigEntry<string> botAvatar = null!;
        private static ConfigEntry<string> botName = null!;

        private static dynamic customSkillList;



        private void Awake()
        {
            loadJson();

            #region Configuration

            AddConfig("Webhook URL:", "General", "webhookAddresss",
                true, " ", ref webhookAddress);
            AddConfig("Bot Avatar URL:", "General", "botAvatar",
                true, " ", ref botAvatar);
            AddConfig("Bot Name:", "General", "botName",
                true, " ", ref botName);

            #endregion

            Assembly assembly = Assembly.GetExecutingAssembly();
            harmony.PatchAll(assembly);
            SetupWatcher();

            Logger.LogInfo($"       XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
            Logger.LogInfo($"       XXX                                                          ");
            Logger.LogInfo($"       XXX                  DiscordSkillTracker                     ");
            Logger.LogInfo($"       XXX                    Loaded!  v{PluginVersion}             ");
            Logger.LogInfo($"       XXX                                                          ");
            Logger.LogInfo($"       XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
        }

        private void OnDestroy()
        {
            Config.Save();
            harmony.UnpatchSelf();
        }




        private void AddConfig<T>(string key, string section, string description, bool synced, T value, ref ConfigEntry<T> configEntry)
        {
            string extendedDescription = GetExtendedDescription(description, synced);
            configEntry = Config.Bind(section, key, value, extendedDescription);
        }

        public string GetExtendedDescription(string description, bool synchronizedSetting) { return description + (synchronizedSetting ? " [Synced with Server]" : " [Not Synced with Server]"); }
        
        private void SetupWatcher()
        {
            FileSystemWatcher watcher = new(Paths.ConfigPath, ConfigFileName);
            watcher.Changed += ReadConfigValues;
            watcher.Created += ReadConfigValues;
            watcher.Renamed += ReadConfigValues;
            watcher.IncludeSubdirectories = true;
            watcher.SynchronizingObject = ThreadingHelper.SynchronizingObject;
            watcher.EnableRaisingEvents = true;
        }

        private void ReadConfigValues(object sender, FileSystemEventArgs e)
        {
            if (!File.Exists(ConfigFileFullPath)) return;
            try
            {
                Logger.LogInfo("    -->  Discord Skill Tracker is reloading the config file!");
                Config.Reload();
                Logger.LogInfo("    -->  Reload successful!");
            }
            catch
            {
                Logger.LogError("    -->  Discord Skill Tracker could not reload config file!");
            }
        }



        public void loadJson()
        {
            StreamReader r = new StreamReader("BepInEx/plugins/DiscordSkillTracker/customSkills.json");
            string jsonString = r.ReadToEnd();
            customSkillList = JsonConvert.DeserializeObject(jsonString);
            Logger.LogInfo("    -->  Discord Skill Tracker has loaded 'customSkills.json' !");
        }

        public static string stripChars(string str)
        {
            string[] chars = new string[] { "'", "\"", ":", " " };
            for (int i = 0; i < chars.Length; i++)
            {
                if (str.Contains(chars[i])) { str = str.Replace(chars[i], ""); }
            }
            return str;
        }

        public static async Task pushDiscord(string pn, string skn, float skl)
        {
            await Task.Run(() =>
            {

                var rnk = "";
                int col = 2303786;
                if (skl > 90) { rnk = "Master"; col = 15277667; }
                else if (skl > 70) { rnk = "Expert"; col = 11342935; }
                else if (skl > 50) { rnk = "Proficient"; col = 10181046; }
                else if (skl > 30) { rnk = "Competent"; col = 3447003; }
                else if (skl > 15) { rnk = "Advanced Beginner"; col = 5763719; }
                else { rnk = "Novice"; col = 16776960; }

                new DiscordMessage()
                    .SetUsername(botName.Value)
                    .SetAvatar(botAvatar.Value)
                    .AddEmbed()
                        .SetTimestamp(DateTime.Now)
                        .SetTitle("Skill Increase!")
                        .SetDescription($"**{pn}** has grown a {skn} level!  [{skn}: {skl}]")
                        .SetColor(col)
                        .SetFooter($"Rank: {rnk}")
                        .Build()
                        .SendMessage(webhookAddress.Value);
                Task.Delay(100).Wait();

            });
        }


        /// <summary>
        /// 
        /// </summary>


        [HarmonyPatch(typeof(Player), nameof(Player.OnSkillLevelup))]
        public static class Patch_Player_OnSkillLevelup
        {
            private static void Prefix(Player __instance, Skills.SkillType skill, float level)
            {
                var playerName = __instance.GetPlayerName();
                var skillName = skill.ToString();
                var skillLevel = (float)Math.Floor(level);

                // Check if skillname is a digit
                if (skillName.All(char.IsDigit))
                {
                    foreach (var item in customSkillList)
                    {
                        string[] words = item.ToString().Split(':');
                        string _id = stripChars(words[0]);
                        string _name = stripChars(words[1]);

                        if (skillName == _id) { skillName = _name; break; }
                    }
                }

                pushDiscord(playerName, skillName, skillLevel).ConfigureAwait(false);
            }
        }




    }
}