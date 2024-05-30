using System;
using Microsoft.Xna.Framework;
using Randomly.common.systems;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Randomly.content.projectiles.weapons.yoyoPros
{
	public class YoyoPros_IceYoyo_BreakPiece : ModProjectile
	{
		public override void SetStaticDefaults() {
			Main.projFrames[Type] = 1;
		}
		private int rotSpe;
		public override void SetDefaults() {
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.Hitbox = new Rectangle(0, 0, 10, 10);
			Projectile.timeLeft = 200;
			Projectile.penetrate = 2;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.aiStyle = -1;
			Projectile.rotation = (float)(Main.rand.Next(360)*Math.PI/180);
			rotSpe = Main.rand.Next(11) - 5;
        }
		public override void AI() {
			Projectile.rotation += rotSpe/2f;
            Projectile.velocity += new Vector2(0, 0.1f);
		}
    }
}
