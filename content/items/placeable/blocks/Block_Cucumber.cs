using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Randomly.content.items.placeable.blocks
{
	public class Block_Cucumber : ModItem
	{
		public override void SetDefaults() {
			Item.DefaultToPlaceableTile(ModContent.TileType<tiles.blocks.Block_Cucumber>());
			Item.width = 12;
			Item.height = 12;
		}

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<walls.Wall_Cucumber>(4)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}
