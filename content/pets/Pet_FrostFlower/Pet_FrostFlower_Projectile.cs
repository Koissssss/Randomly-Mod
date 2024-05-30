using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Randomly.content.pets.Pet_FrostFlower
{
	public class Pet_FrostFlower_Projectile : ModProjectile
	{
		public override void SetStaticDefaults() {
			Main.projFrames[Projectile.type] = 24;
			Main.projPet[Projectile.type] = true;

			// This code is needed to customize the vanity pet display in the player select screen. Quick explanation:
			// * It uses fluent API syntax, just like Recipe
			// * You start with ProjectileID.Sets.SimpleLoop, specifying the start and end frames as well as the speed, and optionally if it should animate from the end after reaching the end, effectively "bouncing"
			// * To stop the animation if the player is not highlighted/is standing, as done by most grounded pets, add a .WhenNotSelected(0, 0) (you can customize it just like SimpleLoop)
			// * To set offset and direction, use .WithOffset(x, y) and .WithSpriteDirection(-1)
			// * To further customize the behavior and animation of the pet (as its AI does not run), you have access to a few vanilla presets in DelegateMethods.CharacterPreview to use via .WithCode(). You can also make your own, showcased in MinionBossPetProjectile
			ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(0, Main.projFrames[Projectile.type], 6)
				.WithOffset(-10, -20f)
				.WithSpriteDirection(-1)
				.WithCode(DelegateMethods.CharacterPreview.Float);
		}

		public override void SetDefaults() {
			Projectile.width = 44;
			Projectile.height = 64;
			//AIType = ProjectileID.ZephyrFish;
		}

		public override bool PreAI() {
			Player player = Main.player[Projectile.owner];
			player.zephyrfish = false;
			return true;
		}

		public override void AI() {
			Player player = Main.player[Projectile.owner];
			if (!player.dead && player.HasBuff(ModContent.BuffType<Pet_FrostFlower_Buff>())) {
				Projectile.timeLeft = 2;
			}
			Vector2 position = Projectile.position;
			int width = Projectile.width;
			int height = Projectile.height;

			float num0 = 0.3f;
			Projectile.tileCollide = false;
			int num1 = 100;
			float num2 = 50f;
			float num3 = 2000f;
			bool flag = false;
			Vector2 vector = new Vector2(position.X + width * 0.5f, position.Y + height * 0.5f);
			float num4 = player.position.X + player.width / 2 - vector.X;
			float num5 = player.position.Y + player.height / 2 - vector.Y;

			num5 += Main.rand.Next(-10, 21);
			num4 += Main.rand.Next(-10, 21);
			num4 += 60 * -player.direction;
			num5 -= 60f;

			float num6 = (float)Math.Sqrt(num4 * num4 + num5 * num5);
			float num7 = 6f;

			if (num6 < num1 && player.velocity.Y == 0f && position.Y + height <= player.position.Y + player.height && !Collision.SolidCollision(position, width, height)) {
				Projectile.ai[0] = 0f;
				if (Projectile.velocity.Y < -6f)
					Projectile.velocity.Y = -6f;
			}

			if (num6 < num2) {
				if (Math.Abs(Projectile.velocity.X) > 2f || Math.Abs(Projectile.velocity.Y) > 2f) {
					Projectile.velocity *= 0.99f;
				}

				num0 = 0.01f;
			}
			else {
				if (num6 < 100f)
					num0 = 0.1f;
				if (num6 > 300f)
					num0 = 0.4f;
				if (num6 > num3)
					flag = true;
				num6 = num7 / num6;
				num4 *= num6;
				num5 *= num6;
			}

			if (Projectile.velocity.X < num4) {
				Projectile.velocity.X += num0;
				if (num0 > 0.05f && Projectile.velocity.X < 0f)
					Projectile.velocity.X += num0;
			}

			if (Projectile.velocity.X > num4) {
				Projectile.velocity.X -= num0;
				if (num0 > 0.05f && Projectile.velocity.X > 0f)
					Projectile.velocity.X -= num0;
			}

			if (Projectile.velocity.Y < num5) {
				Projectile.velocity.Y += num0;
				if (num0 > 0.05f && Projectile.velocity.Y < 0f)
					Projectile.velocity.Y += num0 * 2f;
			}

			if (Projectile.velocity.Y > num5) {
				Projectile.velocity.Y -= num0;
				if (num0 > 0.05f && Projectile.velocity.Y > 0f)
					Projectile.velocity.Y -= num0 * 2f;
			}

			Projectile.spriteDirection = -Projectile.direction;
			Projectile.rotation = Projectile.velocity.X * 0.05f;


			if (flag) {
				int num8 = 33;

				for (int k = 0; k < 12; k++) {
					float speedX3 = 1f - Main.rand.NextFloat() * 2f;
					float speedY3 = 1f - Main.rand.NextFloat() * 2f;
					int num9 = Dust.NewDust(position, width, height, num8, speedX3, speedY3);
					Main.dust[num9].noLightEmittence = true;
					Main.dust[num9].noGravity = true;
				}

				Projectile.Center = player.Center;
				Projectile.velocity = Vector2.Zero;
				if (Main.myPlayer == Projectile.owner)
					Projectile.netUpdate = true;
			}
			
			if (++Projectile.frameCounter >= 10) {
				Projectile.frameCounter = 0;
				Projectile.frame++;
				if (Projectile.frame < 0 || Projectile.frame > 23)
					Projectile.frame = 0;

				return;
			}
		}
	}
}
