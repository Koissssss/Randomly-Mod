using Randomly.content.tiles.banners;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Randomly.content.items.placeable.banners
{
	public class Banners_Defaults
	{
		public static void CopyDefaults(Item item, int bannerIndex){
			item.width = 10;
			item.height = 24;
			item.maxStack = 9999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.Swing;
			item.consumable = true;
			item.rare = ItemRarityID.Blue;
			item.value = Item.buyPrice(0, 0, 10, 0);
			item.createTile = ModContent.TileType<MonsterBanners>();
			item.placeStyle = bannerIndex;
		}
	}
}
