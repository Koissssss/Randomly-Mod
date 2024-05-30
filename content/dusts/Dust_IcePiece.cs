//TODO:更改混色模式
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Randomly.content.dusts
{
	public class Dust_IcePiece : ModDust
	{
        public override void OnSpawn(Dust dust) {
			dust.scale *= 0.75f;
			dust.frame = new Rectangle(0, 0, 16, 16);
		}

		public override bool Update(Dust dust) {
			dust.position += dust.velocity;
			dust.scale *= 0.95f;

			if (dust.scale < 0.05f) {
				dust.active = false;
			}

			return false;
		}
    }
}
