using Terraria;
using Terraria.ID;

namespace Randomly.content.items.placeable.relics
{
	public class Relics_Defaults
	{
		public static void CopyDefaults(Item item, int tileType, int width, int height) {
			item.DefaultToPlaceableTile(tileType, 0);

			item.width = width;
			item.height = height;
			item.rare = ItemRarityID.Master;
			item.master = true;
		}
	}
}
