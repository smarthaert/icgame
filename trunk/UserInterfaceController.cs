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
                Camera.Move(-0.2f,0);
            }
            
            if (curState.IsKeyDown(Keys.X))
            {
                Camera.Move(0.2f, 0);
            }

            if (curState.IsKeyDown(Keys.Up))
            {
                Camera.Move(0, 0.2f);
            }
            
            if (curState.IsKeyDown(Keys.Down))
            {
                Camera.Move(0, -0.2f);
            }

            if (curState.IsKeyDown(Keys.Right))
            {
                Camera.Rotate(0.01f);
            }

            if (curState.IsKeyDown(Keys.Left))
            {
                Camera.Rotate(-0.01f);
            }

            if (curState.IsKeyDown(Keys.Q))
            {
                Camera.ChangeHeight(0.1f);
            }

            if (curState.IsKeyDown(Keys.A))
            {
                Camera.ChangeHeight(-0.1f);
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
            //TEMP
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
