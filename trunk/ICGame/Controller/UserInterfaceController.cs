using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace ICGame
{
    public class UserInterfaceController
    {
        private KeyboardState curState,prevState;
        private MouseState mouseCurState,mousePrevState;

        private bool isMouseDragging = false;
        private bool isMouseRotating = false;

        private double lastDraging = 0;
        private int lastDraggingX = 0;
        private int lastDraggingY = 0;

        public Point? LeftMouseButtonClicked { get; private set; }
        public Point? RightMouseButtonClicked { get; private set; }

        //Konieczne do obslugi kursora
        private Game mainGame;

        public void InitializeUserInterface(Game game)
        {
            mainGame = game;
            UserInterface.LoadGraphics(game.GraphicsDevice);
        }

        public UserInterfaceController(CampaignController campaignController, UserInterface userInterface)
        {
            curState = Keyboard.GetState();
            mouseCurState = Mouse.GetState();
            CampaignController = campaignController;
            UserInterface = userInterface;
        }

        public UserInterface UserInterface
        {
            get; set;
        }

        public CampaignController CampaignController
        {
            get;
            set;
        }

        public void UpdateUserInterfaceState(GameTime gameTime)
        {
            UserInterface.Animate(gameTime);
            UpdateInput(gameTime);
        }

        private void UpdateMouseState(GameTime gameTime)
        {
            mousePrevState = mouseCurState;
            mouseCurState = Mouse.GetState();
            
            //-------------------------------------------------------------------------------------
            // Lewy przycisk myszy
            //-------------------------------------------------------------------------------------

            LeftMouseButtonClicked = null;
            if(mouseCurState.LeftButton==ButtonState.Pressed&&mouseCurState!=mousePrevState)
            {
                //Do poprawki
                
                //Okienko informacyjne
                if (!UserInterface.InterfaceOverlaped(mouseCurState.X,mouseCurState.Y))
                {
                    LeftMouseButtonClicked = new Point(mouseCurState.X, mouseCurState.Y);
                }
            }

            //-------------------------------------------------------------------------------------
            // Środkowy przycisk myszy
            //-------------------------------------------------------------------------------------
            
            if (mouseCurState.MiddleButton == ButtonState.Pressed && mouseCurState != mousePrevState)
                isMouseRotating = true;
            else
                isMouseRotating = false;

            if (isMouseRotating)
            {
                DisplayController.Camera.RotateCameraAccordingToMouseTravel(mouseCurState.X - mousePrevState.X, mouseCurState.Y - mousePrevState.Y);
            }

            //-------------------------------------------------------------------------------------
            // Prawy przycisk myszy - dragging
            //-------------------------------------------------------------------------------------

            RightMouseButtonClicked = null;
            if (mouseCurState.RightButton == ButtonState.Pressed && 
                (Math.Abs(mouseCurState.X - mousePrevState.X) > 3 || Math.Abs(mouseCurState.Y - mousePrevState.Y) > 3))
            {
                if (!isMouseDragging)
                {
                    //If turning dragging on... 
                    //rember the mouse coords...
                    lastDraggingX = mouseCurState.X;
                    lastDraggingY = mouseCurState.Y;
                }

                isMouseDragging = true;
                
                mainGame.IsMouseVisible = false;
            }
            else
            {
                //Stop dragging! //Shitty, doesnt work yet
                isMouseDragging = false;
            }

            if(isMouseDragging)
            {
                lastDraging = gameTime.TotalGameTime.TotalMilliseconds;
                DisplayController.Camera.TransformCameraAccordingToMouseTravel(mouseCurState.X-mousePrevState.X,mouseCurState.Y-mousePrevState.Y);
            }

            //-------------------------------------------------------------------------------------
            // Prawy przycisk myszy - kliknięcie
            //-------------------------------------------------------------------------------------

            else 
            {
                if (gameTime.TotalGameTime.TotalMilliseconds - lastDraging > 200)
                {
                    mainGame.IsMouseVisible = true;
                }
                // WYDAWANIE POLECEN JEDNOSTKOM - Tylko jesli minelo pol sekundy od puszczenia prawego    
                // przycisku - zabezpiecza nas przed przypadkowym kliknieciem przy przesuwaniu
                // Sposob scrollowania moze sie zmienic w wersji finalnej (fullscrenowej) na przesuwanie                  
                // po brzegach - wiec aktualnie to troche hotfix

                if (mouseCurState.RightButton == ButtonState.Released && 
                    mousePrevState.RightButton == ButtonState.Pressed && mouseCurState != mousePrevState && 
                    (gameTime.TotalGameTime.TotalMilliseconds - lastDraging) > 500)
                {
                    RightMouseButtonClicked = new Point(mouseCurState.X, mouseCurState.Y);
                }
            }

            //-------------------------------------------------------------------------------------
            // Scroll
            //-------------------------------------------------------------------------------------
            
            DisplayController.Camera.ChangeHeightAccordingToMouseWheel(mouseCurState.ScrollWheelValue - mousePrevState.ScrollWheelValue);

            //-------------------------------------------------------------------------------------
            // Przewijanie na brzegach
            //-------------------------------------------------------------------------------------

            if (IsFullscreen)
            {
                if (UserInterface.GetHorizontalEdge(mouseCurState.X) == WindowPosition.LEFT)
                    DisplayController.Camera.MoveLeft();
                else if (UserInterface.GetHorizontalEdge(mouseCurState.X) == WindowPosition.RIGHT)
                    DisplayController.Camera.MoveRight();

                if (UserInterface.GetVerticalEdge(mouseCurState.Y) == WindowPosition.UP)
                    DisplayController.Camera.MoveForward();
                else if (UserInterface.GetVerticalEdge(mouseCurState.Y) == WindowPosition.DOWN)
                    DisplayController.Camera.MoveBack();
            }

        }

        public bool IsFullscreen
        {
            get
            {
                return mainGame.GraphicsDeviceManager.IsFullScreen;
            }
        }

        private void UpdateKeyboardState()
        {
            prevState = curState;
            curState = Keyboard.GetState();

            if (curState.IsKeyDown(Keys.Escape) && prevState.IsKeyUp(Keys.Escape))
            {
                CampaignController.CampaignState = GameState.Exit;
            }
            
            if(curState.IsKeyDown(Keys.Z))
            {
                DisplayController.Camera.MoveLeft();
            }
            
            if (curState.IsKeyDown(Keys.X))
            {
                DisplayController.Camera.MoveRight();
            }

            if (curState.IsKeyDown(Keys.Up))
            {
                DisplayController.Camera.MoveForward();
            }
            
            if (curState.IsKeyDown(Keys.Down))
            {
                DisplayController.Camera.MoveBack();
            }

            if (curState.IsKeyDown(Keys.Right))
            {
                DisplayController.Camera.RotateRight();
            }

            if (curState.IsKeyDown(Keys.Left))
            {
                DisplayController.Camera.RotateLeft();
            }

            if (curState.IsKeyDown(Keys.Q))
            {
                DisplayController.Camera.MoveUp();
            }

            if (curState.IsKeyDown(Keys.A))
            {
                DisplayController.Camera.MoveDown();
            }

            if (curState.IsKeyDown(Keys.E))
            {
                CampaignController.EffectController.SetEffects(true);
            }

            if (curState.IsKeyDown(Keys.R))
            {
                CampaignController.EffectController.SetEffects(false);
            }

            if (curState.IsKeyDown(Keys.F6) && prevState.IsKeyUp(Keys.F6))
            {
                UserInterface.ToggleFullscreen(IsFullscreen, mainGame.GraphicsDeviceManager);
            }
        }

        public void UpdateInput(GameTime gameTime)
        {
            UpdateMouseState(gameTime);
            UpdateKeyboardState();
        }
    }
}
