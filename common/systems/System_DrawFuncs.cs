using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;

namespace Randomly{
    public class Draw{
        private static Asset<Texture2D> texPixel = ModContent.Request<Texture2D>("Randomly/Assets/Textures/Draw/OnePixel", AssetRequestMode.AsyncLoad);
        public static void DrawLine(SpriteBatch spriteBatch, Vector2 screenPos, Vector2 startPos, Vector2 endPos, Color drawColor){
            spriteBatch.Draw(texPixel.Value, (startPos + endPos)/2 - screenPos, new Rectangle(0, 0, 2, 2), drawColor, (endPos - startPos).ToRotation(), new Vector2(1, 1), new Vector2((endPos - startPos).Length()/2, 1), SpriteEffects.None, 0f);
        }
    }
}