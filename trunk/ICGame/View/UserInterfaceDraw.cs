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
            //spriteBatch.Begin(SpriteBlendMode.AlphaBlend,SpriteSortMode.Texture,SaveStateMode.SaveState);
            //CONV
            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend);
            for (int i = 0; i < UserInterface.ScreenSizeY; i++)
            {
                spriteBatch.Draw(UserInterface.RightUI, new Vector2(UserInterface.ScreenSizeX - UserInterface.RightUI.Width, UserInterface.ScreenSizeY - UserInterface.RightUI.Height * i),new Color(1.0f,1.0f,1.0f,1.0f));
            }
            spriteBatch.Draw(UserInterface.ZuneUIModel.ZuneUI, new Vector2(UserInterface.ZuneUIModel.PositionX, UserInterface.ZuneUIModel.PositionY), new Color(1.0f, 1.0f, 1.0f, 1.0f));
            spriteBatch.DrawString(UserInterface.spriteFont, "FPS: " + Game.FPS.ToString(), new Vector2(0, 0), Color.White);
            spriteBatch.End();
        }
    }
}
