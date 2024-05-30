using Randomly.content.tiles.blocks;
using System;
using Terraria.ModLoader;

namespace Randomly.common.systems
{
	public class System_CucumberTileCount : ModSystem
	{
		public int CucumberTileCount;

		public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts) {
			CucumberTileCount = tileCounts[ModContent.TileType<Block_Cucumber>()];
		}
	}
}
