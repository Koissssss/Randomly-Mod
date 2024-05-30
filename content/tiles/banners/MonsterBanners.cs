using Microsoft.Xna.Framework.Graphics;
using Randomly.content.npcs.enemys.cucumberStar;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Randomly.content.tiles.banners
{
    public class MonsterBanners : ModTile
	{
		public override void SetStaticDefaults() {
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.CoordinateHeights = [16, 16, 16];
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.StyleWrapLimit = 111;
			TileObjectData.addTile(Type);
			DustType = -1;
		}
		public override void NearbyEffects(int i, int j, bool closer) {
			if (closer) {
				int style = Main.tile[i, j].TileFrameX / 18;
				int EnemyType;
				switch (style) {
					case 0:
						EnemyType = ModContent.NPCType<Enemy_FloatCucumber>();
						break;
					default:
						return;
				}
				Main.SceneMetrics.NPCBannerBuff[EnemyType] = true;
                Main.SceneMetrics.hasBanner = true; 
			}
		}

		public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects) {
			if (i % 2 == 1) {
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
		}
	}
}
