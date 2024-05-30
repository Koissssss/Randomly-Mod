using Terraria;
using Terraria.ID;

namespace Randomly.content.items.placeable.trophys
{
	public class Trophys_Defaults
	{
		public static void CopyDefaults(Item Item, int tileType) {
			Item.DefaultToPlaceableTile(tileType);
			Item.width = 32;
			Item.height = 32;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.buyPrice(0, 1);
		}
	}
}
