using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace Randomly.common.systems
{
	// Showcases using Mod.Call of other mods to facilitate mod integration/compatibility/support
	// Mod.Call is explained here https://github.com/tModLoader/tModLoader/wiki/Expert-Cross-Mod-Content#call-aka-modcall-intermediate
	// This only showcases one way to implement such integrations, you are free to explore your own options and other mods examples

	// You need to look for resources the mod developers provide regarding how they want you to add mod compatibility
	// This can be their homepage, workshop page, wiki, GitHub, Discord, other contacts etc.
	// If the mod is open source, you can visit its code distribution platform (usually GitHub) and look for "Call" in its Mod class

	// In addition to the examples shown here, ExampleMod also integrates with the Census Mod (https://steamcommunity.com/sharedfiles/filedetails/?id=2687866031)
	// That integration is done solely through localization files, look for "Census.SpawnCondition" in the .hjson files. 
	public class System_BossList : ModSystem
	{
		public override void PostSetupContent() {
			// Most often, mods require you to use the PostSetupContent hook to call their methods. This guarantees various data is initialized and set up properly

			// Boss Checklist shows comprehensive information about bosses in its own UI. We can customize it:
			// https://forums.terraria.org/index.php?threads/.50668/
			DoBossChecklistIntegration();

			// We can integrate with other mods here by following the same pattern. Some modders may prefer a ModSystem for each mod they integrate with, or some other design.
		}

		private void DoBossChecklistIntegration() {
			if (!ModLoader.TryGetMod("BossChecklist", out Mod bossChecklistMod)) {
				return;
			}
			if (bossChecklistMod.Version < new Version(1, 6)) {
				return;
			}
			

			AddBoss("冰霜花灵", 0.8f,
					System_DownedBossSystem.DownedBoss_FrostFlower,
					ModContent.NPCType<content.npcs.bosses.frostFlower.Boss_FrostFlower>(),
					ModContent.ItemType<content.items.summons.Summon_FrostFlower>(),
					ModContent.Request<Texture2D>("Randomly/Assets/Textures/BossList/BossList_FrostFlower"),
					bossChecklistMod);


			//TODO: 改一下这逼玩意
			AddBoss("宇宙碎片", 1.75f,
					System_DownedBossSystem.DownedBoss_SpacePiece,
					ModContent.NPCType<content.npcs.bosses.spacePiece.Boss_SpacePiece>(),
					ModContent.ItemType<content.items.summons.Summon_FrostFlower>(),
					ModContent.Request<Texture2D>("Randomly/Assets/Textures/BossList/BossList_FrostFlower"),
					bossChecklistMod);
		}



		private void AddBoss(string internalName, float weight, bool downed, int bossType, int spawnItem, Asset<Texture2D> asset, Mod bossChecklistMod){
			Func<bool> downedFunc = () => downed;
			var customPortrait = (SpriteBatch sb, Rectangle rect, Color color) => {
				Texture2D texture = asset.Value;
				Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
				sb.Draw(texture, centered, color);
			};

			bossChecklistMod.Call(
				"LogBoss",
				Mod,
				internalName,
				weight,
				downedFunc,
				bossType,
				new Dictionary<string, object>() {
					["spawnItems"] = spawnItem,
					["customPortrait"] = customPortrait
				}
			);
			/*
			string internalName = "冰霜花灵";
			float weight = 0.8f;
			Func<bool> downed = () => System_DownedBossSystem.DownedBoss_FrostFlower;
			int bossType = ModContent.NPCType<content.npcs.bosses.frostFlower.Boss_FrostFlower>();
			int spawnItem = ModContent.ItemType<content.items.summons.Summon_FrostFlower>();
			var customPortrait = (SpriteBatch sb, Rectangle rect, Color color) => {
				Texture2D texture = ModContent.Request<Texture2D>("Randomly/Assets/Textures/BossList/BossList_FrostFlower").Value;
				Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
				sb.Draw(texture, centered, color);
			};

			bossChecklistMod.Call(
				"LogBoss",
				Mod,
				internalName,
				weight,
				downed,
				bossType,
				new Dictionary<string, object>() {
					["spawnItems"] = spawnItem,
					["customPortrait"] = customPortrait
				}
			);
			*/
		}
	}
}
/*
KingSlime = 1f;
TorchGod = 1.5f;
EyeOfCthulhu = 2f;
BloodMoon = 2.5f;
EaterOfWorlds = 3f;
GoblinArmy = 3.33f;
OldOnesArmy = 3.66f;
DarkMage = 3.67f;
QueenBee = 4f;
Skeletron = 5f;
DeerClops = 6f;
WallOfFlesh = 7f;
FrostLegion = 7.33f;
PirateInvasion = 7.66f;
PirateShip = 7.67f;
QueenSlime = 8f;
TheTwins = 9f;
TheDestroyer = 10f;
SkeletronPrime = 11f;
Ogre = 11.01f;
SolarEclipse = 11.5f;
Plantera = 12f;
Golem = 13f;
PumpkinMoon = 13.25f;
MourningWood = 13.26f;
Pumpking = 13.27f;
FrostMoon = 13.5f;
Everscream = 13.51f;
SantaNK1 = 13.52f;
IceQueen = 13.53f;
MartianMadness = 13.75f;
MartianSaucer = 13.76f;
DukeFishron = 14f;
EmpressOfLight = 15f;
Betsy = 16f;
LunaticCultist = 17f;
LunarEvent = 17.01f;
Moonlord = 18f;
*/