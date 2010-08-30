using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
namespace ICGame
{
    public enum ZuneState
    {
        Up,
        Down,
        Stop
    } 

    public class UserInterface
    {
        public class Zune
        {
            public Zune(Texture2D zuneTexture,int screenSizeY)
            {
                ZuneUI = zuneTexture;
                this.screenSizeY = screenSizeY;
                PositionY = screenSizeY-20;
                State = ZuneState.Stop;
            }

            private int screenSizeY = 0;
            public Texture2D ZuneUI{ get; set; } 
            public int PositionX=30;
            public int PositionY=0;
            public ZuneState State { get; set; }

            public bool IsUp()
            {
                if(PositionY<screenSizeY-20&&State!=ZuneState.Down)
                {
                    return true;
                }
                else return false;
            }
            public void Animate(GameTime gameTime)
            {
                if(State!=ZuneState.Stop)
                {
                    if((PositionY>=screenSizeY-ZuneUI.Height&&State==ZuneState.Up)||(State==ZuneState.Down&&PositionY<=screenSizeY-20))
                    {
                        PositionY += (int)(gameTime.ElapsedGameTime.Milliseconds*0.5* (State==ZuneState.Up ? -1 : 1));
                    }
                    else
                    {
                        if (State == ZuneState.Down)
                            PositionY = screenSizeY - 20;
                        else
                            PositionY = screenSizeY - ZuneUI.Height;
                        State = ZuneState.Stop;
                    }
                }
            }
            public bool InterfaceOverlaped(int x, int y)
            {
                if (x >= PositionX && x <= PositionX + ZuneUI.Width)
                    if (y >= PositionY)
                        return true;
                return false;

            }

        }

        public Texture2D RightUI { get; set; }
        public Zune ZuneUIModel;

        private int screenSizeX;
        private int screenSizeY;
        private UserInterfaceDraw userInterfaceDraw;


        public bool InterfaceOverlaped(int x, int y)
        {
            // Jesli nad prawym paskiem
            if ((x >= ScreenSizeX - RightUI.Width) || ZuneUIModel.InterfaceOverlaped(x, y)) 
                return true;
            return false;
        }   
     
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
            RightUI=game.Content.Load<Texture2D>("Texture2D/UI/rightUIPanel");
            screenSizeX = game.GraphicsDevice.PresentationParameters.BackBufferWidth;
            screenSizeY = game.GraphicsDevice.PresentationParameters.BackBufferHeight;
            ZuneUIModel = new Zune(game.Content.Load<Texture2D>("Texture2D/UI/zune"),ScreenSizeY);
            userInterfaceDraw=new UserInterfaceDraw(this,game.GraphicsDevice);

        }

    }
}
