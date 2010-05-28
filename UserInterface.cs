using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ICGame
{
    public class UserInterface
    {
        public Texture2D rightUI;
        private int screenSizeX;
        private int screenSizeY;
        private UserInterfaceDraw userInterfaceDraw;
        
        
        public UserInterface(Campaign campaign)
        {
            Campaign = campaign;
          
        }


        public int ScreenSizeY
        {
            get { return screenSizeY; }
        }

        public int ScreenSizeX
        {
            get { return screenSizeX; }
        }

        public Campaign Campaign
        {
            get; set;
        }

        public UserInterfaceDraw Drawer
        {
            get
            {
                return userInterfaceDraw;
            }
            
        }

        public void LoadGraphics(Game game)
        {
            rightUI=game.Content.Load<Texture2D>("rightUIPanel");
            screenSizeX = game.GraphicsDevice.PresentationParameters.BackBufferWidth;
            screenSizeY = game.GraphicsDevice.PresentationParameters.BackBufferHeight;
            userInterfaceDraw=new UserInterfaceDraw(this,game.GraphicsDevice);

        }

    }
}
