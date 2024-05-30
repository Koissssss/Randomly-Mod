using System;
using Microsoft.Xna.Framework;
using Randomly.content.biomes;
using Randomly.content.dusts;
using Randomly.content.items.placeable.banners;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace Randomly.content.npcs.enemys.cucumberStar
{
    public class Enemy_FloatCucumber : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 1;

			NPCID.Sets.NPCBestiaryDrawModifiers value = new() { // Influences how the NPC looks in the Bestiary
				Velocity = 1f // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);

        }
        public override void SetDefaults() {
            NPC.width = 50;
            NPC.height = 50;
            NPC.damage = 15;
            NPC.lifeMax = 25;
            NPC.defense = 0;
            NPC.knockBackResist = 0.75f;
            NPC.aiStyle = -1;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = Item.buyPrice(0, 0, 0, 50);
            NPC.noGravity = true;
            NPC.noTileCollide = true;

			Banner = NPC.type;
            BannerItem = ModContent.ItemType<Banner_FloatCucumber>();
			SpawnModBiomes = [ModContent.GetInstance<Biome_CucumberStar>().Type];
		}
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if(spawnInfo.Player.InModBiome(ModContent.GetInstance<Biome_CucumberStar>())){
                return 100f;
            }
            else{
                return 0f;
            }
        }
            
        //设置图鉴内信息
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
			bestiaryEntry.Info.AddRange([
                //设置所属环境，一般填写他最喜爱的环境
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
                //图鉴内描述
                new FlavorTextBestiaryInfoElement("一个会飞的黄瓜片儿"),
				new BestiaryPortraitBackgroundProviderPreferenceInfoElement(ModContent.GetInstance<Biome_CucumberStar>().ModBiomeBestiaryInfoElement)
            ]);
        }
		public override void HitEffect(NPC.HitInfo hit) {

			for (int i = 0; i < 10; i++) {
				int dustType = ModContent.DustType<Dust_CucumberPiece>();
				var dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, dustType);

				dust.velocity.X += Main.rand.NextFloat(-0.05f, 0.05f);
				dust.velocity.Y += Main.rand.NextFloat(-0.05f, 0.05f);

				dust.scale *= 1f + Main.rand.NextFloat(-0.03f, 0.03f);
			}
		}
        public override void AI()
        {
            NPC.TargetClosest(true);
            Player Tar = Main.player[NPC.target];
            float Speed = NPC.velocity.Length();
            if(Speed >= 0.04f){
                float Friction = (Speed - 0.04f) / Speed;
                NPC.velocity = Friction * NPC.velocity;
            }
            else{
                NPC.velocity *= 0;
            }
            float CurSpeed = NPC.velocity.Length();
            Vector2 IncSpeed = 0.2f* FuncSpeed(CurSpeed)*(Tar.Center - NPC.Center).SafeNormalize(Vector2.Zero);
            NPC.velocity += IncSpeed;
        }
		public override void FindFrame(int frameHeight) {
            NPC.frame.Y = 0 * frameHeight;
        }
        private static float FuncSpeed(float Spe){
            float Input = (3f - Spe < 0) ? 0 : 3f - Spe;
            return (float)Math.Tanh(Input);
        }
    }
}