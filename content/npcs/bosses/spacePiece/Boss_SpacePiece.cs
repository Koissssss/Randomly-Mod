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
using Terraria.DataStructures;
using Randomly.content.projectiles.bossPros.spacePiece;
using Terraria.Audio;

namespace Randomly.content.npcs.bosses.spacePiece
{
    //[AutoloadBossHead]
    public class Boss_SpacePiece : ModNPC
    {
        public static Asset<Texture2D> texBody;
        public static Asset<Texture2D> texBottom;
        public static Asset<Texture2D>[] texHeart = new Asset<Texture2D>[4];
        public static Asset<Texture2D> texLeg;
        public static Asset<Texture2D> texDust;
        public static Asset<Texture2D> texDustBottom;
		private float timeLoop{
			get => NPC.ai[0];
			set => NPC.ai[0] = value;
		}
		private float performType{
			get => NPC.ai[1];
			set => NPC.ai[1] = value;
		}
        public override void SetDefaults()
        {
            NPC.width = 80;
            NPC.height = 80;//这两个代表这个NPC的碰撞箱宽高，以及tr会从你的贴图里扣多大的图
            NPC.damage = 40;
            NPC.lifeMax = System_BossLifeManager.Life(2800);//npc的血量上限
            NPC.defense = 14;
            NPC.scale = 1f;//npc的贴图和碰撞箱的放缩倍率
            NPC.knockBackResist = 0.1f;
            //TODO:选个声音
            //NPC.HitSound = SoundID.NPCHit5;//挨打时发出的声音
            //NPC.DeathSound = SoundID.NPCDeath7;//趋势时发出的声音
            NPC.value = Item.buyPrice(0, 2, 0, 0);//NPC的爆出来的MONEY的数量，四个空从左到右是铂金，金，银，铜
            NPC.lavaImmune = true;//对岩浆免疫
            NPC.noGravity = true;//不受重力影响。一般BOSS都是无重力的
            NPC.noTileCollide = true;//可穿墙
            NPC.npcSlots = 20; //NPC所占用的NPC数量，在TR世界里，NPC上限是200个，通常，这个用来限制Boss战时敌怪数量，填个10，20什么的
            NPC.boss = true; //将npc设为boss 会掉弱治药水和心，会显示xxx已被击败，会有血条
                             //if (!Main.dedServ){
                             //    Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/Feryquitous - Fervidex");
                             //}
        }
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 1;//图像帧数

