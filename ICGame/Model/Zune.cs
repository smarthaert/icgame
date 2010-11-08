using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ICGame
{
    public enum ZuneState
    {
        Up,
        Down,
        Stop
    }

    public class Zune
    {
        public Zune(Texture2D zuneTexture, int screenSizeY)
        {
            ZuneUI = zuneTexture;
            UpdateScreenSize(screenSizeY);
        }

        /// <summary>
        /// Update po przejściu w tryb fullscreen
        /// </summary>
        /// <param name="screenSizeY">wysokosc ekranu</param>
        public void UpdateScreenSize(int screenSizeY)
        {
            this.screenSizeY = screenSizeY;
            PositionY = screenSizeY - 20;
            PositionX = 30;
            State = ZuneState.Stop;
        }

        private int screenSizeY;

        private int _positionX;
        private int _positionY;
        public ZuneState State { get; set; }
        public Texture2D ZuneUI { get; set; }

        public int PositionX
        {
            get { return _positionX; }
            set { _positionX = value; }
        }

        public int PositionY
        {
            get { return _positionY; }
            set { _positionY = value; }
        }

        public bool IsUp()
        {
            if (PositionY < screenSizeY - 20 && State != ZuneState.Down)
            {
                return true;
            }
            else return false;
        }
        public void Animate(GameTime gameTime)
        {
            if (State != ZuneState.Stop)
            {
                if ((PositionY >= screenSizeY - ZuneUI.Height && State == ZuneState.Up) || (State == ZuneState.Down && PositionY <= screenSizeY - 20))
                {
                    PositionY = PositionY + (int)(gameTime.ElapsedGameTime.Milliseconds * 0.5 * (State == ZuneState.Up ? -1 : 1));
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
}
