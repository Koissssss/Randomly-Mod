//TODO: 与黑曜石玫瑰合成
//TODO: 人物佩戴外貌

using Randomly.content.items.materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Randomly.content.items.accessories
{
	public class Accessorie_SnowFlower : ModItem
	{
		public override void SetDefaults() {
			Item.width = 40;
			Item.height = 40;
			Item.accessory = true;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.buyPrice(0, 5, 0, 0);
		}
		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.buffImmune[BuffID.Chilled] = true;
		}
		public override void AddRecipes(){
			CreateRecipe()
				.AddIngredient(ItemID.Shiverthorn)
				.AddIngredient(ItemID.IceBlock, 20)
				.AddIngredient<Material_SoulyIce>(10)
				.AddTile(TileID.IceMachine)
				.Register();
		}
	}
}
