using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Randomly.common.systems;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Randomly.content.projectiles.bossPros.frostFlower
{
	public class BossPros_FrostFlower_IceNeedle : ModProjectile
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
			Projectile.timeLeft = 500;
			Projectile.penetrate = -1;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.netImportant = true;
			Projectile.aiStyle = -1;
			CooldownSlot = ImmunityCooldownID.Bosses; // use the boss immunity cooldown counter, to prevent ignoring boss attacks by taking damage from other sources
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
		public int performType;
		public override void AI() {
            Projectile.rotation = Projectile.velocity.ToRotation();
			FadeInAndOut();
			Vector2 vecStd = Projectile.velocity.SafeNormalize(Vector2.Zero);

			if(performType == 1){
				if(Projectile.timeLeft < 400f){
					
					Projectile.velocity += Projectile.velocity*(vecStd*5f - Projectile.velocity).Length()/100;
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

			//float offsetX = 20f;
			//origin.X = Projectile.spriteDirection == 1 ? sourceRectangle.Width - offsetX : offsetX;

			Color drawColor = Projectile.GetAlpha(lightColor);
			Main.EntitySpriteDraw(texture,
				Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
				sourceRectangle, drawColor, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);
			return false;
		}
    }
}
