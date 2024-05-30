using Randomly.content.items.placeable.blocks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Randomly.content.items.weapons.swords
{
    public class Sword_PickledCucumber : ModItem
    {
		public override void SetDefaults() {
			Item.width = 40; // The item texture's width.
			Item.height = 40; // The item texture's height.
			Item.scale = 2;
			
			Item.useStyle = ItemUseStyleID.Swing; // The useStyle of the Item.
			Item.useTime = 15; // The time span of using the weapon. Remember in terraria, 60 frames is a second.
			Item.useAnimation = 15; // The time span of the using animation of the weapon, suggest setting it the same as useTime.
			Item.autoReuse = true; // Whether the weapon can be used more than once automatically by holding the use button.

			Item.DamageType = DamageClass.Melee; // Whether your item is part of the melee class.
			Item.damage = 12; // The damage your item deals.
			Item.knockBack = 4; // The force of knockback of the weapon. Maximum is 20

			Item.value = Item.buyPrice(silver: 10); // The value of the weapon in copper coins.
			Item.UseSound = SoundID.Item1; // The sound when the weapon is being used.
		}
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<Block_Cucumber>(40)
				.Register();
		}
    }
}