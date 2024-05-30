using Randomly.content.tiles.banners;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Randomly.content.items.placeable.banners
{
	public class Banner_FloatCucumber : ModItem
	{
		public override void SetDefaults() {
			Banners_Defaults.CopyDefaults(Item, 0);
		}
	}
}
