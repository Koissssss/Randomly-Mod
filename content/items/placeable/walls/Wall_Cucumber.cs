using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Randomly.content.items.placeable.walls
{
	public class Wall_Cucumber : ModItem
	{
		public override void SetDefaults() {
			// ModContent.WallType<Walls.ExampleWall>() retrieves the id of the wall that this item should place when used.
			// DefaultToPlaceableWall handles setting various Item values that placeable wall items use.
			// Hover over DefaultToPlaceableWall in Visual Studio to read the documentation!
			Item.DefaultToPlaceableWall(ModContent.WallType<content.walls.Wall_Cucumber>());
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe(4)
				.AddIngredient<blocks.Block_Cucumber>()
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}
