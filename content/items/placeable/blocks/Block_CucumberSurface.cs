using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Randomly.content.items.placeable.blocks
{
	public class Block_CucumberSurface : ModItem
	{
		public override void SetDefaults() {
			Item.DefaultToPlaceableTile(ModContent.TileType<tiles.blocks.Block_CucumberSurface>());
			Item.width = 12;
			Item.height = 12;
        }
    }
}
