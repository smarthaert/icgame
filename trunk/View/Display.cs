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

        private Effect effect;


        public Display(GraphicsDeviceManager graphicsDeviceManager, UserInterface userInterface, Camera camera, CampaignController campaignController, Effect effect)
        {
            graphics = graphicsDeviceManager;
            graphicsDevice = graphics.GraphicsDevice;
            graphicsDevice.RenderState.CullMode = CullMode.None;
            UserInterface = userInterface;
            Camera = camera;
            CampaignController = campaignController;
            this.effect = effect;
        }

        public CampaignController CampaignController
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

     
        public void Draw(GameTime gameTime)
        {
            graphicsDevice.Clear(Color.CornflowerBlue);
            Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, graphics.GraphicsDevice.Viewport.AspectRatio, 1.0f, 300.0f);
            Camera.CalculateCamera();

            foreach (GameObject gameObject in CampaignController.GetObjectsToDraw())
            {
                if(gameObject is IAnimated) 
                ((IAnimated)gameObject).Animate(gameTime);
                gameObject.GetDrawer().Draw(projection,Camera,graphicsDevice);
            }
            CampaignController.GetBackgroundDrawer().Draw(graphicsDevice,effect,Camera.CameraMatrix,projection);
            UserInterface.Drawer.Draw();

        }
    }
}
