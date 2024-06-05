using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Randomly.common.systems;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Randomly.content.projectiles.bossPros.spacePiece
{
	public class BossPros_SpacePiece_SpikeYellow : ModProjectile
	{
		public override void SetStaticDefaults() {
			Main.projFrames[Type] = 1;
		}

		public override void SetDefaults() {
			Projectile.width = 30;
			Projectile.height = 30;
			Projectile.timeLeft = 30;
			Projectile.penetrate = -1;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.alpha = 255;
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
			if (Projectile.timeLeft < 255f / fadeSpeed) {
				// Fade out so it aligns with the projectile despawning
				Projectile.alpha += fadeSpeed;
				Projectile.hostile = false;
				if (Projectile.alpha > 255) {
					Projectile.alpha = 255;
				}
			}
			else{
				Projectile.alpha = 0;
			}
		}
		public override void AI() {
			Projectile.rotation = Projectile.ai[0];
			Projectile.scale = Projectile.ai[1];
			FadeInAndOut();
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
