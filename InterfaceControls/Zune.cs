using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace InterfaceControls
{
    public enum ZuneState
    {
        Up,
        Down,
        Stop
    }

    public class Zune : InterfaceControl
    {
        public Zune(Texture2D zuneTexture, int screenSizeY)
        {
            Texture = zuneTexture;
            UpdateScreenSize(screenSizeY);
        }

        /// <summary>
        /// Update po przejściu w tryb fullscreen
        /// </summary>
        /// <param name="screenSizeY">wysokosc ekranu</param>
        public void UpdateScreenSize(int screenSizeY)
        {
            this.screenSizeY = screenSizeY;
            Position = new Vector2(30, screenSizeY - 20);
            State = ZuneState.Stop;
        }

        private int screenSizeY;

        private ZuneState State { get; set; }
        public Texture2D Texture { get; set; }

        private bool IsUp()
        {
            if (Position.Y < screenSizeY - 20 && State != ZuneState.Down)
            {
                return true;
            }
            else return false;
        }

        public void Animate(GameTime gameTime)
        {
            if (State != ZuneState.Stop)
            {
                if ((Position.Y >= screenSizeY - Texture.Height && State == ZuneState.Up) || (State == ZuneState.Down && Position.Y <= screenSizeY - 20))
                {
                    Position = new Vector2(Position.X, Position.Y + (int)(gameTime.ElapsedGameTime.Milliseconds * 0.5 * (State == ZuneState.Up ? -1 : 1)));
                }
                else
                {
                    if (State == ZuneState.Down)
                        Position = new Vector2(Position.X, screenSizeY - 20);
                    else
                        Position = new Vector2(Position.X, screenSizeY - Texture.Height);
                    State = ZuneState.Stop;
                }
            }
        }

        public bool InterfaceOverlaped(int x, int y)
        {
            if (x >= Position.X && x <= Position.X + Size.X)
                if (y >= Position.Y + 20)
                    return true;
            return false;

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.Draw(Texture, Position, new Color(1.0f, 1.0f, 1.0f, 1.0f));
        }

        public void ToggleOnClick()
        {
            State = IsUp() ? ZuneState.Down : ZuneState.Up;
        }

    }
}
