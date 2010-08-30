using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ICGame
{
    public class UserInterfaceController
    {
        private KeyboardState curState,prevState;
        private MouseState mouseCurState,mousePrevState;

        private bool isMouseDragging = false;
        private bool isMouseRotating = false;

        public UserInterfaceController(Camera camera, Campaign campaign, UserInterface userInterface)
        {
            curState = Keyboard.GetState();
            mouseCurState = Mouse.GetState();
            Camera = camera;
            Campaign = campaign;
            UserInterface = userInterface;
        }

        public UserInterface UserInterface
        {
            get; set;
        }

        public Campaign Campaign
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
        }

        private void UpdateMouseState()
        {
            mousePrevState = mouseCurState;
            mouseCurState = Mouse.GetState();
            if(mouseCurState.LeftButton==ButtonState.Pressed&&mouseCurState!=mousePrevState)
            {
                if (mouseCurState.X >= UserInterface.ZuneUIModel.PositionX && mouseCurState.X <= UserInterface.ZuneUIModel.PositionX+UserInterface.ZuneUIModel.ZuneUI.Width
                 && mouseCurState.Y >= UserInterface.ZuneUIModel.PositionY && mouseCurState.Y <= UserInterface.ZuneUIModel.PositionY+20)
                {
                    UserInterface.ZuneUIModel.State = UserInterface.ZuneUIModel.IsUp() == true
                                                          ? ZuneState.Down
                                                          : ZuneState.Up;
                }
            }

            
            //Drag control
            if (mouseCurState.RightButton == ButtonState.Pressed && mouseCurState != mousePrevState)
                isMouseDragging = true;
            else
                isMouseDragging = false;

            if (mouseCurState.MiddleButton == ButtonState.Pressed && mouseCurState != mousePrevState)
                isMouseRotating = true;
            else
                isMouseRotating = false;

            if(isMouseDragging)
            {
                Camera.TransformCameraAccordingToMouseTravel(mouseCurState.X-mousePrevState.X,mouseCurState.Y-mousePrevState.Y);
            }

            if (isMouseRotating)
            {
                Camera.RotateCameraAccordingToMouseTravel(mouseCurState.X - mousePrevState.X, mouseCurState.Y - mousePrevState.Y);
            }

            //Scroll control
            Camera.ChangeHeightAccordingToMouseWheel(mouseCurState.ScrollWheelValue - mousePrevState.ScrollWheelValue);

            if(UserInterface.InterfaceOverlaped(mouseCurState.X,mouseCurState.Y))
            {
                
            }
        }

        private void UpdateKeyboardState()
        {
            prevState = curState;
            curState = Keyboard.GetState();

            if (curState.IsKeyDown(Keys.Escape) && prevState.IsKeyUp(Keys.Escape))
            {
                Campaign.GameState = GameState.Exit;
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
            if (curState.IsKeyDown(Keys.Tab)&& prevState.IsKeyUp(Keys.Tab))
            {
                Campaign.UnitContainer.Units[0].Selected = Campaign.UnitContainer.Units[0].Selected ? false : true;
            }

            
            if (curState.IsKeyDown(Keys.I))
            {
                Campaign.UnitContainer.Units[0].Position+=new Vector3(0,0,0.2f);
            }
            if (curState.IsKeyDown(Keys.K))
            {
                Campaign.UnitContainer.Units[0].Position += new Vector3(0, 0, -0.2f);
            }
            if (curState.IsKeyDown(Keys.J))
            {
                Campaign.UnitContainer.Units[0].Position += new Vector3(0.2f, 0, 0);
            }
            if (curState.IsKeyDown(Keys.L))
            {
                Campaign.UnitContainer.Units[0].Position += new Vector3(-0.2f, 0, 0);
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
                ((Vehicle)Campaign.Mission.ObjectContainer.GameObjects[0]).turretDestination = MathHelper.PiOver2;
            }
            if(curState.IsKeyDown(Keys.F2))
            {
                ((Vehicle)Campaign.Mission.ObjectContainer.GameObjects[0]).turretDestination = MathHelper.Pi;
            }
            if(curState.IsKeyDown(Keys.F3))
            {
                ((Vehicle)Campaign.Mission.ObjectContainer.GameObjects[0]).turretDestination = 3*MathHelper.PiOver2;
            }
            if(curState.IsKeyDown(Keys.F4))
            {
                ((Vehicle)Campaign.Mission.ObjectContainer.GameObjects[0]).turretDestination = 2*MathHelper.Pi;
            }
            //\TEMP
        }

        public void UpdateInput()
        {
            UpdateMouseState();
            UpdateKeyboardState();
        }
    }
}
