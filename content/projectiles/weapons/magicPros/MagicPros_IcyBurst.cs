using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Randomly.content.dusts;
using Randomly;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.RGB;
using Terraria.ID;
using Terraria.ModLoader;

namespace Randomly.content.projectiles.weapons.magicPros
{
	public class MagicPros_IcyBurst : ModProjectile
	{
		public override string Texture => "Randomly/content/items/weapons/magics/Magic_IcyBurst";

        private float dirSt;

        private float dirSp;

		private Vector2 Center;

        private Vector2 vecCenPast;

		private float FrameCounter {
			get => Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}
		public override void SetStaticDefaults() {
			Main.projFrames[Projectile.type] = 1;

			//ProjectileID.Sets.NeedsUUID[Projectile.type] = true;
			//ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
		}

		public override void SetDefaults() {
			Projectile.width = 60;
			Projectile.height = 60;
            Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.hide = true;
            dirSt = Main.rand.Next(360);
            dirSp = (Main.rand.Next(5) + 5) * (Main.rand.Next(2) * 2 - 1)/10f;
		}

		public override void AI() {
			Player player = Main.player[Projectile.owner];
            FrameCounter++;
            if(FrameCounter == 1){
				SoundEngine.PlaySound(SoundID.Item15, Projectile.position);
                Center = Main.MouseWorld;
                vecCenPast = Center;
            }
            Center += (Main.MouseWorld - Center)*0.1f;
            Projectile.velocity = new(0, 0);

            UpdateDamageForManaSickness(player);

			UpdatePlayerVisuals(player);

            /*
			Vector2 rrp = player.RotatedRelativePoint(player.MountedCenter, true);

			// Update the Prism's damage every frame so that it is dynamically affected by Mana Sickness.
			

			// Update the frame counter.
			FrameCounter += 1f;

			// Update Projectile visuals and sound.
			UpdateAnimation();
			PlaySounds();

			// Update the Prism's position in the world and relevant variables of the player holding it.
            */

			// Update the Prism's behavior: project beams on frame 1, consume mana, and despawn if out of mana.
			if (Projectile.owner == Main.myPlayer) {
				// Slightly re-aim the Prism every frame so that it gradually sweeps to point towards the mouse.
				//UpdateAim(rrp, player.HeldItem.shootSpeed);

				bool stillInUse = player.channel && !player.noItems && !player.CCed;
                //player.channel => 按着鼠标
                //player.noItems => 是否受到诅咒
                //player.CCed => 是否石化，冰冻，受缚

				if (stillInUse && FrameCounter % 2 == 0 && FrameCounter < 120) {
				    CreateDustEffects();
				}
				else if (stillInUse) {
					if(FrameCounter == 120){
						SoundEngine.PlaySound(SoundID.Item107, Projectile.position);
						for(int i = 0; i < 6; i += 1){
							Vector2 velocity = Vec2.LengthdirDeg(20, dirSt + i * 60);
							Projectile.NewProjectile(Projectile.GetSource_FromThis(), Center, velocity, ModContent.ProjectileType<MagicPros_IcyBurst_BurstOut>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0.75f);
						}
						Projectile.netUpdate = true;
					}
					if(FrameCounter == 123){
						for(int i = 0; i < 12; i += 1){
							Vector2 velocity = Vec2.LengthdirDeg(10, dirSt + 15 + i * 30);
							Projectile.NewProjectile(Projectile.GetSource_FromThis(), Center, velocity, ModContent.ProjectileType<MagicPros_IcyBurst_BurstOut>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0.625f);
						}
						Projectile.netUpdate = true;
					}
					if(FrameCounter == 126){
						for(int i = 0; i < 6; i += 1){
							Vector2 velocity = Vec2.LengthdirDeg(5, dirSt + 30 + i * 60);
							Projectile.NewProjectile(Projectile.GetSource_FromThis(), Center, velocity, ModContent.ProjectileType<MagicPros_IcyBurst_BurstOut>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0.5f);
						}
						Projectile.netUpdate = true;
						Projectile.Kill();
					}
				}

				else if (!stillInUse) {
					Projectile.Kill();
				}
			}

			Projectile.timeLeft = 2;
		}
        private void CreateDustEffects(){
            Vector2 vecCen = Center;
            Vector2 vecOff = Vec2.LengthdirDeg(120 - FrameCounter, dirSt + dirSp * FrameCounter) - new Vector2(4, 4);
            Vector2 vecSpe = new Vector2(Main.rand.Next(20)/10f - 1, Main.rand.Next(20)/10f - 1)/2f + (vecCen - vecCenPast)*0.02f;

            int type = ModContent.DustType<Dust_IcePiece>();
            for(int i = 0; i < 6; i++){
                Dust.NewDust(vecCen - new Vector2(8, 8) + vecOff.RotatedBy(i * Math.PI / 3), 8, 8, type, vecSpe.X, vecSpe.Y);
            }
            vecCenPast += (vecCen - vecCenPast)*0.1f;
        }
        private void UpdateDamageForManaSickness(Player player) {
			Projectile.damage = (int)player.GetDamage(DamageClass.Magic).ApplyTo(player.HeldItem.damage);
		}
		private void UpdatePlayerVisuals(Player player) {
			Vector2 vecToTar = Center - player.Center;
			float dir = vecToTar.ToRotation();
			
			Projectile.Center = player.Center + Vec2.LengthdirRad(50, - dir + Math.PI / 2);
			Projectile.direction = Center.X < player.Center.X ? -1 : 1;
			Projectile.spriteDirection = Projectile.direction;
			Projectile.rotation = (float)(dir - Projectile.direction * Math.PI / 4 + Math.PI / 2);
			player.ChangeDir(Projectile.direction);
			player.heldProj = Projectile.whoAmI;
			player.itemTime = 2;
			player.itemAnimation = 2;

			// If you do not multiply by Projectile.direction, the player's hand will point the wrong direction while facing left.
			player.itemRotation = dir * Projectile.direction;
		}
        /*
            private void FireBeams() {
                // If for some reason the beam velocity can't be correctly normalized, set it to a default value.
                Vector2 beamVelocity = Vector2.Normalize(Projectile.velocity);
                if (beamVelocity.HasNaNs()) {
                    beamVelocity = -Vector2.UnitY;
                }

                // This UUID will be the same between all players in multiplayer, ensuring that the beams are properly anchored on the Prism on everyone's screen.
                int uuid = Projectile.GetByUUID(Projectile.owner, Projectile.whoAmI);

                int damage = Projectile.damage;
                float knockback = Projectile.knockBack;
                for (int b = 0; b < NumBeams; ++b) {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Center, beamVelocity, ModContent.ProjectileType<ExampleLastPrismBeam>(), damage, knockback, Projectile.owner, b, uuid);
                }

                // After creating the beams, mark the Prism as having an important network event. This will make Terraria sync its data to other players ASAP.
                Projectile.netUpdate = true;
            }

            // Because the Prism is a holdout Projectile and stays glued to its user, it needs custom drawcode.
            public override bool PreDraw(ref Color lightColor) {
                SpriteEffects effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                Texture2D texture = TextureAssets.Projectile[Type].Value;
                int frameHeight = texture.Height / Main.projFrames[Projectile.type];
                int spriteSheetOffset = frameHeight * Projectile.frame;
                Vector2 sheetInsertPosition = (Center + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition).Floor();

                // The Prism is always at full brightness, regardless of the surrounding light. This is equivalent to it being its own glowmask.
                // It is drawn in a non-white color to distinguish it from the vanilla Last Prism.
                Color drawColor = ExampleLastPrism.OverrideColor;
                Main.EntitySpriteDraw(texture, sheetInsertPosition, new Rectangle?(new Rectangle(0, spriteSheetOffset, texture.Width, frameHeight)), drawColor, Projectile.rotation, new Vector2(texture.Width / 2f, frameHeight / 2f), Projectile.scale, effects, 0f);
                return false;
            }

            private void PlaySounds() {
                // The Prism makes sound intermittently while in use, using the vanilla Projectile variable soundDelay.
                if (Projectile.soundDelay <= 0) {
                    Projectile.soundDelay = SoundInterval;

                    // On the very first frame, the sound playing is skipped. This way it doesn't overlap the starting hiss sound.
                    if (FrameCounter > 1f) {
                        SoundEngine.PlaySound(SoundID.Item15, Projectile.position);
                    }
                }
            }


            private bool ShouldConsumeMana() {
                // If the mana consumption timer hasn't been initialized yet, initialize it and consume mana on frame 1.
                if (ManaConsumptionRate == 0f) {
                    NextManaFrame = ManaConsumptionRate = MaxManaConsumptionDelay;
                    return true;
                }

                // Should mana be consumed this frame?
                bool consume = FrameCounter == NextManaFrame;

                // If mana is being consumed this frame, update the rate of mana consumption and write down the next frame mana will be consumed.
                if (consume) {
                    // MathHelper.Clamp(X,A,B) guarantees that A <= X <= B. If X is outside the range, it will be set to A or B accordingly.
                    ManaConsumptionRate = MathHelper.Clamp(ManaConsumptionRate - 1f, MinManaConsumptionDelay, MaxManaConsumptionDelay);
                    NextManaFrame += ManaConsumptionRate;
                }
                return consume;
            }

            private void UpdateAim(Vector2 source, float speed) {
                // Get the player's current aiming direction as a normalized vector.
                Vector2 aim = Vector2.Normalize(Main.MouseWorld - source);
                if (aim.HasNaNs()) {
                    aim = -Vector2.UnitY;
                }

                // Change a portion of the Prism's current velocity so that it points to the mouse. This gives smooth movement over time.
                aim = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(Projectile.velocity), aim, AimResponsiveness));
                aim *= speed;

                if (aim != Projectile.velocity) {
                    Projectile.netUpdate = true;
                }
                Projectile.velocity = aim;
            }
            */
    }
}