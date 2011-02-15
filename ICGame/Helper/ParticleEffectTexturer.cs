using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ICGame.Helper
{
    /// <summary>
    /// Klasa pomocnicza renderująca efekty cząsteczkowe do tekstury
    /// </summary>
    public class ParticleEffectTexturer
    {
        /// <summary>
        /// Inicjalizacja
        /// </summary>
        public ParticleEffectTexturer()
        {
            
        }

        public void Draw(GraphicsDevice graphicsDevice, IEnumerable<IDrawer> particleEffectDrawers, GameTime gameTime)
        {
            graphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
            graphicsDevice.RasterizerState = RasterizerState.CullNone;
            graphicsDevice.BlendState = BlendState.AlphaBlend;

            //TODO: Sortowanie efektów

            foreach (IParticleEffectDrawer particleEffectDrawer in particleEffectDrawers)
            {
                particleEffectDrawer.Draw(graphicsDevice, gameTime);
            }
            
            graphicsDevice.DepthStencilState = DepthStencilState.Default;

            /*using (FileStream fileStream = File.OpenWrite("effects.png"))
            {
                ParticleTexture.SaveAsPng(fileStream, ParticleTexture.Width, ParticleTexture.Height);
                fileStream.Close();
            }*/
        }
    }
}
