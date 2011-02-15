using System;
using System.Collections.Generic;
using System.IO;
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

        public void Draw(IEnumerable<IDrawer> drawers, Camera camera, GameTime gameTime)
        {
            //TODO: Refactoring - update stanu obiektów raczej nie powinien być tutaj
            foreach (GameObject gameObject in CampaignController.GetObjectsToDraw())
            {
                if (gameObject is IAnimated)
                    ((IAnimated)gameObject).Animate(gameTime, CampaignController.GetObjectsToDraw());
            }
            camera.CalculateCamera();

            graphicsDevice.SetRenderTarget(renderTargetManager.SceneRenderTarget);
            graphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here // O RLY!? :D
            
            //Main scene
            DrawScene(camera, gameTime);
            
            graphicsDevice.SetRenderTarget(renderTargetManager.ParticleTexture);

            graphicsDevice.Clear(ClearOptions.Target, new Color(0.0f, 0.0f, 0.0f, 0.0f), 1.0f, 0);

            //Effects
            DrawScene(camera, gameTime,true);
            
            particleEffectTexturer.Draw(graphicsDevice, CampaignController.GetObjectsToDraw(), camera, DisplayController.Projection, gameTime);
            
            graphicsDevice.SetRenderTarget(null);

            TextureDrawer.DrawMergedTextures(graphicsDevice, renderTargetManager.SceneRenderTarget,
                                             renderTargetManager.ParticleTexture, DisplayController.Projection);

#if !DEBUG

            UserInterface.Drawer.Draw();
#endif
        }

        private void DrawScene(Camera camera, GameTime gameTime, bool creatingEffects = false)
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

            if(!creatingEffects)
            {
                CampaignController.GetTerrainWaterDrawer().Draw(graphicsDevice, gameTime);
            }

            CampaignController.GetBackgroundDrawer().Draw(graphicsDevice, gameTime, null, alpha);

            foreach (GameObject gameObject in CampaignController.GetObjectsToDraw())
            {
                //TODO: CampaignController.GetObjectsToCheck(IPhysical physical) -> lista obiektów które mogą kolidować z animowanym
                gameObject.GetDrawer().Draw(graphicsDevice, gameTime, null, alpha);
            }

            //TODO: Rysowanie malego modelu powinno obslugiwac zaznaczenie wielu jednostek i ogolnie powinno wyleciec do jakiegos kontrolera jednostek (niestniejacego)

            if (!creatingEffects)
            {
                GameObject selectedObject = CampaignController.MissionController.GetSeletedObject();
                if (selectedObject != null)
                {
                    selectedObject.GetMiniModelDrawer().Draw(graphicsDevice, gameTime);
                }
            }
        }
    }
}
