# DiscordSkillTracker

Automatically posts skill levelup events and details to your discord server.

## Features

- Posts level up events and details to your discord server via webhook.
- Ranks players skill proficiency (Novice, Advanced Beginner, Competent, Proficient, Expert, and Master)
- Discord message color coded according to proficiency.
- Built in support for most custom skills on the thunderstore.
- Easily add support for new skills/IDs in the customSkills.json

Currently DiscordSkillTracker only needs to be installed on the client, however everyone on your server should be using the same config files. I'm looking into ServerSync, however the mod functions basically the same without it.

## Installation
Copy the plugin and config folders into your 'Valheim/BepinEx' directory. If you're updating, be sure to overwrite any existing files.

## Adding custom skills
Many custom skills are already supported, however it's easy to add support in the case that you have one that isn't.
Inside your Valheim/BepinEx/plugins/DiscordSkillTracker folder is a 'customSkills.json'. Simply follow this format and add the ID that posts to your discord, along with the name of the skill and save the file.

Current skills identified in 'customSkills.json'
```json
{
  "184859086":"Cooking",
  "1363793286":"Lumberjacking",
  "1408976878":"Mining",
  "1555733581":"Gathering",
  "839129794":"Farming",
  "705858871":"Alchemy",
  "001":"Sailing",
  "002":"Jewelcrafting",
  "276088460":"Ranching",
  "757121602":"Building",
  "352670":"Wagon",
  "1208107160":"Blacksmithing",
  "638":"Vitality",
  "1443093080":"Packhorse",
  "94909903":"Evasion",
  "1612888259":"Tenacity",
  "1337":"Cartograpgy",
  "003":"Artifact",
  "781":"Discapline",
  "792":"Alteration",
  "794":"Evocation",
  "795":"Illusion",
  "791":"Abjuration",
  "793":"Conjuration",
  "200":"Nature Magic",
  "202":"Fire Magic",
  "203":"Frost Magic",
  "201":"Holy Magic",
  "4982908":"Duel Swords",
  "3981298":"Duel Clubs",
  "409822":"Duel Knives",
  "874298":"Duel Axes",
  "99904":"Runic Wizard",
  "99902":"Runic Cleric",
  "99903":"Runic Rogue",
  "99901":"Runic Warrior"
}
```

## Proficiencys

This has no effect on gameplay, it's merely to give players more milestones to hit. Each discord post will display the players proficiency in the skill they leveled.

| Proficieny | Level Req |
| ------ | ------ |
| Novice | 1-15 |
| Advanced Beginner | 16-30 |
| Competent | 31-50 |
| Proficient | 51-70 |
| Expert | 71-90 |
| Master | 90+ |

## Contact
discord info coming soon

## Credit
negrifelipe/DiscordMessenger (https://github.com/negrifelipe/DiscordMessenger)

And a very special thanks to OrianaVenture for all her help and patience!