            NPCID.Sets.MPAllowedEnemies[Type] = true;//允许多人联机召唤
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            if (!Main.dedServ)
            {
                texBody = TextureAssets.Npc[Type];
                texBottom = ModContent.Request<Texture2D>(Texture + "Bottom", AssetRequestMode.AsyncLoad);
                texHeart[0] = ModContent.Request<Texture2D>(Texture + "RedHeart", AssetRequestMode.AsyncLoad);
                texHeart[1] = ModContent.Request<Texture2D>(Texture + "YellowHeart", AssetRequestMode.AsyncLoad);
                texHeart[2] = ModContent.Request<Texture2D>(Texture + "BlueHeart", AssetRequestMode.AsyncLoad);
                texHeart[3] = ModContent.Request<Texture2D>(Texture + "PinkHeart", AssetRequestMode.AsyncLoad);
                texLeg = ModContent.Request<Texture2D>(Texture + "Leg", AssetRequestMode.AsyncLoad);
                texDust = ModContent.Request<Texture2D>(Texture + "Dust", AssetRequestMode.AsyncLoad);
                texDustBottom = ModContent.Request<Texture2D>(Texture + "DustBottom", AssetRequestMode.AsyncLoad);
            }//读取纹理素材
        }
        public override void OnKill()
        {
            NPC.SetEventFlagCleared(ref System_DownedBossSystem.DownedBoss_SpacePiece, -1);
            //战胜boss记录变量
        }
        public override void OnSpawn(IEntitySource source)
        {
            if (!spawnInit)
            {
                spawnInit = true;
                drawLegPosition = [NPC.Center, NPC.Center, NPC.Center, NPC.Center];
                drawLegPositionTo = [NPC.Center, NPC.Center, NPC.Center, NPC.Center];
                drawMidLegPosition =
                    [NPC.Center + Vec2.LengthdirDeg(141, Main.rand.Next(360)),
                    NPC.Center + Vec2.LengthdirDeg(141, Main.rand.Next(360)),
                    NPC.Center + Vec2.LengthdirDeg(141, Main.rand.Next(360)),
                    NPC.Center + Vec2.LengthdirDeg(141, Main.rand.Next(360))];
            }
        }






        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            /*
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

            */
        }










        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange([
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,//环境
				new FlavorTextBestiaryInfoElement("眼前响起了悦耳的歌声，它正慢慢朝你靠近...")//描述
			]);//图鉴
        }




        private bool spawnInit = false;
        private int heartAct = -1;
        private const int TYPE_normalmove = 0;
        private const int TYPE_redheart = 1;
        private const int TYPE_yellowheart = 2;
        private const int TYPE_blueheart = 3;
        private const int TYPE_pinkheart = 4;
        private bool actionInit = false;
        private Vector2[][]tStagePos = [new Vector2[5], new Vector2[5], new Vector2[5], new Vector2[5], new Vector2[5], new Vector2[5], new Vector2[5], new Vector2[5], new Vector2[5], new Vector2[5]];
        private float[]dirset = new float[6];
        private float[]dirdset = new float[6];
        private float dirDelta;
        private int dirRot;
        public override void AI()
        {
            NPC.TargetClosest(true);
            Player Tar = Main.player[NPC.target];
            Vector2 destinationPos = Tar.Center;

            if(performType == TYPE_normalmove){
                if(Main.netMode != NetmodeID.MultiplayerClient){
                    timeLoop++;
                    if(timeLoop == 120){
                        performType = Main.rand.Next(3) + 1;
                        actionInit = true;
                    }
                }
            }
        //------------------------------------//
            else if(performType == TYPE_redheart){
                if(Main.netMode != NetmodeID.MultiplayerClient){
                    if(actionInit){
                        timeLoop = 0;
                        actionInit = false;
                        NPC.netUpdate = true;
                    }
                }
                if(timeLoop == 0){
                    drawShoutPos = NPC.Center - new Vector2(0, 100);
                    drawShout = 120;
                    if(Main.netMode != NetmodeID.MultiplayerClient){
                        int dirSt = Main.rand.Next(360);
                        for(int i = 0; i < 10; i++){
                            for(int j = 0; j < 5; j++){
                                tStagePos[i][j] = Vec2.LengthdirDeg(j * 250, i * 36 + dirSt + Main.rand.Next(25));
                            }
                        }
                    }
                }
                if(Main.netMode != NetmodeID.MultiplayerClient){
                    const int TIMESCALE = 10;
                    for(int i = 0; i < 3; i++){
                        if(timeLoop == i * TIMESCALE){
                            for(int j = 0; j < 10; j++){
                                CBMusicSignRed(drawShoutPos + tStagePos[j][i + 1]);
                            }
                        }
                    }
                    const int TIMESTARTSHOOT = 60;
                    if(timeLoop > TIMESTARTSHOOT && timeLoop <= TIMESTARTSHOOT + 40){
                        int timeSt = (int)timeLoop - TIMESTARTSHOOT;
                        for(int i = 0; i < 10; i++){
                            Vector2 Pos = drawShoutPos + Vec2.CurveType1_Bezier(timeSt / 40f, [tStagePos[i][0], tStagePos[i][1], tStagePos[i][2], tStagePos[i][3], tStagePos[i][4]]);
                            Vector2 PosNext = drawShoutPos + Vec2.CurveType1_Bezier((timeSt - 0.1f) / 40f, [tStagePos[i][0], tStagePos[i][1], tStagePos[i][2], tStagePos[i][3], tStagePos[i][4]]);
                            CBSpikeRed(Pos, (Pos - PosNext).ToRotation(), 1 - timeSt/60f);
                        }
                    }
                }
                const int TIMEEND = 120;
                if(timeLoop == TIMEEND){
                    performType = TYPE_normalmove;
                    timeLoop = 0;
                }
                else{
                    timeLoop++;
                }
                destinationPos = Vector2.Lerp(NPC.Center, drawShoutPos + new Vector2(0, 100), 0.1f) + NPC.velocity * 0.95f;
            }
        //------------------------------------//
            else if(performType == TYPE_yellowheart){
                if(Main.netMode != NetmodeID.MultiplayerClient){
                    if(actionInit){
                        timeLoop = 0;
                        actionInit = false;
                        NPC.netUpdate = true;
                    }
                }
                if(timeLoop == 0){
                    drawShoutPos = NPC.Center - new Vector2(0, 100);
                    drawShout = 120;
                    if(Main.netMode != NetmodeID.MultiplayerClient){
                        for(int i = 0; i < 6; i++){
                            dirset[i] = Main.rand.Next(360);
                            dirdset[i] = (Main.rand.Next(40) - 20)/10f;
                        }
                    }
                }
                if(Main.netMode != NetmodeID.MultiplayerClient){
                    const int TIMESCALE = 10;
                    for(int i = 0; i < 6; i++){
                        if(timeLoop == i * TIMESCALE){
                            float dirBase = Main.rand.Next(360);
                            for(int j = 0; j < 8; j++){
                                CBMusicSignYellow(i * 100 + 100, j * 45 + dirBase);
                            }
                        }
                    }
                }
                const int TIMEEND = 200;
                if(timeLoop == TIMEEND){
                    performType = TYPE_normalmove;
                    timeLoop = 0;
                }
                else{
                    timeLoop++;
                }
            }
        //------------------------------------//
            else if(performType == TYPE_blueheart){
                if(Main.netMode != NetmodeID.MultiplayerClient){
                    if(actionInit){
                        timeLoop = 0;
                        actionInit = false;
                        NPC.netUpdate = true;
                    }
                }
                if(timeLoop == 0){
                    drawShoutPos = NPC.Center - new Vector2(0, 100);
                    drawShout = 120;
                    if(Main.netMode != NetmodeID.MultiplayerClient){
                        dirDelta = Main.rand.Next(360);
                        dirRot = -1;// Main.rand.Next(2) * 2 - 1;
                    }
                }
                if(Main.netMode != NetmodeID.MultiplayerClient){
                    const int TIMESCALE = 30;
                    if(timeLoop == TIMESCALE){
                        for(int j = 0; j < 16; j++){
                            Vector2 Pos = NPC.Center + Vec2.Polygon_Type1(dirDelta, 4, 4, j, 100) - new Vector2(0, 100);
                            CBMusicSignBlue(Pos);
                        }
                    }
                    const int TIMESTARTSHOOT = 60;
                    if(timeLoop > TIMESTARTSHOOT && timeLoop <= TIMESTARTSHOOT + 90){
                        int timeSt = (int)timeLoop - TIMESTARTSHOOT;
                        for(int j = 0; j < 16; j++) {
                            Vector2 Pos = NPC.Center + Vec2.Polygon_Type1(dirDelta, 4, 4, (float)(j + dirRot * timeSt * 0.03), 100 + timeSt * 15) - new Vector2(0, 100);
                            Vector2 PosNext = NPC.Center + Vec2.Polygon_Type1(dirDelta, 4, 4, (float)(j + dirRot * (timeSt + 1) * 0.03), 100 + (timeSt + 1) * 15)  - new Vector2(0, 100);
                            CBSpikeBlue(Pos, (PosNext - Pos).ToRotation(), 1 - timeSt/180f);
                        }
                    }
                }
                const int TIMEEND = 200;
                if(timeLoop == TIMEEND){
                    performType = TYPE_normalmove;
                    timeLoop = 0;
                }
                else{
                    timeLoop++;
                }
                destinationPos = Vector2.Lerp(NPC.Center, drawShoutPos + new Vector2(0, 100), 0.1f) + NPC.velocity * 0.95f;
            }


            //Move To Destination Pos
            float Speed = NPC.velocity.Length();
            if ((performType != TYPE_yellowheart && Speed >= 0.1f) || (performType == TYPE_yellowheart && Speed >= 0.62f))
            {
                float friction = (Speed - 0.1f) / Speed;
                if(performType == TYPE_yellowheart){
                    friction = (Speed - 0.62f) / Speed;
                }
                NPC.velocity = friction * NPC.velocity;
            }
            else{NPC.velocity *= 0;}
            float CurSpeed = NPC.velocity.Length();
            Vector2 IncSpeed = 0.6f * FuncSpeed(CurSpeed) * (destinationPos - NPC.Center).SafeNormalize(Vector2.Zero);
            NPC.velocity += IncSpeed;
            if(NPC.velocity.Length() > (destinationPos - NPC.Center).Length()){
                NPC.velocity = NPC.velocity*(destinationPos - NPC.Center).Length()/NPC.velocity.Length();
            }
        }
        private static float FuncSpeed(float Spe)
        {
            float Input = (8f - Spe < 0) ? 0 : 8f - Spe;
            return (float)Math.Tanh(Input);
        }
        private int CBMusicSignRed(Vector2 position){
            IEntitySource entitySource = NPC.GetSource_FromAI();
            int type = ModContent.ProjectileType<BossPros_SpacePiece_MusicSignRed>();
            return Projectile.NewProjectile(entitySource, position, new Vector2(0, 0), type, NPC.damage/8, 0f);
        }
        private int CBSpikeRed(Vector2 position, float direction, float scale){
            IEntitySource entitySource = NPC.GetSource_FromAI();
            int type = ModContent.ProjectileType<BossPros_SpacePiece_SpikeRed>();
            return Projectile.NewProjectile(entitySource, position, new Vector2(0, 0), type, NPC.damage/8, 0f, -1, direction, scale);
        }


        private int CBMusicSignYellow(float reLen, float reDir){
            IEntitySource entitySource = NPC.GetSource_FromAI();
            int type = ModContent.ProjectileType<BossPros_SpacePiece_MusicSignYellow>();
            return Projectile.NewProjectile(entitySource, NPC.Center + Vec2.LengthdirDeg(reLen, reDir) - new Vector2(0, 100), new Vector2(0, 0), type, NPC.damage/8, 0f, -1, reLen, reDir, NPC.whoAmI);
        }


        private int CBMusicSignBlue(Vector2 position){
            IEntitySource entitySource = NPC.GetSource_FromAI();
            int type = ModContent.ProjectileType<BossPros_SpacePiece_MusicSignBlue>();
            return Projectile.NewProjectile(entitySource, position, new Vector2(0, 0), type, NPC.damage/8, 0f);
        }
        private int CBSpikeBlue(Vector2 position, float direction, float scale){
            IEntitySource entitySource = NPC.GetSource_FromAI();
            int type = ModContent.ProjectileType<BossPros_SpacePiece_SpikeBlue>();
            return Projectile.NewProjectile(entitySource, position, new Vector2(0, 0), type, NPC.damage/8, 0f, -1, direction, scale);
        }








        private Color statueColor = new(50, 0, 100);
        private readonly Color []HEARTSTATUECOLOR = [
            new Color(50, 0, 100),
            new Color(128, 0, 0),
            new Color(120, 99, 0),
            new Color(0, 66, 125),
            new Color(163, 65, 85),
        ];
        private float bottomRotation;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            statueColor = Color.Lerp(statueColor, HEARTSTATUECOLOR[heartAct + 1], 0.1f);
            heartAct = (int)(performType - 1);
            DrawLegs(spriteBatch, screenPos, drawColor);
            DrawMainBody(spriteBatch, screenPos, drawColor);
            DrawHeart(spriteBatch, screenPos, drawColor);
            DrawShout(spriteBatch, screenPos, drawColor);
            return false;
        }




        private Vector2[] drawLegPosition;
        private Vector2[] drawLegPositionTo;
        private Vector2[] drawMidLegPosition;
        private void DrawLegs(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            for (int i = 0; i < 4; i++)
            {
                if ((drawLegPositionTo[i] - NPC.Center - NPC.velocity * 20f).Length() < 40 || (drawLegPositionTo[i] - NPC.Center - NPC.velocity * 20f).Length() > 242)
                {
                    drawLegPositionTo[i] = NPC.Center + NPC.velocity * 20f + Vec2.LengthdirDeg(100 + Main.rand.Next(82), Main.rand.Next(360));
				    SoundEngine.PlaySound(SoundID.DD2_CrystalCartImpact);
                }
                drawLegPosition[i] = (drawLegPosition[i] * 0.5f) + (drawLegPositionTo[i] * 0.5f);
                if ((drawLegPosition[i] - NPC.Center).Length() > 280)
                {
                    drawLegPosition[i] = NPC.Center + 280 * (drawLegPosition[i] - NPC.Center).SafeNormalize(Vector2.Zero);
                }

                Vector2 midptrVec = drawMidLegPosition[i] - NPC.Center;
                Vector2 ptrVec = drawLegPosition[i] - NPC.Center;
                float len = (float)Math.Sqrt(141 * 141 - ptrVec.Length() * ptrVec.Length() / 4f);
                Vector2 midVecTo1 = 0.5f * ptrVec + len * ptrVec.SafeNormalize(Vector2.Zero).RotatedBy(Math.PI / 2);
                Vector2 midVecTo2 = 0.5f * ptrVec + len * ptrVec.SafeNormalize(Vector2.Zero).RotatedBy(-Math.PI / 2);
                if ((midptrVec - midVecTo1).Length() < (midptrVec - midVecTo2).Length())
                {
                    drawMidLegPosition[i] += (midVecTo1 - midptrVec) * 0.5f;
                }
                else
                {
                    drawMidLegPosition[i] += (midVecTo2 - midptrVec) * 0.5f;
                }

                Asset<Texture2D> sprite = texLeg;
                Rectangle recSource = new Rectangle(0, 0, sprite.Width() - 1, sprite.Height() - 1);
                spriteBatch.Draw(sprite.Value, NPC.Center - screenPos, recSource, drawColor, (drawMidLegPosition[i] - NPC.Center).ToRotation() - (float)Math.PI / 2, new Vector2(9, 9), 1f, SpriteEffects.None, 0f);
                spriteBatch.Draw(sprite.Value, drawMidLegPosition[i] - screenPos, recSource, drawColor, (drawLegPosition[i] - drawMidLegPosition[i]).ToRotation() - (float)Math.PI / 2, new Vector2(9, 9), 1f, SpriteEffects.None, 0f);
            }
        }




        private int drawBodyDustTimeCount = 0;
        private int drawBodyDustNum = 0;
        private Vector2[] drawBodyDustPosition = new Vector2[25];
        private int[] drawBodyDustTime = new int[25];
        private float[] drawBodyDustDir = new float[25];
        private void DrawMainBody(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Vector2 drawPosCenter = Entity.Center - screenPos;
            Color finalColor = drawColor.MultiplyRGB(statueColor);

            bottomRotation += (float)Math.PI / 240;

            drawBodyDustTimeCount++;
            if (drawBodyDustTimeCount == 10)
            {
                drawBodyDustTimeCount = 0;
                drawBodyDustTime[drawBodyDustNum] = 200;
                drawBodyDustDir[drawBodyDustNum] = Main.rand.Next(360);
                drawBodyDustPosition[drawBodyDustNum] = Entity.Center + Vec2.LengthdirDeg(30, drawBodyDustDir[drawBodyDustNum]);
                drawBodyDustNum++;
            }
            DrawSprite(texBottom, drawPosCenter, bottomRotation, 1f, finalColor, spriteBatch);
            for (int i = 0; i < drawBodyDustNum; i++)
            {
                DrawSprite(texDustBottom, drawBodyDustPosition[i] - screenPos, 0, drawBodyDustTime[i] / 200f, finalColor, spriteBatch);
            }
            DrawSprite(texBody, drawPosCenter, 0, 1f, finalColor, spriteBatch);
            for (int i = 0; i < drawBodyDustNum; i++)
            {
                DrawSprite(texDust, drawBodyDustPosition[i] - screenPos, 0, drawBodyDustTime[i] / 200f, finalColor, spriteBatch);
            }
            for (int i = 0; i < drawBodyDustNum; i++)
            {
                drawBodyDustTime[i]--;
                drawBodyDustPosition[i] += Vec2.LengthdirDeg(0.1, drawBodyDustDir[i]);
                if (drawBodyDustTime[i] <= 0)
                {
                    drawBodyDustTime[i] = drawBodyDustTime[drawBodyDustNum - 1];
                    drawBodyDustDir[i] = drawBodyDustDir[drawBodyDustNum - 1];
                    drawBodyDustPosition[i] = drawBodyDustPosition[drawBodyDustNum - 1];
                    drawBodyDustNum--;
                    i--;
                }
            }
        }

        private Vector2[] drawHeartPos = [new(0, 0), new(0, 0), new(0, 0), new(0, 0)];
        private void DrawHeart(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++){
                    Draw.DrawLine(spriteBatch, screenPos, Vec2.LengthdirDeg(15, j * 90 - bottomRotation * 180 / Math.PI) + Entity.Center, drawHeartPos[i] + Entity.Center, Color.Black);
                }
            }
            for (int i = 0; i < 4; i++)
            {
                if(heartAct == -1){
                    drawHeartPos[i] += (Vec2.LengthdirDeg(25, i * 90 + bottomRotation * 180 / Math.PI) - drawHeartPos[i])*0.2f;
                }
                else{
                    if(i == heartAct){
                        drawHeartPos[i] += (new Vector2(0, -100) - drawHeartPos[i])*0.2f;
                    }
                    else{
                        int temp = (i - heartAct + 4) % 4;
                        drawHeartPos[i] += (Vec2.LengthdirDeg(25, (temp - 2) * 30 + i * 90 + bottomRotation * 180 / Math.PI) - drawHeartPos[i])*0.2f;
                    }
                }
                DrawSprite(texHeart[i], drawHeartPos[i] + Entity.Center - screenPos, 0, 1, drawColor, spriteBatch);
            }
        }


        private int drawShout = 0;
        private Vector2 drawShoutPos;
        private void DrawShout(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor){
            if(drawShout == 120){
                SoundStyle impactSound;
                switch(Main.rand.Next(2)){
                    default:case 0:impactSound = new SoundStyle("Randomly/Assets/SoundEffects/SoundEffect_SpacePiece_1") {
					Volume = 0.7f,
					PitchVariance = 0.5f,
				    };break;
                    case 1:impactSound = new SoundStyle("Randomly/Assets/SoundEffects/SoundEffect_SpacePiece_2") {
					Volume = 0.7f,
					PitchVariance = 0.5f,
				    };break;
                }

				SoundEngine.PlaySound(impactSound);
            }
            if(drawShout > 0){
                for(int i = Math.Min(drawShout, 100); i >= Math.Max(0, drawShout - 20); i--){
                    int len = 100 - i;
                    int existTime = 20 - (drawShout - i); 
                    Color color = Color.Lerp(statueColor, Color.Black, 0.5f + (float)Math.Sin((100 - i)/10f) * 0.5f);
                    DrawCircle(spriteBatch, screenPos, color, drawShoutPos, len * 24, existTime * 3, 255 - existTime * 12);
                }
                drawShout--;
            }
        }

        private void DrawCircle(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor, Vector2 CenterPos, float r, float shrink, int alpha){
            int ddir = Main.rand.Next(360);
            Color newColor = new Color(drawColor.R, drawColor.G, drawColor.B, alpha)*(float)(alpha/255f);
            for(int i = 0; i < 36; i++){
                Vector2 stPos = CenterPos + Vec2.LengthdirDeg(r - i % 2 * shrink, i * 10 + ddir);
                Vector2 edPos = CenterPos + Vec2.LengthdirDeg(r - (i + 1) % 2 * shrink, i * 10 + 10 + ddir);
                Draw.DrawLine(spriteBatch, screenPos, stPos, edPos, newColor);
            }
        }

        private void DrawSprite(Asset<Texture2D> sprite, Vector2 drawPosCenter, float rotation, float scale, Color blend, SpriteBatch spriteBatch)
        {
            Rectangle recSource = new Rectangle(0, 0, sprite.Width() - 1, sprite.Height() - 1);
            Vector2 texCenter = new(sprite.Width() / 2, sprite.Height() / 2);
            spriteBatch.Draw(sprite.Value, drawPosCenter, recSource, blend, rotation, texCenter, scale, SpriteEffects.None, 0f);
        }
        private bool CheckIfPlayerAlive()
        {
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active || Vector2.Distance(NPC.Center, Main.player[NPC.target].Center) > 4000)
            {
                NPC.TargetClosest();
                if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active || Vector2.Distance(NPC.Center, Main.player[NPC.target].Center) > 4000)
                {
                    return false;
                }
            }
            return true;
        }
    }
}