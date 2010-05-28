using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ICGame
{
    public class UserInterfaceController
    {
        KeyboardState curState,prevState;


        public UserInterfaceController(Camera camera, Campaign campaign)
        {
            curState = Keyboard.GetState();
            Camera = camera;
            Campaign = campaign;

           
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

        private void UpdateMouseState()
        {
            
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
        }

        public void UpdateInput()
        {
            UpdateMouseState();
            UpdateKeyboardState();
        }
    }
}
