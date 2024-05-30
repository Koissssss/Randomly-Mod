using Terraria;

namespace Randomly.common.systems
{
	public class System_BossLifeManager
    {
        public static int Life(float Health)
        {
            float Temp;
            if (Main.masterMode) Temp = (int)(Health * 0.6);
            else if (Main.expertMode) Temp = (int)(Health * 0.7);
            else Temp = Health;
            Temp = (int)(Temp *(0.5 + SystemFuncs.PlayersOnServer() * 0.5));
            return (int)Temp;
        }
    }
}