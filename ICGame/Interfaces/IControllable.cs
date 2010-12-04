using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Point = System.Drawing.Point;

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

        List<Point> Path
        {
            get; set;
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

        DateTime PathTime
        {
            get; set;
        }

        void CalculateNextStep(GameTime gameTime, GameObject[] gameObjects);

        void Move(Direction direction, GameTime gameTime);

        void Turn(Direction direction, GameTime gameTime);
    }
}
