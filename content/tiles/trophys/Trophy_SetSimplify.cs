using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Randomly.content.tiles.trophys
{
	public class SetFunc
	{
		public static void SetsAll(int Type, ModTile source) {
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileID.Sets.FramesOnKillWall[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
			TileObjectData.addTile(Type);

			source.AddMapEntry(new Color(120, 85, 60), Language.GetText("MapObject.Trophy"));
			source.DustType = 7;
		}
	}
}
