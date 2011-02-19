using System;
using System.Collections.Generic;
using System.Text;
using InterfaceControls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ICGame
{
    public class UserInterfaceDraw : IDrawer
    {
        public UserInterfaceDraw(UserInterface userInterface)
        {
            UserInterface = userInterface;
        }

        public UserInterface UserInterface
        {
            get; set;
        }

        public void Draw(GraphicsDevice graphicsDevice, GameTime gameTime, Vector4? clipPlane = null, float? alpha = null)
        {
            SpriteBatch spriteBatch=new SpriteBatch(graphicsDevice);

            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend);

            foreach (InterfaceControl interfaceControl in UserInterface.Controls)
            {
                interfaceControl.Draw(spriteBatch);
            }
            
            spriteBatch.DrawString(UserInterface.spriteFont, "FPS: " + Game.FPS.ToString(), new Vector2(0, 0), Color.White);
            spriteBatch.End();
        }
    }
}
