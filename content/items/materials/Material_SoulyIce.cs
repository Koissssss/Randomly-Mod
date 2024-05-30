//TODO: 多整点合成
using Terraria.ID;
using Terraria.ModLoader;

namespace Randomly.content.items.materials
{
    public class Material_SoulyIce : ModItem
    {
        public override void SetDefaults()
        {
			Item.rare = ItemRarityID.Blue;
            Item.maxStack = 9999;
        }
    }
}