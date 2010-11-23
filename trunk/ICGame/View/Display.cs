using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ICGame
{
    public class Display
    {
        private GraphicsDeviceManager graphics;
        private GraphicsDevice graphicsDevice;

        public Display(GraphicsDeviceManager graphicsDeviceManager, UserInterface userInterface, Camera camera, CampaignController campaignController)
        {
            graphics = graphicsDeviceManager;
            graphicsDevice = graphics.GraphicsDevice;
         //   graphicsDevice.RenderState.CullMode = CullMode.None;
            UserInterface = userInterface;
            Camera = camera;
            CampaignController = campaignController;
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, graphics.GraphicsDevice.Viewport.AspectRatio, 1.0f, 600.0f);
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

        public void Draw(GameTime gameTime)
        {
            //TODO: Refakcoring - update stanu obiektów raczej nie powinien być tutaj
            foreach (GameObject gameObject in CampaignController.GetObjectsToDraw())
            {
                if (gameObject is IAnimated)
                    ((IAnimated)gameObject).Animate(gameTime, CampaignController.GetObjectsToDraw());
            }
            graphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here // O RLY!? :D
            graphicsDevice.DepthStencilState = DepthStencilState.Default;
            graphicsDevice.RasterizerState = RasterizerState.CullClockwise;
            Camera.CalculateCamera();
            CampaignController.GetTerrainWaterDrawer().Draw(graphicsDevice, CampaignController.GetObjectsToDraw(), gameTime);
            CampaignController.GetBackgroundDrawer().Draw(graphicsDevice,Camera.CameraMatrix,Projection,Camera.CameraPosition,null);

            foreach (GameObject gameObject in CampaignController.GetObjectsToDraw())
            {
                //TODO: CampaignController.GetObjectsToCheck(IPhysical physical) -> lista obiektów które mogą kolidować z animowanym
                gameObject.GetDrawer().Draw(Projection, Camera.CameraMatrix, Camera.CameraPosition, graphicsDevice, null);
                
            }

            foreach (GameObject gameObject in CampaignController.GetObjectsToDraw())
            {
                foreach (IObjectEffect objectEffect in gameObject.GetEffectsToDraw())
                {
                    objectEffect.GetDrawer().Draw(Projection, Camera, graphicsDevice, gameTime);
                }
            }

            //TODO: Rysowanie malego modelu powinno obslugiwac zaznaczenie wielu jednostek i ogolnie powinno wyleciec do jakiegos kontrolera jednostek (niestniejacego)
            GameObject selectedObject = CampaignController.MissionController.GetSeletedObject();
            if (selectedObject != null)
            {
                selectedObject.GetDrawer().DrawSmallModel(Projection, Camera, graphicsDevice,gameTime);
            }

            UserInterface.Drawer.Draw();
        }
    }
}
