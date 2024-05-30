using Randomly.content.items.armors.vanitys;
using Randomly.content.items.materials;
using Randomly.content.items.weapons.magics;
using Randomly.content.items.weapons.yoyos;
using Randomly.content.npcs.bosses.frostFlower;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Randomly.content.items.consumables.bossbags
{
	// Basic code for a boss treasure bag
	public class Bossbag_FrostFlower : ModItem
	{
		public override void SetStaticDefaults() {
			// This set is one that every boss bag should have.
			// It will create a glowing effect around the item when dropped in the world.
			// It will also let our boss bag drop dev armor..
			ItemID.Sets.BossBag[Type] = true;
			ItemID.Sets.PreHardmodeLikeBossBag[Type] = true; // ..But this set ensures that dev armor will only be dropped on special world seeds, since that's the behavior of pre-hardmode boss bags.
		}

		public override void SetDefaults(){
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.width = 24;
			Item.height = 24;
			Item.rare = ItemRarityID.Purple;
			Item.expert = true;
		}

		public override bool CanRightClick() {
			return true;
		}

		public override void ModifyItemLoot(ItemLoot itemLoot) {
			//TODO:添加武器掉落

    		itemLoot.Add(ItemDropRule.FewFromOptions(1, 1, [ModContent.ItemType<Magic_IcyBurst>(), ModContent.ItemType<Yoyo_IceYoyo>()]));
			itemLoot.Add(ItemDropRule.NotScalingWithLuck(ModContent.ItemType<Vanity_FrostFlowerMask>(), 7));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Material_SoulyIce>(), 1, 40, 50));
			itemLoot.Add(ItemDropRule.CoinsBasedOnNPCValue(ModContent.NPCType<Boss_FrostFlower>()));
		}
	}
}