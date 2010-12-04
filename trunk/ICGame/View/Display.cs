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

        public Display(GraphicsDeviceManager graphicsDeviceManager, UserInterface userInterface, Camera camera, CampaignController campaignController)
        {
            graphics = graphicsDeviceManager;
            graphicsDevice = graphics.GraphicsDevice;
            
            UserInterface = userInterface;
            Camera = camera;
            CampaignController = campaignController;
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, graphics.GraphicsDevice.Viewport.AspectRatio, 1.0f, 600.0f);
            particleEffectTexturer = new ParticleEffectTexturer();
            renderTargetManager = new RenderTargetManager(graphicsDevice);

            graphicsDeviceManager.DeviceReset += new EventHandler<EventArgs>(DeviceReset);
        }

        void DeviceReset(object sender, EventArgs e)
        {
            renderTargetManager.ResetRenderTargets();
        }

        public ContentManager Content
        {
            get; set;
        }

        public UserInterface UserInterface
        {
            get; set;
        }

        public Camera Camera
        {
            get; set;
        }

        public CampaignController CampaignController

        {
            get; set;
        }

        public Matrix Projection
        {
            get;
            set;
        }

        public void Draw(GameTime gameTime, string fps)
        {
            //TODO: Refactoring - update stanu obiektów raczej nie powinien być tutaj
            foreach (GameObject gameObject in CampaignController.GetObjectsToDraw())
            {
                if (gameObject is IAnimated)
                    ((IAnimated)gameObject).Animate(gameTime, CampaignController.GetObjectsToDraw());
            }
            Camera.CalculateCamera();

            graphicsDevice.SetRenderTarget(renderTargetManager.SceneRenderTarget);
            graphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here // O RLY!? :D
            
            //Main scene
            DrawScene(gameTime);
            
            graphicsDevice.SetRenderTarget(renderTargetManager.ParticleTexture);

            graphicsDevice.Clear(ClearOptions.Target, new Color(0.0f, 0.0f, 0.0f, 0.0f), 1.0f, 0);

            //Effects
            DrawScene(gameTime,true);
            
            particleEffectTexturer.Draw(graphicsDevice, CampaignController.GetObjectsToDraw(), Camera, Projection, gameTime);
            
            graphicsDevice.SetRenderTarget(null);

            TextureDrawer.DrawMergedTextures(graphicsDevice, renderTargetManager.SceneRenderTarget, renderTargetManager.ParticleTexture, Projection);

#if !DEBUG

            UserInterface.Drawer.Draw(fps);
#endif
        }

        private void DrawScene(GameTime gameTime, bool creatingEffects = false)
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
                CampaignController.GetTerrainWaterDrawer().Draw(graphicsDevice, CampaignController.GetObjectsToDraw(), gameTime);
            }

            CampaignController.GetBackgroundDrawer().Draw(graphicsDevice, Camera.CameraMatrix, Projection, Camera.CameraPosition, null, creatingEffects, alpha);

            foreach (GameObject gameObject in CampaignController.GetObjectsToDraw())
            {
                //TODO: CampaignController.GetObjectsToCheck(IPhysical physical) -> lista obiektów które mogą kolidować z animowanym
                gameObject.GetDrawer().Draw(Projection, Camera.CameraMatrix, Camera.CameraPosition, graphicsDevice, null, alpha);
            }

            //TODO: Rysowanie malego modelu powinno obslugiwac zaznaczenie wielu jednostek i ogolnie powinno wyleciec do jakiegos kontrolera jednostek (niestniejacego)

            if (!creatingEffects)
            {
                GameObject selectedObject = CampaignController.MissionController.GetSeletedObject();
                if (selectedObject != null)
                {
                    selectedObject.GetDrawer().DrawSmallModel(Projection, Camera, graphicsDevice, gameTime);
                }
            }
        }
    }
}
