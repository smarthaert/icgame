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
        private Matrix viewMatrix;
        private Matrix projectionMatrix;


        public Display(GraphicsDeviceManager graphicsDeviceManager, UserInterface userInterface, Camera camera, Effect effect)
        {
            graphics = graphicsDeviceManager;
            graphicsDevice = graphics.GraphicsDevice;
            UserInterface = userInterface;
            Camera = camera;
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

        public void Draw()
        {
            graphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            //TYMCZASOWE
            VertexPositionColor[] vertices = new VertexPositionColor[3];
            Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, graphics.GraphicsDevice.Viewport.AspectRatio, 1.0f, 300.0f);
            Matrix view = Camera.CameraMatrix;
            Matrix worldMatrix = Matrix.Identity;

            effect.Parameters["xView"].SetValue(view);
            effect.Parameters["xProjection"].SetValue(projection);
            effect.Parameters["xWorld"].SetValue(worldMatrix);

            vertices[0].Position = new Vector3(-0.5f, -0.5f, 0f);
            vertices[0].Color = Color.Red;
            vertices[1].Position = new Vector3(0, 0.5f, 0f);
            vertices[1].Color = Color.Green;
            vertices[2].Position = new Vector3(0.5f, -0.5f, 0f);
            vertices[2].Color = Color.Yellow;

            VertexDeclaration myVertexDeclaration = new VertexDeclaration(graphics.GraphicsDevice, VertexPositionColor.VertexElements);


            effect.CurrentTechnique = effect.Techniques["Colored"];
            effect.Begin();
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Begin();

                graphics.GraphicsDevice.VertexDeclaration = myVertexDeclaration;
                graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, 1);

                pass.End();
            }
            effect.End();
            //TYMCZASOWE

        }
    }
}
