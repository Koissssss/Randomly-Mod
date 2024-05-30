using Microsoft.Xna.Framework;
using Randomly.common.systems;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ModLoader;

namespace Randomly.content.biomes
{
	// Shows setting up two basic biomes. For a more complicated example, please request.
	public class Biome_CucumberStar : ModBiome
	{
		//public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.GetInstance<Background_CucumberStar>();
		public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Jungle;
		public override int BiomeTorchItemType => base.BiomeTorchItemType;
		public override int BiomeCampfireItemType => base.BiomeCampfireItemType;

		// Populate the Bestiary Filter
		public override string BestiaryIcon => base.BestiaryIcon;
		//public override string BackgroundPath => base.BackgroundPath;
		public override Color? BackgroundColor => new Color(177, 227, 167);
		public override string MapBackground => BackgroundPath; // Re-uses Bestiary Background for Map Background
		

		//public new ModBiomeBestiaryInfoElement ModBiomeBestiaryInfoElement =  ;
		// Calculate when the biome is active.
		public override bool IsBiomeActive(Player player) {
			bool b1 = ModContent.GetInstance<System_CucumberTileCount>().CucumberTileCount >= 100;
			bool b2 = player.position.Y/16 < Main.worldSurface * 0.45;
			return b1 && b2;
		}
	}
}