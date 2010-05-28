using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace ICGame
{
    public class UserInterfaceDraw
    {
        private GraphicsDevice graphicsDevice;
        public UserInterfaceDraw(UserInterface userInterface, GraphicsDevice graphicsDevice)
        {
            this.UserInterface = userInterface;
            this.graphicsDevice = graphicsDevice;
        }

        public UserInterface UserInterface
        {
            get; set;
        }
    
        public void Draw()
        {
            SpriteBatch spriteBatch=new SpriteBatch(graphicsDevice);
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend,SpriteSortMode.Texture,SaveStateMode.SaveState);
            for (int i = 0; i < UserInterface.ScreenSizeY; i++)
            {
                spriteBatch.Draw(UserInterface.rightUI, new Vector2(UserInterface.ScreenSizeX - UserInterface.rightUI.Width, UserInterface.ScreenSizeY - UserInterface.rightUI.Height * i),new Color(1.0f,1.0f,1.0f,1.0f));
            }
            
            spriteBatch.End();
        }
    }
}
