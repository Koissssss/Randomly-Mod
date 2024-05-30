using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Randomly.common.systems;
using Randomly.content.projectiles.bossPros.kingSlime;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Randomly.content.npcs.overrideBosses
{
	public class OverrideBoss_KingSlime : GlobalNPC
	{
		private enum performType : int
		{
			MakeDecision,
			Teleporting,
			Jump,
			Fall
		}

		private static Asset<Texture2D> texBody;
		private static Asset<Texture2D> texCrown;
		private const int SPRITEWIDTH = 98;
		private const int SPRITEHEIGHT = 92;
        public override bool AppliesToEntity(NPC npc, bool lateInstantiation) {
			return npc.type == NPCID.KingSlime;
		}
        public override void SetStaticDefaults()
        {
            if (Main.netMode != NetmodeID.Server){
				texBody = ModContent.Request<Texture2D>("Randomly/content/npcs/overrideBosses/OverrideBoss_KingSlime", AssetRequestMode.AsyncLoad);
				texCrown = ModContent.Request<Texture2D>("Randomly/content/npcs/overrideBosses/Gores/OverrideBoss_KingSlimeCrown", AssetRequestMode.AsyncLoad);
			}
			ChildSafety.SafeGore[Mod.Find<ModGore>("OverrideBoss_KingSlimeCrown").Type] = true;
        }
        public override void SetDefaults(NPC entity)
        {
            float Temp;
            if (Main.masterMode) Temp = 2000f * 1.2f;
            else if (Main.expertMode) Temp = 2000f * 2.1f;
            else Temp = 2000f;
            Temp = (float)(Temp *(0.5 + SystemFuncs.PlayersOnServer() * 0.5));
            entity.lifeMax = (int)Temp;
        }
        public override bool PreAI(NPC npc)
        {
			float bossShirnkScale = 1;
			float bossSpriteScale = 1;
            float difImp = 1f - ((float)npc.life / npc.lifeMax);
			if (Main.player[npc.target].dead || Vector2.Distance(npc.Center, Main.player[npc.target].Center) > 4000) {
				npc.TargetClosest();
				if (Main.player[npc.target].dead || Vector2.Distance(npc.Center, Main.player[npc.target].Center) > 4000) {

					if (Main.netMode != NetmodeID.Server) {
						int CrownGore = Mod.Find<ModGore>("OverrideBoss_KingSlimeCrown").Type;
						Vector2 centerPos = npc.Center + new Vector2(0, npc.scale * 93 * 0.6f * (float)Math.Sin(npc.localAI[3])*0.1f - 8);
						Gore.NewGore(npc.GetSource_FromThis(), centerPos - new Vector2(76.5f, 75), npc.velocity, CrownGore, 0.6f);
					}

					npc.active = false;
				}
			}

			if ((int)npc.ai[0] == (int)performType.MakeDecision){
				if(OnGround(npc)){
					npc.ai[1]++;
					npc.noGravity = false;
					if(npc.ai[1] == 10){
						if (Main.netMode != NetmodeID.MultiplayerClient){
							npc.TargetClosest();
							npc.ai[1] = 0;
							npc.ai[0] = Main.rand.Next(2) * 2 - 1;
							npc.netUpdate = true;
						}
					}
				}	
			}
			if ((int)Math.Abs(npc.ai[0]) == (int)performType.Teleporting){
				npc.ai[1]++;
				npc.frameCounter++;
				if(npc.ai[1] < 60){
					bossShirnkScale = MathHelper.Clamp((60f - npc.ai[1]) / 60f, 0f, 1f);
					bossShirnkScale = 0.5f + bossShirnkScale * 0.5f;
					for(int i = 0; i < 10; i++){
						int dustInd = Dust.NewDust(npc.position + Vector2.UnitX * -20f, npc.width + 40, npc.height, DustID.TintableDust, npc.velocity.X, npc.velocity.Y, 150, new Color(255, 255, 255, 80), 2f);
						Dust dustPtr = Main.dust[dustInd];
						dustPtr.noGravity = true;
						dustPtr.velocity *= 0.5f;
					}
				}
				if(npc.ai[1] == 60){
					int tries = 0;
				OutIf2:
					if(npc.ai[3] == 0){
						tries++;
						int widOc = (npc.width * 2 + 16)/32;
						Vector2 teleportTo;
						Tile tile;
						int left = 32, right = 32;
						for(int i = 0; i < 32 + widOc; i++){
							for(int j = - (npc.height * 2 + 16) / 32; j <= (npc.height * 2 + 16) / 32; j++){
								Vector2 Pos = Main.player[npc.target].Center + new Vector2 (- i * 16, j * 16 - 32);
								tile = Main.tile[(int)(Pos.X / 16), (int)(Pos.Y / 16)];
								if(tile.HasTile && Main.tileSolid[tile.TileType] && !Main.tileSolidTop[tile.TileType]){
									left = i - widOc;
									goto OutFor1;
								}
							}
						}
						OutFor1:
						for(int i = 0; i < 32 + widOc; i++){
							for(int j = - (npc.height * 2 + 16) / 32; j <= (npc.height * 2 + 16) / 32; j++){
								Vector2 Pos = Main.player[npc.target].Center + new Vector2 (i * 16, j * 16 - 64);
								tile = Main.tile[(int)(Pos.X / 16), (int)(Pos.Y / 16)];
								if(tile.HasTile && Main.tileSolid[tile.TileType] && !Main.tileSolidTop[tile.TileType]){
									right = i - widOc;
									goto OutFor2;
								}
							}
						}
						OutFor2:
						int xPos;
						if(left + widOc < 8 && right + widOc < 8 && tries == 1){
							npc.ai[3] = 1;
							goto OutIf1;
						}
						if(left - right > 16){
							npc.ai[0] = -1;
						}
						if(right - left > 16){
							npc.ai[0] = 1;
						}
						if(npc.ai[0] == -1){
							xPos = - left * 16;
							npc.ai[2] = 1;
						}
						else{
							xPos = right * 16;
							npc.ai[2] = -1;
						}
						teleportTo = Main.player[npc.target].Center + new Vector2 (xPos, - 64);

						if (Main.netMode != NetmodeID.Server) {
							int CrownGore = Mod.Find<ModGore>("OverrideBoss_KingSlimeCrown").Type;
							Vector2 centerPos = npc.Center + new Vector2(0, npc.scale * 93 * 0.6f * (float)Math.Sin(npc.localAI[3])*0.1f - 8);
							Gore.NewGore(npc.GetSource_FromThis(), centerPos - new Vector2(76.5f, 75), npc.velocity, CrownGore, 0.6f);
						}
							
						npc.Center = teleportTo;
						if (Main.netMode != NetmodeID.MultiplayerClient){
							npc.netUpdate = true;
						}
					}
				OutIf1:
					if(npc.ai[3] == 1){
						tries++;
						Vector2 teleportTo;
						Tile tile;
						int top = 32;
						for(int i = 0; i < 32; i++){
							for(int j = - (npc.width * 2 + 16) / 32; j <= (npc.width * 2 + 16) / 32; j++){
								Vector2 Pos = Main.player[npc.target].Center + new Vector2 (- j * 16, -i * 16);
								tile = Main.tile[(int)(Pos.X / 16), (int)(Pos.Y / 16)];
								if(tile.HasTile && Main.tileSolid[tile.TileType] && !Main.tileSolidTop[tile.TileType]){
									top = i;
									goto OutFor3;
								}
							}
						}
						OutFor3:
						if(top < 8 && tries == 1){
							npc.ai[3] = 0;
							goto OutIf2;
						}

						if (Main.netMode != NetmodeID.Server) {
							int CrownGore = Mod.Find<ModGore>("OverrideBoss_KingSlimeCrown").Type;
							Vector2 centerPos = npc.Center + new Vector2(0, npc.scale * 93 * 0.6f * (float)Math.Sin(npc.localAI[3])*0.1f - 8);
							Gore.NewGore(npc.GetSource_FromThis(), centerPos - new Vector2(76.5f, 75), npc.velocity, CrownGore, 0.6f);
						}

						teleportTo = Main.player[npc.target].Center + new Vector2 (0, - top * 16);
						npc.noGravity = true;
						if (Main.netMode != NetmodeID.MultiplayerClient){
							npc.Center = teleportTo;
							npc.netUpdate = true;
						}
					}
				}
				if(npc.ai[1] >= 60){
					if(npc.ai[3] == 1){
						npc.noGravity = true;
						npc.velocity.Y += 0.5f*(1 + difImp * 0.5f);
					}
					bossShirnkScale = MathHelper.Clamp((npc.ai[1] - 60f) / 60f, 0f, 1f);
					bossShirnkScale = 0.5f + bossShirnkScale * 0.5f;
					for(int i = 0; i < 10; i++){
						int dustInd = Dust.NewDust(npc.position + Vector2.UnitX * -20f, npc.width + 40, npc.height, DustID.TintableDust, npc.velocity.X, npc.velocity.Y, 150, new Color(255, 255, 255, 80), 2f);
						Dust dustPtr = Main.dust[dustInd];
						dustPtr.noGravity = true;
						dustPtr.velocity *= 0.5f;
					}
				}
				if(npc.ai[1] == 120){
					//npc.localAI[0] = 0;
					if (Main.netMode != NetmodeID.MultiplayerClient){
						npc.ai[0] = 2 + npc.ai[3];
						npc.ai[1] = 0;
						npc.netUpdate = true;
					}
				}
			}
			if ((int)npc.ai[0] == (int)performType.Jump){
				if(npc.ai[3] >= 6){
					npc.ai[2] -= npc.ai[2]/30;
				}
				npc.velocity.X = npc.ai[2] * 5 * (1 + difImp);
				if(OnGround(npc)){
					if(Math.Abs(npc.ai[2]) > 0.05){
						SoundEngine.PlaySound(SoundID.Meowmere, npc.position);
                        if(Main.netMode != NetmodeID.MultiplayerClient){
							Projectile.NewProjectile(npc.GetSource_FromThis(), npc.Center, new Vector2((Main.rand.Next(11) - 5)/5f, -15f), ModContent.ProjectileType<BossPros_KingSlime_Meowmere>(), (int)(npc.damage * 0.13f), 0f);
                        	npc.velocity = new(npc.ai[2] * 5 * (1 + difImp), -(float)(Main.rand.Next(20) + 20) / 4f * (1f - difImp * 0.5f) * Math.Abs(npc.ai[2]));
							npc.netUpdate = true;
						}
						npc.ai[3]++;
					}
					else{
						npc.ai[0] = 0;
						npc.ai[1] = 0;
						npc.ai[2] = 0;
						npc.ai[3] = 1;
						npc.noGravity = false;
						npc.velocity.X = 0;
						if(Main.netMode != NetmodeID.MultiplayerClient){
							npc.netUpdate = true;
						}
					}
				}
			}
			else if ((int)npc.ai[0] == (int)performType.Fall){
				if(OnGround(npc)){
					npc.ai[1]++;
					if(npc.ai[1] == 1){
						SoundEngine.PlaySound(SoundID.Meowmere, npc.position);
                        if(Main.netMode != NetmodeID.MultiplayerClient){
							for(int i = 0; i < 5; i++){
								Projectile.NewProjectile(npc.GetSource_FromThis(), npc.Center, Vec2.LengthdirDeg(15, i * 30 + Main.rand.Next(20) + 20), ModContent.ProjectileType<BossPros_KingSlime_Meowmere>(), (int)(npc.damage * 0.13f), 0f);
							}
						}
						if(Main.netMode != NetmodeID.MultiplayerClient){
							npc.netUpdate = true;
						}
					}
					if(npc.ai[1] == 20){
						npc.noGravity = false;
						npc.ai[0] = 0;
						npc.ai[1] = 0;
						npc.ai[2] = 0;
						npc.ai[3] = 0;
						if(Main.netMode != NetmodeID.MultiplayerClient){
							npc.netUpdate = true;
						}
					}
				}
				else{
					if(!WillBouncePlat(npc)){
						npc.velocity.Y += 0.5f*(1 + difImp * 0.5f);
					}
					else{
						npc.velocity.Y = 16f;
					}
				}
			}
            SetBossScale(npc, bossSpriteScale, bossShirnkScale);
            return false;
        }
		private void SetBossScale(NPC npc, float bossSpriteScale, float bossShirnkScale){
				bossSpriteScale = (float)npc.life / npc.lifeMax;
				bossSpriteScale = bossSpriteScale * 0.5f + 0.5f;
				bossSpriteScale *= bossShirnkScale;
				if (bossSpriteScale != npc.scale) {
					npc.position.X += npc.width / 2;
					npc.position.Y += npc.height;
					npc.scale = bossSpriteScale;
					npc.width = (int)(SPRITEWIDTH * npc.scale);
					npc.height = (int)(SPRITEHEIGHT * npc.scale);
					npc.position.X -= npc.width / 2;
					npc.position.Y -= npc.height;
				}
		}
		private static bool OnGround(NPC npc){
			if(npc.velocity.Y >= 0){
				int left = (int)(npc.position.X / 16);
				int right = (int)((npc.position.X + npc.width - 1) / 16);
				int bottom = (int)((npc.position.Y + npc.height) / 16);
				for(int i = left; i <= right; i++){
					Tile tile = Main.tile[i, bottom];
					if(tile.HasTile && !tile.IsActuated && Main.tileSolid[tile.TileType] && !(Main.tileSolidTop[tile.TileType] && Main.player[npc.target].position.Y >= npc.position.Y + npc.height)) {
						return true;
					}
				}
			}
			return false;
		}
		private static bool WillBouncePlat(NPC npc){
			if(npc.velocity.Y >= 0){
				int left = (int)(npc.position.X / 16);
				int right = (int)((npc.position.X + npc.width - 1) / 16);
				int bottom = (int)((npc.position.Y + npc.height) / 16);
				int vComp = (int)(npc.velocity.Y / 16);
				for(int i = left; i <= right; i++){
					for(int j = 0; j <= vComp; j++){
						Tile tile = Main.tile[i, bottom + j];
						if(tile.HasTile && !tile.IsActuated && Main.tileSolid[tile.TileType] && !(Main.tileSolidTop[tile.TileType] && Main.player[npc.target].position.Y >= npc.position.Y + npc.height)) {
							return true;
						}
					}
				}
			}
			return false;
		}
        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			npc.localAI[3] += (float)Math.PI/20;
			Vector2 centerPos = npc.Center - screenPos + new Vector2(0, npc.scale * 93 * 0.6f * (float)Math.Sin(npc.localAI[3])*0.1f - 8);
			spriteBatch.Draw(texBody.Value, centerPos, new Rectangle(0, 0, 296, 186), drawColor, 0, new Vector2(148, 93), new Vector2((float)Math.Sin(npc.localAI[3])*0.1f + 1, -(float)Math.Sin(npc.localAI[3])*0.1f + 1)*npc.scale*0.6f, SpriteEffects.None, 0f);
            spriteBatch.Draw(texCrown.Value, centerPos - new Vector2(0, npc.height/2 * 0.6f * npc.scale), new Rectangle(0, 0, 153, 95), drawColor, 0, new Vector2(76.5f, 95), 0.6f, SpriteEffects.None, 0f);
            return false;
        }
        public override void OnKill(NPC npc)
        {
			if (Main.netMode != NetmodeID.Server) {
				int CrownGore = Mod.Find<ModGore>("OverrideBoss_KingSlimeCrown").Type;
				Vector2 centerPos = npc.Center + new Vector2(0, npc.scale * 93 * 0.6f * (float)Math.Sin(npc.localAI[3])*0.1f - 8);
				Gore.NewGore(npc.GetSource_FromThis(), centerPos - new Vector2(76.5f, 75), npc.velocity, CrownGore, 0.6f);
			}
        }
        public override bool? CanFallThroughPlatforms(NPC npc)
        {
            return Main.player[npc.target].position.Y > npc.position.Y;
        }
    }
}