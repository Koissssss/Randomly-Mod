using Microsoft.Xna.Framework;
using Randomly.content.tiles.blocks;
using Randomly.content.walls;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
namespace Randomly.common.systems
{
	// This example shows spawning rubble tiles during world generation.
	public class System_WorldGen : ModSystem
	{
		public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight) {
			// Add a GenPass immediately after the "Piles" pass. ExampleOreSystem explains this approach in more detail.
			int PilesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Spawn Point"));
			if (PilesIndex != -1) {
				tasks.Insert(PilesIndex + 1, new Pass_CucumberStar("Pickled Cucumber Star", 100f));
			}
		}
	}

	public class Pass_CucumberStar : GenPass
	{
		public Pass_CucumberStar(string name, float loadWeight) : base(name, loadWeight) {
		}

		protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration) {
			progress.Message = "使黄瓜之星存在于此世界";

			int[] tileTypes = [ModContent.TileType<Block_Cucumber>(), ModContent.TileType<Block_CucumberSurface>()];
			int[] wallTypes = [ModContent.WallType<Wall_CucumberUnsafe>()];
            int tries = 0;
            int XCenter = 0;
            int YCenter = 0;
            while(tries < 100){
                bool Pass = false;
                tries++;
                XCenter = WorldGen.genRand.Next(-250, 250);
                YCenter = WorldGen.genRand.Next(-10, 10);
                Rectangle TryArea = new(Main.maxTilesX / 2 - 35 + XCenter, (int)((Main.worldSurface * 0.3) - 35 + YCenter), 70, 70);
                if(GenVars.structures.CanPlace(TryArea)){
                    GenVars.structures.AddProtectedStructure(TryArea);
                    Pass = true;
                }
                if(Pass){
                    break;
                }
            }
            for(int k = -30; k <= 30; k++){
                for(int l = -(int)Math.Sqrt(900 - Math.Pow(k, 2)); l <= (int)Math.Sqrt(900 - Math.Pow(k, 2)); l++){
                    int x = Main.maxTilesX / 2 + k + XCenter;
                    int y = (int)(Main.worldSurface * 0.3) + l + YCenter;
                    if(Math.Pow(k, 2) + Math.Pow(l, 2) >= 225){
                        int tileType = tileTypes[0];
                        int placeStyle = 0;
                        WorldGen.PlaceTile(x, y, tileType, mute: true, forced: true, style: placeStyle);
                    }
                    int wallType = wallTypes[0];
                    WorldGen.PlaceWall(x, y, wallType, mute: true);
                    
                }
            }
            for(int k = -35; k <= 35; k++){
                for(int l = -(int)Math.Sqrt(1225 - Math.Pow(k, 2)); l <= (int)Math.Sqrt(1225 - Math.Pow(k, 2)); l++){
                    float len = new Vector2(k, l).Length();
                    float possible;
                    if (len > 29){
                        possible = 500;
                    }
                    else{
                        possible = (int)((len - 25)*100);
                    }
                    if(possible >= WorldGen.genRand.Next(0, 500)){
                        int x = Main.maxTilesX / 2 + k + XCenter;
                        int y = (int)(Main.worldSurface * 0.3) + l + YCenter;
                        if(Math.Pow(k, 2) + Math.Pow(l, 2) >= 225){
                            int tileType = tileTypes[1];
                            int placeStyle = 0;
                            WorldGen.PlaceTile(x, y, tileType, mute: true, forced: true, style: placeStyle);
                        }
                    }
                }
            }
		}
	}
}