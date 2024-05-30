using Terraria;
using Terraria.ModLoader;

namespace Randomly.content.items.placeable.trophys
{
	public class Trophy_FrostFlower : ModItem
	{
		private readonly int tileType = ModContent.TileType<tiles.trophys.Trophy_FrostFlower>();

		public override void SetDefaults() {
            Trophys_Defaults.CopyDefaults(Item, tileType);
		}
	}
}
