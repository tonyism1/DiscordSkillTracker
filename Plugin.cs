using BepInEx;
using DiscordMessenger;
using System;
using HarmonyLib;
using BepInEx.Configuration;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace DiscordSkillTracker
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]

    public class SkillTracker : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("com.tonyism1.discordSkillTracker");
        public const string PluginGUID = "com.tonyism1.discordSkillTracker";
        public const string PluginName = "DiscordSkillTracker";
        public const string PluginVersion = "0.0.1";

        public static ConfigEntry<string> webhookAddress = null!;
        public static ConfigEntry<string> botAvatar = null!;
        public static ConfigEntry<string> botName = null!;

        static string webhookAddress1 = "https://discord.com/api/webhooks/1016088172339396629/rihfgzKDBcI7i_bKsiA5GUeJ34FnOMTZlxZ6Wj7Obqv7sBKdJzpW19AdEyTjn2szU84T";
        static string botAvatar1 = "https://thumbs.dreamstime.com/b/scandinavian-viking-design-ancient-decorative-dragon-celtic-style-knot-work-illustration-northern-runes-vector-214616877.jpg";
        static string botName1 = "Odinbot";

        static string jsonStringLocal;

        private void Awake()
        {
            harmony.PatchAll();
            LoadJson();
        }

        public static string RemoveSpecialChars(string str)
        {
            string[] chars = new string[] { "'", "\"", ":", " "};
            for (int i = 0; i < chars.Length; i++)
            {
                if (str.Contains(chars[i])) {str = str.Replace(chars[i], "");}
            }
            return str;
        }

        public void LoadJson()
        {
            StreamReader r = new StreamReader("BepInEx/plugins/DiscordSkillTracker/moddedSkills.json");
            string jsonString = r.ReadToEnd();
            jsonStringLocal = jsonString;
            Logger.LogInfo($"JSON LOADED XX");
        }

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
                    dynamic array = JsonConvert.DeserializeObject(jsonStringLocal);
                    foreach (var item in array)
                    {
                        string[] words = item.ToString().Split(':');
                        string _a = RemoveSpecialChars(words[0]);
                        string _b = RemoveSpecialChars(words[1]);

                        if (skillName == _a) {skillName = _b;}
                    }
                }

                // Send message to discord
                new DiscordMessage()
                    .SetUsername(botName1)
                    .SetAvatar(botAvatar1)
                    .AddEmbed()
                        .SetTimestamp(DateTime.Now)
                        .SetTitle("Skill Increase!")
                        .SetDescription($"**{playerName}** has grown a {skillName} level!  [{skillName}: {skillLevel}]")
                        .SetColor(14177041)
                        .SetFooter("")
                        .Build()
                        .SendMessage(webhookAddress1);
            }
        }
    }
}