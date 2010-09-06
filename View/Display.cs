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
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, graphics.GraphicsDevice.Viewport.AspectRatio, 1.0f, 600.0f);
            this.effect = effect;

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
            graphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            Camera.CalculateCamera();
            CampaignController.GetBackgroundDrawer().Draw(graphicsDevice,effect,Camera.CameraMatrix,Projection,Camera.CameraPosition);
            foreach (GameObject gameObject in CampaignController.GetObjectsToDraw())
            {
                if(gameObject is IAnimated) 
                ((IAnimated)gameObject).Animate(gameTime);
                gameObject.GetDrawer().Draw(Projection,Camera,graphicsDevice);
            }
            
            UserInterface.Drawer.Draw();

        }
    }
}
