using BepInEx;
using DiscordMessenger;
using System;
using HarmonyLib;
using static Skills;
using BepInEx.Configuration;

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

        private void Awake()
        {
            harmony.PatchAll();
        }

        [HarmonyPatch(typeof(Player), nameof(Player.OnSkillLevelup))]
        public static class Patch_Player_OnSkillLevelup
        {
            private static void Prefix(Player __instance, Skills.SkillType skill, float level)
            {
                var playerName = __instance.GetPlayerName();
                var skillName = skill.ToString();

                if (skillName == "1555733581") { skillName = "Gathering"; }
                if (skillName == "1408976878") { skillName = "Mining"; }
                if (skillName == "184859086") { skillName = "Cooking"; }
                if (skillName == "1363793286") { skillName = "Lumberjacking"; }
                
                var skillLevel = (float)Math.Floor(level);

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