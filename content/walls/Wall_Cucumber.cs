using Randomly.content.dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Randomly.content.walls
{
	public class Wall_Cucumber : ModWall
	{
		public override void SetStaticDefaults() {
			Main.wallHouse[Type] = true;

			DustType = ModContent.DustType<Dust_CucumberPiece>();

			AddMapEntry(new Color(138, 164, 63));
		}

		public override void NumDust(int i, int j, bool fail, ref int num) {
			num = fail ? 1 : 3;
		}
	}
	public class Wall_CucumberUnsafe : ModWall
	{
		public override void SetStaticDefaults() {
			Main.wallHouse[Type] = false;

			DustType = ModContent.DustType<Dust_CucumberPiece>();

			AddMapEntry(new Color(138, 164, 63));
		}

		public override void NumDust(int i, int j, bool fail, ref int num) {
			num = fail ? 1 : 3;
		}
	}
}