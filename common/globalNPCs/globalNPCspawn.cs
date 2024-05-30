using Randomly.content.biomes;
using Terraria;
using Terraria.ModLoader;

namespace Randomly.content.globalNPCs
{
    public class globalNPCspawn : GlobalNPC
    {
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            if(player.InModBiome(ModContent.GetInstance<Biome_CucumberStar>())){
                if(spawnRate > 100){
                    spawnRate = 100;
                }
                if(maxSpawns < 10){
                    maxSpawns = 10;
                }
            }
        }
        
    }
}