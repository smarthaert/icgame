using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace ICGame
{
    public interface IControllable
    {
        Microsoft.Xna.Framework.Vector2 Destination
        {
            get;
            set;
        }

        Microsoft.Xna.Framework.Vector2 NextStep
        {
            get;
            set;
        }

        float Speed
        {
            get;
            set;
        }

        float TurnRadius
        {
            get;
            set;
        }

        void CalculateNextStep(GameTime gameTime);

        void Move(Direction direction, GameTime gameTime);

        void Turn(Direction direction, GameTime gameTime);
    }
}
