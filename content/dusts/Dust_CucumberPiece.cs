using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Randomly.content.dusts
{
	public class Dust_CucumberPiece : ModDust
	{
		public override void OnSpawn(Dust dust) {
            
			dust.velocity *= 0.4f; // Multiply the dust's start velocity by 0.4, slowing it down
			dust.scale *= 0.25f; // Multiplies the dust's initial scale by 1.5.
			dust.frame = new Rectangle(0, 0, 20, 20);
			// If our texture had 3 different dust on top of each other (a 30x90 pixel image), we might do this:
			// dust.frame = new Rectangle(0, Main.rand.Next(3) * 30, 30, 30);
		}

		public override bool Update(Dust dust) { // Calls every frame the dust is active
			dust.position += dust.velocity;
			dust.scale *= 0.99f;

			if (dust.scale < 0.05f) {
				dust.active = false;
			}

			return false; // Return false to prevent vanilla behavior.
		}
	}
}
