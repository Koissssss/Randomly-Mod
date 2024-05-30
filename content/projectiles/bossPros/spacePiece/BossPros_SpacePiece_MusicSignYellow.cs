using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Randomly.common.systems;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Randomly.content.projectiles.bossPros.spacePiece
{
	public class BossPros_SpacePiece_MusicSignYellow : ModProjectile
	{
		public bool FadedIn {
			get => Projectile.localAI[0] == 1f;
			set => Projectile.localAI[0] = value ? 1f : 0f;
		}

		public override void SetStaticDefaults() {
			Main.projFrames[Type] = 1;
		}

		public override void SetDefaults() {
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.alpha = 255;
			Projectile.timeLeft = 110;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.netImportant = true;
			Projectile.aiStyle = -1;
			rotSpeed = (Main.rand.Next(10) - 5) / 5f;
		}
		public override Color? GetAlpha(Color lightColor) {
			// When overriding GetAlpha, you usually want to take the projectiles alpha into account. As it is a value between 0 and 255,
			// it's annoying to convert it into a float to multiply. Luckily the Opacity property handles that for us (0f transparent, 1f opaque)
			return Color.White * Projectile.Opacity;
		}

		private void FadeInAndOut() {
			// Fade in (we have Projectile.alpha = 255 in SetDefaults which means it spawns transparent)
			int fadeSpeed = 10;
			if (!FadedIn && Projectile.alpha > 0) {
				Projectile.alpha -= fadeSpeed;
				if (Projectile.alpha < 0) {
					FadedIn = true;
					Projectile.alpha = 0;
				}
			}
			else if (FadedIn && Projectile.timeLeft < 255f / fadeSpeed) {
				// Fade out so it aligns with the projectile despawning
				Projectile.alpha += fadeSpeed;
				if (Projectile.alpha > 255) {
					Projectile.alpha = 255;
				}
			}
		}
		public Vector2 ReVec2{
			get => Vec2.LengthdirDeg(Projectile.ai[0], Projectile.ai[1]);
			set
			{
				Projectile.ai[0] = value.Length();
				Projectile.ai[1] = value.ToRotation();
			}
		}
		private Vector2 tarCenter = new(0, 0);
		private float rotDir;
		private float rotSpeed;
		public override void AI() {
			NPC parentNPC = Main.npc[(int)Projectile.ai[2]];
			if(parentNPC.active && Projectile.timeLeft > 10){
				tarCenter = parentNPC.Center;
			}
			rotDir += Projectile.timeLeft/50f * rotSpeed;
			Vector2 newReVec2 = ReVec2.RotatedBy(rotDir * Math.PI / 180) - new Vector2(0, 100);
			Projectile.Center = tarCenter + newReVec2;
			FadeInAndOut();
			if(Projectile.timeLeft <= 10){
				IEntitySource entitySource = Projectile.GetSource_FromAI();
				int type = ModContent.ProjectileType<BossPros_SpacePiece_SpikeYellow>();
				float scale = 0.5f + Projectile.timeLeft * 0.05f;
				float direction = (float)(ReVec2.ToRotation() + rotDir * Math.PI / 180);
                if(Main.netMode != NetmodeID.Server){
					Projectile.NewProjectile(entitySource, Projectile.Center + Vec2.LengthdirRad(150 - Projectile.timeLeft * 15, direction), new Vector2(0, 0), type, Projectile.damage, 0f, -1, direction, scale);
				}
			}
		}
		public override bool PreDraw(ref Color lightColor) {
			SpriteEffects spriteEffects = SpriteEffects.None;
			Texture2D texture = TextureAssets.Projectile[Type].Value;

			int frameHeight = texture.Height / Main.projFrames[Type];
			int startY = frameHeight * Projectile.frame;

			Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);

			Vector2 origin = sourceRectangle.Size() / 2f;

			Color drawColor = Projectile.GetAlpha(lightColor);
			Main.EntitySpriteDraw(texture,
				Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
				sourceRectangle, drawColor, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);
			return false;
		}
    }
}
