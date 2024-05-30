using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Randomly.common.systems;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent.ItemDropRules;
using Randomly.content.items.placeable.trophys;
using Randomly.content.items.armors.vanitys;
using Randomly.content.items.materials;
using Randomly.content.items.consumables.bossbags;
using Randomly.content.items.placeable.relics;
using Randomly.content.pets.Pet_FrostFlower;
using Terraria.GameContent;
using Randomly.content.projectiles.bossPros.frostFlower;
using Randomly.content.items.weapons.magics;
using Randomly.content.items.weapons.yoyos;

namespace Randomly.content.npcs.bosses.frostFlower
{
    [AutoloadBossHead]
    public class Boss_FrostFlower : ModNPC
    {
        public static Asset<Texture2D> Part;
        public override void SetDefaults() {
            NPC.width = 68;
            NPC.height = 68;//这两个代表这个NPC的碰撞箱宽高，以及tr会从你的贴图里扣多大的图
            NPC.damage = 30;
            NPC.lifeMax = System_BossLifeManager.Life(1500);//npc的血量上限
            NPC.defense = 10;
            NPC.scale = 1f;//npc的贴图和碰撞箱的放缩倍率
            NPC.knockBackResist = 0f;
            NPC.HitSound = SoundID.NPCHit5;//挨打时发出的声音
            NPC.DeathSound = SoundID.NPCDeath7;//趋势时发出的声音
            NPC.value = Item.buyPrice(0, 1, 0, 0);//NPC的爆出来的MONEY的数量，四个空从左到右是铂金，金，银，铜
            NPC.lavaImmune = true;//对岩浆免疫
            NPC.noGravity = true;//不受重力影响。一般BOSS都是无重力的
            NPC.noTileCollide = true;//可穿墙
            NPC.npcSlots = 20; //NPC所占用的NPC数量，在TR世界里，NPC上限是200个，通常，这个用来限制Boss战时敌怪数量，填个10，20什么的
            NPC.boss = true; //将npc设为boss 会掉弱治药水和心，会显示xxx已被击败，会有血条
            if (!Main.dedServ){
			    Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/Feryquitous - Fervidex");
            }
            //NPC.dontTakeDamage = true;//为true则为无敌，这里的无敌意思是弹幕不会打到npc，并且npc的血条也不会显示了
		}
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 1;//图像帧数

			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frozen] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Chilled] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frostburn] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frostburn2] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;

			NPCID.Sets.MPAllowedEnemies[Type] = true;//允许多人联机召唤
			NPCID.Sets.BossBestiaryPriority.Add(Type);
            if (!Main.dedServ)
            {
                Part = ModContent.Request<Texture2D>(Texture + "Part", AssetRequestMode.AsyncLoad);
            }//读取纹理素材


			var drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers() { // Influences how the NPC looks in the Bestiary
				CustomTexturePath = "Randomly/Assets/Textures/BossList/BossList_FrostFlower", // If the NPC is multiple parts like a worm, a custom texture for the Bestiary is encouraged.
				//Position = new Vector2(40f, 24f),
				//PortraitPositionXOverride = 0f,
				//PortraitPositionYOverride = 12f
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifier);
        }
		public override void OnKill() {
			NPC.SetEventFlagCleared(ref System_DownedBossSystem.DownedBoss_FrostFlower, -1);
            //战胜boss记录变量
		}






		public override void ModifyNPCLoot(NPCLoot npcLoot) {
			// Trophies are spawned with 1/10 chance
    		npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Trophy_FrostFlower>(), 10));

			// All the Classic Mode drops here are based on "not expert", meaning we use .OnSuccess() to add them into the rule, which then gets added
		    LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());

			// Notice we use notExpertRule.OnSuccess instead of npcLoot.Add so it only applies in normal mode
			// Boss masks are spawned with 1/7 chance
    		notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Vanity_FrostFlowerMask>(), 7));

    		notExpertRule.OnSuccess(ItemDropRule.FewFromOptions(1, 1, [ModContent.ItemType<Magic_IcyBurst>(), ModContent.ItemType<Yoyo_IceYoyo>()]));

			// This part is not required for a boss and is just showcasing some advanced stuff you can do with drop rules to control how items spawn
			// We make 12-15 ExampleItems spawn randomly in all directions, like the lunar pillar fragments. Hereby we need the DropOneByOne rule,
			// which requires these parameters to be defined
            int itemType = ModContent.ItemType<Material_SoulyIce>();
            var parameters = new DropOneByOne.Parameters() {
                ChanceNumerator = 1,
                ChanceDenominator = 1,
                MinimumStackPerChunkBase = 1,
                MaximumStackPerChunkBase = 1,
                MinimumItemDropsCount = 40,
                MaximumItemDropsCount = 50,
            };

		    notExpertRule.OnSuccess(new DropOneByOne(itemType, parameters));

			// Finally add the leading rule
		    npcLoot.Add(notExpertRule);

			// Add the treasure bag using ItemDropRule.BossBag (automatically checks for expert mode)
		    npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<Bossbag_FrostFlower>()));

			// ItemDropRule.MasterModeCommonDrop for the relic
		    npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<Relic_FrostFlower>()));

			// ItemDropRule.MasterModeDropOnAllPlayers for the pet
		    npcLoot.Add(ItemDropRule.MasterModeDropOnAllPlayers(ModContent.ItemType<Pet_FrostFlower_Item>(), 4));
		}










		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.Info.AddRange([
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Snow,//环境
				new FlavorTextBestiaryInfoElement("雪中开放的花朵通常有着坚韧的灵魂，当这些灵魂受到某些因素的诱导而聚合的时候，就会出现一些异常现象……")//描述
			]);//图鉴
		}
        private float ImgAngleDeg;
        private float ImgAngleRad;
        private float BulAngleDeg = 0;
        private Player Tar;
        private bool []PartInit = [false, false, false, false];
        private int []Time_Part = [0, 0, 0, 0];
        private int loopCount = 0;
        private float actionSpeed;
        private bool instantFlee;
        public override void AI()
        {
            const int TIMEPERLOOP = 1000;
            int timeLoop = ((int)NPC.ai[0]) % TIMEPERLOOP;
            while(loopCount < ((int)NPC.ai[0]) / TIMEPERLOOP){
                loopCount++;
                PartInit[0] = false;
                PartInit[1] = false;
                PartInit[2] = false;
                PartInit[3] = false;
            }

            if(!instantFlee){
                //Form 1
                if(timeLoop <= 600){
                    if(!PartInit[0]){
                        Time_Part[0] = 0;
                        ImgAngleDeg = Main.rand.Next(360);
                        NPC.TargetClosest(true);
                        Tar = Main.player[NPC.target];
                        if(Main.netMode != NetmodeID.MultiplayerClient){
                            BulAngleDeg = Main.rand.Next(360);
                            NPC.ai[1] = (Main.rand.Next(50) + 50)/25 * (Main.rand.Next(2)*2 - 1);
                            Entity.Center = Tar.Center + Vec2.LengthdirDeg(350f, Main.rand.Next(360));
                            NPC.netUpdate = true;
                        }
                        PartInit[0] = true;
                    }
                    while(Time_Part[0] <= timeLoop){
                        Time_Part[0]++;
                        ImgAngleDeg += NPC.ai[1];

                        if(Time_Part[0] < 30){NPC.alpha = 255 - Time_Part[0] * 255 / 30;}
                        if(Time_Part[0] > 570){NPC.alpha = (Time_Part[0] - 570) * 255 / 30;}

                        if (NPC.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient) {
                            BulAngleDeg += NPC.ai[1];
                            if(Time_Part[0] % 40 == 0){
                                for(int i = 0; i < 8 ; i++){
                                    float direction = i * 45 + BulAngleDeg;
                                    Vector2 posOffset = Vec2.LengthdirDeg(50, direction);
                                    CBIcyNeedle(posOffset, direction, 4);
                                }
                            }
                        }
                        NPC.velocity = actionSpeed*(Tar.Center - NPC.Center).SafeNormalize(Vector2.Zero);
                    }
                }
                //Form 2
                else if(timeLoop <= 1000){
                    if(!PartInit[1]){
                        Time_Part[1] = 0;
                        ImgAngleDeg = Main.rand.Next(360);
                        if(Main.netMode != NetmodeID.MultiplayerClient){
                            NPC.ai[1] = Main.rand.Next(2)*2 - 1;
                            NPC.velocity = new(0, 0);
                            int dirTemp = Main.rand.Next(360);
                            BulAngleDeg = dirTemp + 22.5f;
                            Entity.Center = Tar.Center + Vec2.LengthdirDeg(250f, dirTemp);
                            NPC.netUpdate = true;
                        }

                        PartInit[1] = true;
                    }
                    while(Time_Part[1] <= timeLoop - 600){
                        Time_Part[1]++;
                        ImgAngleDeg += NPC.ai[1] * 0.2f;
                        
                        if(Time_Part[1] < 30){NPC.alpha = 255 - Time_Part[1] * 255 / 30;}
                        if(Time_Part[1] > 370){NPC.alpha = (Time_Part[1] - 370) * 255 / 30;}

                        if (NPC.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient) {
                            BulAngleDeg += NPC.ai[1] * 0.2f;
                            if(Time_Part[1] >= 30 && Time_Part[1] <= 270 && Time_Part[1] % 30 == 0){
                                for(int i = 0; i < 8; i += 1){
                                    double s = Math.Pow((double)(Time_Part[1] - 30) / 240, 2);
                                    float XPos = (float)(4 + Math.Pow(4, Math.Pow(1 - s, 10)) * Math.Sin((1 - s) * 3 * Math.PI / 2));
                                    float YPos = (float)Math.Cos((1 - s) * 3 * Math.PI / 2) * NPC.ai[1];
                                    float dirRot = BulAngleDeg + i * 45;
                                    Vector2 Pos = new Vector2 (XPos * 150, YPos * 150).RotatedBy(dirRot * Math.PI / 180);
                                    int bul = CBIcyNeedle(Pos, (float)(s * 180 * NPC.ai[1] + dirRot), 0.1f);
                                    if(bul >= 0){
                                        Projectile needle = Main.projectile[bul];
                                        BossPros_FrostFlower_IceNeedle objNeedle = (BossPros_FrostFlower_IceNeedle)needle.ModProjectile;
                                        objNeedle.performType = 1;
                                    }
                                }
                            }
                        }
                    }
                }
                CheckIfPlayerAlive();
            }
            else{
                NPC.alpha += 10;
                if(timeLoop <= 600){
                    ImgAngleDeg += NPC.ai[1];
                }
                else{
                    ImgAngleDeg -= NPC.ai[1] * 0.2f;
                }
                if(NPC.alpha > 255){
                    NPC.active = false;
                }
            }

            if (Main.netMode == NetmodeID.MultiplayerClient) {
                NPC.netOffset = new(0, 0);
            }

            ImgAngleRad = (float)(ImgAngleDeg * Math.PI / 180);

            actionSpeed = 2 - (float)NPC.life / NPC.lifeMax;
            NPC.ai[0] += actionSpeed;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {


            Texture2D texture = Part.Value;
            Vector2 HalfTexture= new(texture.Width / 2, texture.Height / 2);
            spriteBatch.Draw(texture, Entity.Center - screenPos, new Rectangle(0, 0, 160, 160), NPC.GetAlpha(drawColor), -ImgAngleRad, HalfTexture, 1f, SpriteEffects.None, 0f);
            //绘制纹理素材，纹理素材在SetStaticDefaults中加载
			SpriteEffects spriteEffects = SpriteEffects.None;

			texture = TextureAssets.Npc[Type].Value;

			Rectangle sourceRectangle = new Rectangle(0, 0, 68, 68);

			Vector2 origin = sourceRectangle.Size() / 2f;
			spriteBatch.Draw(texture,
				NPC.Center - Main.screenPosition + new Vector2(0f, NPC.gfxOffY),
				sourceRectangle, NPC.GetAlpha(drawColor), NPC.rotation, origin, NPC.scale, spriteEffects, 0);
			return false;
        }
        private int CBIcyNeedle(Vector2 posOffset, float rotation, float v)
        {
            var entitySource = NPC.GetSource_FromAI();
            Vector2 position = NPC.Center;
            int damage = NPC.damage / 6;
            int type = ModContent.ProjectileType<BossPros_FrostFlower_IceNeedle>();
            Vector2 speed = Vec2.LengthdirDeg(v, rotation);
            return Projectile.NewProjectile(entitySource, position + posOffset, speed, type, (int)(damage * 0.7), 0f);
        }
        private void CheckIfPlayerAlive(){
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active || Vector2.Distance(NPC.Center, Main.player[NPC.target].Center) > 4000) {
				NPC.TargetClosest();
				if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active || Vector2.Distance(NPC.Center, Main.player[NPC.target].Center) > 4000) {
                    instantFlee = true;
				}
			}
        }
    }
}