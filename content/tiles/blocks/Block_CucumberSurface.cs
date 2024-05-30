using Microsoft.Xna.Framework;
using Randomly.content.dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Randomly.content.tiles.blocks
{
	public class Block_CucumberSurface : ModTile
	{
		public override void SetStaticDefaults() {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = true;
			Main.tileBrick[Type] = true;

			HitSound = SoundID.NPCDeath1;
			DustType = ModContent.DustType<Dust_CucumberPiece>();
			
			AddMapEntry(new Color(49, 128, 30));
		}

		public override void NumDust(int i, int j, bool fail, ref int num) {
			num = fail ? 1 : 3;
		}
    }
}