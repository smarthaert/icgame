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


        public Display(GraphicsDeviceManager graphicsDeviceManager, UserInterface userInterface, Camera camera, Campaign campaign, Effect effect)
        {
            graphics = graphicsDeviceManager;
            graphicsDevice = graphics.GraphicsDevice;
            graphicsDevice.RenderState.CullMode = CullMode.None;
            UserInterface = userInterface;
            Camera = camera;
            Campaign = campaign;
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

        public Campaign Campaign
        {
            get; set;
        }

        public void Draw(GameTime gameTime)
        {
            graphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, graphics.GraphicsDevice.Viewport.AspectRatio, 1.0f, 300.0f);
            //Matrix view = Camera.CameraMatrix;
            Camera.CalculateCamera();

            foreach (Unit unit in Campaign.UnitContainer.Units)
            {
                unit.Animate(gameTime);
                unit.GetDrawer().Draw(projection,Camera);
            }
            Campaign.Mission.Board.GetDrawer().Draw(graphicsDevice,effect,Camera.CameraMatrix,projection);
        }
    }
}
