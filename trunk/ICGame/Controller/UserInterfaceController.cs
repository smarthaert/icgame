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

        //Konieczne do obslugi kursora
        private Game mainGame;

        public void InitializeUserInterface(Game game)
        {
            mainGame = game;
            UserInterface.LoadGraphics(mainGame);
        }

        public UserInterfaceController(Camera camera, CampaignController campaignController, UserInterface userInterface)
        {
            curState = Keyboard.GetState();
            mouseCurState = Mouse.GetState();
            Camera = camera;
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

        public Camera Camera
        {
            get; set;
        }

        public void UpdateUserInterfaceState(GameTime gameTime)
        {
            UserInterface.ZuneUIModel.Animate(gameTime);
            UpdateInput(gameTime);
        }

        private void UpdateMouseState(GameTime gameTime)
        {
            mousePrevState = mouseCurState;
            mouseCurState = Mouse.GetState();
            
            //TODO: Wysuwanie Modelu Zune'a - do refaktoringu
            if(mouseCurState.LeftButton==ButtonState.Pressed&&mouseCurState!=mousePrevState)
            {
                //Do poprawki
                if (mouseCurState.X >= UserInterface.ZuneUIModel.PositionX && mouseCurState.X <= UserInterface.ZuneUIModel.PositionX+UserInterface.ZuneUIModel.ZuneUI.Width
                 && mouseCurState.Y >= UserInterface.ZuneUIModel.PositionY && mouseCurState.Y <= UserInterface.ZuneUIModel.PositionY+20)
                {
                    UserInterface.ZuneUIModel.State = UserInterface.ZuneUIModel.IsUp() == true
                                                          ? ZuneState.Down
                                                          : ZuneState.Up;
               
                }

                //Okienko informacyjne
                if (!UserInterface.InterfaceOverlaped(mouseCurState.X,mouseCurState.Y))
                {
                    if(!CampaignController.CheckSelection(mouseCurState.X, mouseCurState.Y, Camera, mainGame.Display.Projection,
                        mainGame.GraphicsDevice))
                    {
                        //GameInfo gi = new GameInfo();
                        Vector3 pos3D = CampaignController.MissionController.Mission.Board.GetPosition(mouseCurState.X, mouseCurState.Y);
                        //gi.ShowInfo(pos3D.ToString());
                    }
                }
            }
          
            
            //Drag control
            if (mouseCurState.RightButton == ButtonState.Pressed && (Math.Abs(mouseCurState.X - mousePrevState.X) > 3 || Math.Abs(mouseCurState.Y - mousePrevState.Y) > 3))
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
             //   if (isMouseDragging)
             //       Mouse.SetPosition(lastDraggingX, lastDraggingY);

                isMouseDragging = false;
            }
            
            
            
            

            if (mouseCurState.MiddleButton == ButtonState.Pressed && mouseCurState != mousePrevState)
                isMouseRotating = true;
            else 
                isMouseRotating = false;

            if(isMouseDragging)
            {
                lastDraging = gameTime.TotalGameTime.TotalMilliseconds;
                Camera.TransformCameraAccordingToMouseTravel(mouseCurState.X-mousePrevState.X,mouseCurState.Y-mousePrevState.Y);
            }
            else 
            {
                if (gameTime.TotalGameTime.TotalMilliseconds - lastDraging > 200)
                {
                    mainGame.IsMouseVisible = true;

                }
                // WYDAWANIE POLECEN JEDNOSTKOM - Tylko jesli minelo pol sekundy od puszczenia prawego    
                // przycisku - zabezpiecza nas przed przypadkowym kliknieciem przy przesuwaniu
                // Sposob scrollowania moze sie zmienic w wersji finalnej (fullscrenowej) na przesuwanie                  // po brzegach - wiec aktualnie to troche hotfix
                if (mouseCurState.RightButton == ButtonState.Released && mousePrevState.RightButton == ButtonState.Pressed  && mouseCurState != mousePrevState && (gameTime.TotalGameTime.TotalMilliseconds-lastDraging)>500)
                {
                    Vector3 pos3D = CampaignController.MissionController.Mission.Board.GetPosition(mouseCurState.X, mouseCurState.Y);
                    //Zakapsulkowac To!
                    GameObject pointedObject = CampaignController.MissionController.Mission.ObjectContainer.CheckClickedObject(mouseCurState.X, mouseCurState.Y, Camera, mainGame.Display.Projection, mainGame.GraphicsDevice);
                    if (pointedObject != null)
                    {
                        if (CampaignController.MissionController.GetSeletedObject()!=null && (pointedObject is Building))
                        {
                        //    CampaignController.MissionController.Mission.ObjectContainer.MoveToLocation(pointedObject.Position.X, pointedObject.Position.Z);
                            (CampaignController.MissionController.GetSeletedObject() as Vehicle).PointTurretToGameObject
                                (pointedObject);
                            (CampaignController.MissionController.GetSeletedObject() as Vehicle).ActivateSpecialAction();

                        }
                    }
                    else
                    {
                        CampaignController.MissionController.Mission.ObjectContainer.MoveToLocation(pos3D.X, pos3D.Z);
                    }
                }
            }

            if (isMouseRotating)
            {
                Camera.RotateCameraAccordingToMouseTravel(mouseCurState.X - mousePrevState.X, mouseCurState.Y - mousePrevState.Y);
            }


         

            //Scroll control
            Camera.ChangeHeightAccordingToMouseWheel(mouseCurState.ScrollWheelValue - mousePrevState.ScrollWheelValue);


            //Scroll na brzegach
            if (UserInterface.IsFullscreen)
            {
                if (UserInterface.GetHorizontalEdge(mouseCurState.X) == WindowPosition.LEFT)
                    Camera.MoveLeft();
                else if (UserInterface.GetHorizontalEdge(mouseCurState.X) == WindowPosition.RIGHT)
                    Camera.MoveRight();

                if (UserInterface.GetVerticalEdge(mouseCurState.Y) == WindowPosition.UP)
                    Camera.MoveForward();
                else if (UserInterface.GetVerticalEdge(mouseCurState.Y) == WindowPosition.DOWN)
                    Camera.MoveBack();
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
                Camera.MoveLeft();
            }
            
            if (curState.IsKeyDown(Keys.X))
            {
                Camera.MoveRight();
            }

            if (curState.IsKeyDown(Keys.Up))
            {
                Camera.MoveForward();
            }
            
            if (curState.IsKeyDown(Keys.Down))
            {
                Camera.MoveBack();
            }

            if (curState.IsKeyDown(Keys.Right))
            {
                Camera.RotateRight();
            }

            if (curState.IsKeyDown(Keys.Left))
            {
                Camera.RotateLeft();
            }

            if (curState.IsKeyDown(Keys.Q))
            {
                Camera.MoveUp();
            }

            if (curState.IsKeyDown(Keys.A))
            {
                Camera.MoveDown();
            }

            if (curState.IsKeyDown(Keys.E))
            {
                CampaignController.EffectController.SetEffects(true);
            }

            if (curState.IsKeyDown(Keys.R))
            {
                CampaignController.EffectController.SetEffects(false);
            }

            if (curState.IsKeyDown(Keys.Tab)&& prevState.IsKeyUp(Keys.Tab))
            {
                ((Unit)CampaignController.GetActiveObject()).Selected =  ((Unit)CampaignController.GetActiveObject()).Selected ? false : true;
            }

            
            if (curState.IsKeyDown(Keys.I))
            {
                ((Unit)CampaignController.GetActiveObject()).Position += new Vector3(0, 0, 0.2f);
            }
            if (curState.IsKeyDown(Keys.K))
            {
                ((Unit)CampaignController.GetActiveObject()).Position += new Vector3(0, 0, -0.2f);
            }
            if (curState.IsKeyDown(Keys.J))
            {
                ((Unit)CampaignController.GetActiveObject()).Position += new Vector3(0.2f, 0, 0);
            }
            if (curState.IsKeyDown(Keys.L))
            {
                ((Unit)CampaignController.GetActiveObject()).Position += new Vector3(-0.2f, 0, 0);
			}
            if (curState.IsKeyDown(Keys.U))
            {
                ((Unit)CampaignController.GetActiveObject()).Angle += new Vector3(0, 0.01f, 0);
            }
            if (curState.IsKeyDown(Keys.O))
            {
                ((Unit)CampaignController.GetActiveObject()).Angle += new Vector3(0, -0.01f, 0);
            }


            if (curState.IsKeyDown(Keys.G))
            {
                UserInterface.ZuneUIModel.State = ZuneState.Up;
            }
            if (curState.IsKeyDown(Keys.H))
            {
                UserInterface.ZuneUIModel.State = ZuneState.Down;
            }

            //TEMP  //...jak wszysztko :D
            if(curState.IsKeyDown(Keys.F1))
            {
                ((Vehicle)CampaignController.MissionController.GetSeletedObject()).turretDestination = MathHelper.PiOver2;
            }
            if(curState.IsKeyDown(Keys.F2))
            {
                ((Vehicle)CampaignController.MissionController.GetSeletedObject()).turretDestination = MathHelper.Pi;
            }
            if(curState.IsKeyDown(Keys.F3))
            {
                ((Vehicle)CampaignController.MissionController.GetSeletedObject()).turretDestination = 3 * MathHelper.PiOver2;
            }
            if(curState.IsKeyDown(Keys.F4))
            {
                ((Vehicle)CampaignController.MissionController.GetSeletedObject()).turretDestination = 2 * MathHelper.Pi;
            }
            if (curState.IsKeyDown(Keys.F5) && prevState.IsKeyUp(Keys.F5))
            {
                UserInterface.ToggleFullscreen();
            }
        }

        public void UpdateInput(GameTime gameTime)
        {
            UpdateMouseState(gameTime);
            UpdateKeyboardState();
        }
    }
}
