using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
namespace ICGame
{
    public enum WindowPosition
    {
        NONE, LEFT, RIGHT, UP, DOWN
    }

    public class UserInterface
    {
    #region Bebechy
		
        private Game mainGame;

        private int originalScreenSizeX;
        private int originalScreenSizeY;
        private int screenSizeX;
        private int screenSizeY;
        private int fullscreenSizeX = 1024;
        private int fullscreenSizeY = 768;
        const int THRESHOLD = 5; //Do wykrywania brzegow

        private UserInterfaceDraw userInterfaceDraw;

	#endregion

        
        public Texture2D RightUI { get; set; }
        public Zune ZuneUIModel { get; set; }

        public int ScreenSizeY
        {
            get { return screenSizeY; }
        }

        public int ScreenSizeX
        {
            get { return screenSizeX; }
        }

        public bool IsFullscreen
        {
            get
            {
                return mainGame.GraphicsDeviceManager.IsFullScreen;
            }
        }
        
        
        public UserInterfaceDraw Drawer
        {
            get
            {
                return userInterfaceDraw;
            }

        }

        /// <summary>
        /// Czy myszka jest nad elementem interfejsu? (zune i sidebar)
        /// </summary>
        /// <param name="x">x myszy</param>
        /// <param name="y">y myszy</param>
        /// <returns></returns>

        public bool InterfaceOverlaped(int x, int y)
        {
            // Jesli nad prawym paskiem
            if ((x >= ScreenSizeX - RightUI.Width) || ZuneUIModel.InterfaceOverlaped(x, y))
                return true;
            return false;
        }

        /// <summary>
        /// KONIECZNA inicjalizacja modelu (nie wrzucona do konstruktora z powodu ograniczen technicznych - kolejnosc metod w
        /// klasie Game
        /// </summary>
        /// <param name="game">referencja na maingame</param>

        public void LoadGraphics(Game game)
        {
            mainGame = game;
            RightUI = mainGame.Content.Load<Texture2D>("Texture2D/UI/rightUIPanel");
            screenSizeX = mainGame.GraphicsDevice.PresentationParameters.BackBufferWidth;
            screenSizeY = mainGame.GraphicsDevice.PresentationParameters.BackBufferHeight;
            originalScreenSizeX = screenSizeX;
            originalScreenSizeY = screenSizeY;

            ZuneUIModel = new Zune(mainGame.Content.Load<Texture2D>("Texture2D/UI/zune"), ScreenSizeY);
            userInterfaceDraw = new UserInterfaceDraw(this, mainGame.GraphicsDevice);

            fullscreenSizeY = mainGame.GraphicsDevice.DisplayMode.Height;
            fullscreenSizeX = mainGame.GraphicsDevice.DisplayMode.Width;

        }
        /// <summary>
        /// Zmienia stan ekranu (fullscren/windowed) i informuje obiekty UI o tej zmianie
        /// </summary>
        public void ToggleFullscreen()
        {
            if (!mainGame.GraphicsDeviceManager.IsFullScreen)
            {
                screenSizeX = fullscreenSizeX;
                screenSizeY = fullscreenSizeY;
            }
            else
            {
                screenSizeX = originalScreenSizeX;
                screenSizeY = originalScreenSizeY;
            }

            ZuneUIModel.UpdateScreenSize(screenSizeY);
            mainGame.GraphicsDeviceManager.PreferredBackBufferWidth = screenSizeX;
            mainGame.GraphicsDeviceManager.PreferredBackBufferHeight = screenSizeY;


            mainGame.GraphicsDeviceManager.ApplyChanges();

            mainGame.GraphicsDeviceManager.ToggleFullScreen();

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
        
    }
}
