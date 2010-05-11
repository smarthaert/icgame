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


        public UserInterfaceController()
        {
            curState = Keyboard.GetState();
        }
    
        public Camera Camera
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
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
                Mediator.Game.GameState = GameState.Exit;
            }

        }

        public void UpdateInput()
        {
            UpdateMouseState();
            UpdateKeyboardState();
        }
    }
}
