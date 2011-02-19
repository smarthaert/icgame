using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using InterfaceControls;

namespace ICGame
{
    public enum WindowPosition
    {
        NONE, LEFT, RIGHT, UP, DOWN
    }

    public class UserInterface
    {
    #region Bebechy
		
        private int originalScreenSizeX;
        private int originalScreenSizeY;
        private int screenSizeX;
        private int screenSizeY;
        private int fullscreenSizeX = 1024;
        private int fullscreenSizeY = 768;
        const int THRESHOLD = 5; //Do wykrywania brzegow
        public SpriteFont spriteFont;
        
	#endregion

        #region Controls

        public List<InterfaceControl> Controls { get; set; }

        private Panel rightUI;
        private InterfaceControls.Zune zune;

        private void InitializeControls(GraphicsDevice device)
        {
            Controls = new List<InterfaceControl>();

            //--------------------------------------------------------------------------------------------
            //Right UI Panel
            //--------------------------------------------------------------------------------------------

            rightUI = new Panel();
            rightUI.Texture = GameContentManager.Content.GetTexture("Texture2D/UI/rightUIPanel");
            rightUI.Position = new Vector2(device.PresentationParameters.BackBufferWidth - rightUI.Texture.Width, 0);
            rightUI.Size = new Vector2(rightUI.Texture.Width, device.PresentationParameters.BackBufferHeight);

            Controls.Add(rightUI);


            //--------------------------------------------------------------------------------------------
            //Zune
            //--------------------------------------------------------------------------------------------

            zune = new InterfaceControls.Zune(GameContentManager.Content.GetTexture("Texture2D/UI/zune"), device.PresentationParameters.BackBufferHeight);
            zune.Size = new Vector2(zune.Texture.Width, zune.Texture.Height);

            Controls.Add(zune);
        }

        #endregion

        public UserInterfaceDraw GetDrawer()
        {
            return new UserInterfaceDraw(this);
        }

        /// <summary>
        /// Czy myszka jest nad elementem interfejsu? (zune i sidebar)
        /// </summary>
        /// <param name="x">x myszy</param>
        /// <param name="y">y myszy</param>
        /// <returns></returns>

        public bool InterfaceOverlaped(int x, int y)
        {
            if ((x >= rightUI.Position.X) || zune.InterfaceOverlaped(x, y))
                return true;
            else if (x >= zune.Position.X && x <= zune.Position.X + zune.Size.X && y >= zune.Position.Y && y <= zune.Position.Y + 20)
            {
                zune.ToggleOnClick();
            }
            return false;
        }

        /// <summary>
        /// Inicjalizacja interfejsu
        /// </summary>
        /// <param name="device">GraphicsDevice - tymczasowo</param>

        public void LoadGraphics(GraphicsDevice device)
        {
            screenSizeX = device.PresentationParameters.BackBufferWidth;
            screenSizeY = device.PresentationParameters.BackBufferHeight;

            originalScreenSizeX = screenSizeX;
            originalScreenSizeY = screenSizeY;

            fullscreenSizeY = device.DisplayMode.Height;
            fullscreenSizeX = device.DisplayMode.Width;

            InitializeControls(device);
            spriteFont = GameContentManager.Content.GetFont();
        }

        /// <summary>
        /// Zmienia stan ekranu (fullscren/windowed) i informuje obiekty UI o tej zmianie
        /// </summary>
        public void ToggleFullscreen(bool isFullScreen, GraphicsDeviceManager graphicsDeviceManager)
        {
            if (!isFullScreen)
            {
                screenSizeX = fullscreenSizeX;
                screenSizeY = fullscreenSizeY;
            }
            else
            {
                screenSizeX = originalScreenSizeX;
                screenSizeY = originalScreenSizeY;
            }

            graphicsDeviceManager.PreferredBackBufferWidth = screenSizeX;
            graphicsDeviceManager.PreferredBackBufferHeight = screenSizeY;

            graphicsDeviceManager.ApplyChanges();

            graphicsDeviceManager.ToggleFullScreen();

            UpdateControls();
        }

        private void UpdateControls()
        {
            zune.UpdateScreenSize(screenSizeY);
            rightUI.Position = new Vector2(screenSizeX - rightUI.Size.X, rightUI.Position.Y);
            rightUI.Size = new Vector2(rightUI.Size.X, screenSizeY);
        }

        public WindowPosition GetHorizontalEdge(int x)
        {
            if (x - THRESHOLD < 0)
                return WindowPosition.LEFT;
            if (screenSizeX - x < THRESHOLD)
                return WindowPosition.RIGHT;
            return WindowPosition.NONE;
        }

        public WindowPosition GetVerticalEdge(int y)
        {
            if (y - THRESHOLD < 0)
                return WindowPosition.UP;
            if (screenSizeY - y < THRESHOLD)
                return WindowPosition.DOWN;
            return WindowPosition.NONE;
        }
        
        public void Animate(GameTime gameTime)
        {
            zune.Animate(gameTime);
        }
    }
}
