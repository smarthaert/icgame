using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ICGame.Helper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ICGame
{
    public class Display
    {
        private GraphicsDeviceManager graphics;
        private GraphicsDevice graphicsDevice;
        private ParticleEffectTexturer particleEffectTexturer;
        private RenderTargetManager renderTargetManager;

        public Display(GraphicsDeviceManager graphicsDeviceManager, UserInterface userInterface, CampaignController campaignController)
        {
            graphics = graphicsDeviceManager;
            graphicsDevice = graphics.GraphicsDevice;
            
            UserInterface = userInterface;
            CampaignController = campaignController;
            particleEffectTexturer = new ParticleEffectTexturer();
            renderTargetManager = new RenderTargetManager(graphicsDevice);

            graphicsDeviceManager.DeviceReset += new EventHandler<EventArgs>(DeviceReset);
        }

        void DeviceReset(object sender, EventArgs e)
        {
            renderTargetManager.ResetRenderTargets();
        }

        public UserInterface UserInterface
        {
            get; set;
        }

        public CampaignController CampaignController

        {
            get; set;
        }

        public void Draw(IEnumerable<IDrawer> drawers, GameTime gameTime)
        {
            //TODO: Refactoring - update stanu obiektów raczej nie powinien być tutaj
            foreach (GameObject gameObject in CampaignController.GetObjectsToDraw())
            {
                if (gameObject is IAnimated)
                    ((IAnimated)gameObject).Animate(gameTime, CampaignController.GetObjectsToDraw());
            }
            DisplayController.Camera.CalculateCamera();

            graphicsDevice.SetRenderTarget(renderTargetManager.SceneRenderTarget);
            graphicsDevice.Clear(Color.Black);

            //Main scene
            DrawScene(drawers, gameTime);
            
            graphicsDevice.SetRenderTarget(renderTargetManager.ParticleTexture);

            graphicsDevice.Clear(ClearOptions.Target, new Color(0.0f, 0.0f, 0.0f, 0.0f), 1.0f, 0);

            //Effects
            DrawScene(drawers, gameTime,true);
            
            particleEffectTexturer.Draw(graphicsDevice, drawers.Where(s => s is IParticleEffectDrawer), gameTime);
            
            graphicsDevice.SetRenderTarget(null);

            TextureDrawer.DrawMergedTextures(graphicsDevice, renderTargetManager.SceneRenderTarget,
                                             renderTargetManager.ParticleTexture, DisplayController.Projection);

#if !DEBUG
            foreach (IDrawer drawer in drawers.Where(s => s is UserInterfaceDraw))
            {
                drawer.Draw(graphicsDevice, gameTime);
            }
#endif
        }

        private void DrawScene(IEnumerable<IDrawer> drawers, GameTime gameTime, bool creatingEffects = false)
        {
            graphicsDevice.DepthStencilState = DepthStencilState.Default;
            graphicsDevice.RasterizerState = RasterizerState.CullClockwise;
            graphicsDevice.BlendState = BlendState.AlphaBlend;

            //Set alpha for effects drawing
            float? alpha = null;
            if(creatingEffects)
            {
                alpha = 1.0f;
            }

            if(creatingEffects)
            {
                foreach (IDrawer drawer in drawers.Where(s => !(s is UserInterfaceDraw || s is TerrainWaterDrawer || s is MiniModelDrawer)))
                {
                    drawer.Draw(graphicsDevice, gameTime, null, alpha);
                }
            }
            else
            {
                foreach (IDrawer drawer in drawers.Where(s => !(s is UserInterfaceDraw)))
                {
                    drawer.Draw(graphicsDevice, gameTime, null, alpha);
                }
            }
        }
    }
}
