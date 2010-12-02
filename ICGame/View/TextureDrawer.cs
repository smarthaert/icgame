using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ICGame
{
    public class TextureDrawer
    {
        public static void DrawTexture(GraphicsDevice graphicsDevice, Texture2D texture2D, BlendState blendState)
        {
            SpriteBatch spriteBatch = new SpriteBatch(graphicsDevice);

            spriteBatch.Begin(SpriteSortMode.Texture, blendState);
            
            spriteBatch.Draw(texture2D,
                             new Rectangle(0, 0, graphicsDevice.PresentationParameters.BackBufferWidth,
                                           graphicsDevice.PresentationParameters.BackBufferHeight),
                             new Color(1.0f, 1.0f, 1.0f, 1.0f));

            spriteBatch.End();            
        }

        public static void DrawMergedTextures(GraphicsDevice graphicsDevice, Texture2D baseTexture, Texture2D texture1, Matrix projection)
        {
            Effect effect = TechniqueProvider.GetEffect("TextureMerger");

            effect.CurrentTechnique = effect.Techniques["TextureMerger"];
            
            SpriteBatch spriteBatch = new SpriteBatch(graphicsDevice);

            spriteBatch.Begin(SpriteSortMode.Deferred,BlendState.AlphaBlend,null,null,null,effect);

            graphicsDevice.Textures[1] = texture1;

            spriteBatch.Draw(baseTexture, new Rectangle(0, 0, graphicsDevice.PresentationParameters.BackBufferWidth,
                                                        graphicsDevice.PresentationParameters.BackBufferHeight),
                                                        new Color(255, 255, 255, 255));

            spriteBatch.End();
        }
    }
}
