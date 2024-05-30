using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Randomly.content.items.armors.vanitys
{
	[AutoloadEquip(EquipType.Head)]
	public class Vanity_FrostFlowerMask : ModItem
	{
		public override void SetDefaults() {
			Item.width = 22;
			Item.height = 22;

			// Common values for every boss mask
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(silver: 75);
			Item.vanity = true;
			Item.maxStack = 1;
		}
	}
}
