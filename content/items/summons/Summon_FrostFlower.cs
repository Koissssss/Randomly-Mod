using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.Audio;
using Randomly.content.npcs.bosses.frostFlower;
namespace Randomly.content.items.summons
{
    public class Summon_FrostFlower : ModItem
    {
		public override void SetStaticDefaults() {
			ItemID.Sets.SortingPriorityBossSpawns[Type] = 12;
		}
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.Blue;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
        }
        public override bool CanUseItem(Player player)
        {
            return !NPC.AnyNPCs(ModContent.NPCType<Boss_FrostFlower>()) && player.ZoneSnow;
        }
        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                SoundEngine.PlaySound(SoundID.Roar, player.position);//播放吼叫音效
                int type = ModContent.NPCType<Boss_FrostFlower>();
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.SpawnOnPlayer(player.whoAmI, type);//生成Boss
                }
                else
                {
                    NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: type);//发包，用来联机同步
                }
            }
            return true;
        }
		public override void AddRecipes(){
			CreateRecipe()
				.AddIngredient(ItemID.IceBlock, 20)
				.AddIngredient(ItemID.SnowBlock, 20)
				.AddIngredient(ItemID.Shiverthorn, 3)
				.Register();
		}
    }
}