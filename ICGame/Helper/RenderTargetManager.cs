using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ICGame.Helper
{
    /// <summary>
    /// Zarządza RenderTargetami
    /// </summary>
    public class RenderTargetManager
    {
        private GraphicsDevice graphicsDevice;
        private RenderTarget2D sceneRenderTarget;
        private RenderTarget2D particleTexture;

        public RenderTargetManager(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            ResetRenderTargets();
        }

        public RenderTarget2D ParticleTexture
        {
            get { return particleTexture; }
        }

        public RenderTarget2D SceneRenderTarget
        {
            get { return sceneRenderTarget; }
        }

        public void ResetRenderTargets()
        {
            sceneRenderTarget = new RenderTarget2D(graphicsDevice, graphicsDevice.PresentationParameters.BackBufferWidth,
                                                   graphicsDevice.PresentationParameters.BackBufferHeight, false,
                                                   SurfaceFormat.Color,
                                                   graphicsDevice.PresentationParameters.DepthStencilFormat,
                                                   graphicsDevice.PresentationParameters.MultiSampleCount,
                                                   RenderTargetUsage.PreserveContents);
            particleTexture = new RenderTarget2D(graphicsDevice, graphicsDevice.PresentationParameters.BackBufferWidth/2,
                                                 graphicsDevice.PresentationParameters.BackBufferHeight/2, false,
                                                 SurfaceFormat.Color,
                                                 graphicsDevice.PresentationParameters.DepthStencilFormat);
        }
    }
}
