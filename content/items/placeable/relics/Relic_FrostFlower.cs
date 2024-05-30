using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Randomly.content.items.placeable.relics
{
	public class Relic_FrostFlower : ModItem
	{
		private readonly int tileType = ModContent.TileType<tiles.relics.Relic_FrostFlower>();
		private const int WIDTH = 30;
		private const int HEIGHT = 40;
		public override void SetDefaults() {
			Relics_Defaults.CopyDefaults(Item, tileType, WIDTH, HEIGHT);
		}
	}
}
