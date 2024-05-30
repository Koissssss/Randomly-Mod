using Randomly.content.items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Randomly.content.pets.Pet_FrostFlower
{
	public class Pet_FrostFlower_Item : ModItem
	{
		// Names and descriptions of all ExamplePetX classes are defined using .hjson files in the Localization folder
		public override void SetDefaults() {
			Item.CloneDefaults(ItemID.ZephyrFish); // Copy the Defaults of the Zephyr Fish Item.

			Item.width = 22;
			Item.height = 22;
			Item.rare = ItemRarityID.Master;
			Item.master = true;
			Item.value = Item.sellPrice(0, 5);

			Item.shoot = ModContent.ProjectileType<Pet_FrostFlower_Projectile>(); // "Shoot" your pet projectile.
			Item.buffType = ModContent.BuffType<Pet_FrostFlower_Buff>(); // Apply buff upon usage of the Item.
		}

        public override bool? UseItem(Player player)
        {
			if (player.whoAmI == Main.myPlayer) {
				player.AddBuff(Item.buffType, 3600);
			}
   			return true;
		}
	}
}
