using System;
using Microsoft.Xna.Framework;
using Terraria;

namespace Randomly
{
    public class SystemFuncs
    {
        public static int PlayersOnServer(){
            int num = 0;
            foreach (var player in Main.ActivePlayers) {
                num++;
            }
            return num;
        }

    }
}