using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Randomly.common.systems;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Randomly.content.projectiles.weapons.magicPros
{
	public class MagicPros_IcyBurst_BurstOut : ModProjectile
	{
		public override void SetStaticDefaults() {
			Main.projFrames[Type] = 1;
		}

		public override void SetDefaults() {
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.timeLeft = 180;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.aiStyle = -1;
		}

		private void FadeInAndOut() {
			int fadeSpeed = 10;
            if (Projectile.timeLeft < 255f / fadeSpeed) {
				Projectile.alpha += fadeSpeed;
				if (Projectile.alpha > 255) {
					Projectile.alpha = 255;
				}
			}
		}
		public override void AI() {
            Projectile.scale = Projectile.ai[0]*2;
            Projectile.rotation = Projectile.velocity.ToRotation();
            float Spe = Projectile.velocity.Length();
            Projectile.velocity = Projectile.velocity * (Spe*0.9f + 0.1f) / Spe;
			FadeInAndOut();
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
