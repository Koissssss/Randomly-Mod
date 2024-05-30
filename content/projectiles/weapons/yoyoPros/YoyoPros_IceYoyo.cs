using Microsoft.Xna.Framework;
using Randomly;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Randomly.content.projectiles.weapons.yoyoPros
{
	public class YoyoPros_IceYoyo : ModProjectile
	{
		public override void SetStaticDefaults() {
			// The following sets are only applicable to yoyo that use aiStyle 99.

			// YoyosLifeTimeMultiplier is how long in seconds the yoyo will stay out before automatically returning to the player. 
			// Vanilla values range from 3f (Wood) to 16f (Chik), and defaults to -1f. Leaving as -1 will make the time infinite.
			ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = -1;

			// YoyosMaximumRange is the maximum distance the yoyo sleep away from the player. 
			// Vanilla values range from 130f (Wood) to 400f (Terrarian), and defaults to 200f.
			ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 200f;

			// YoyosTopSpeed is top speed of the yoyo Projectile.
			// Vanilla values range from 9f (Wood) to 17.5f (Terrarian), and defaults to 10f.
			ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 12f;
		}

		public override void SetDefaults() {
			Projectile.width = 26;
			Projectile.height = 26;
			Projectile.aiStyle = ProjAIStyleID.Yoyo;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.MeleeNoSpeed;
			Projectile.penetrate = 1;
		}
        // notes for aiStyle 99: 
        // localAI[0] is used for timing up to YoyosLifeTimeMultiplier
        // localAI[1] can be used freely by specific types
        // ai[0] and ai[1] usually point towards the x and y world coordinate hover point
        // ai[0] is -1f once YoyosLifeTimeMultiplier is reached, when the player is stoned/frozen, when the yoyo is too far away, or the player is no longer clicking the shoot button.
        // ai[0] being negative makes the yoyo move back towards the player
        // Any AI method can be used for dust, spawning projectiles, etc specific to your yoyo.

        public override void OnKill(int timeLeft)
        {
			if(Projectile.owner == Main.myPlayer){
				SoundEngine.PlaySound(SoundID.Item107, Projectile.position);
				for(int i = 0; i < 3; i += 1){
					Vector2 velocity = Vec2.LengthdirDeg(Main.rand.Next(5) + 5, Main.rand.Next(360));
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<YoyoPros_IceYoyo_BreakPiece>(), (int)(Projectile.damage * 0.8), Projectile.knockBack, Projectile.owner, 0.5f);
				}
				Projectile.netUpdate = true;
			}
        }
    }
}
