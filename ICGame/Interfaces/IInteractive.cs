using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace ICGame
{
    public interface IInteractive
    {
        bool Selected
        {
            get;
            set;
        }

        bool CheckMove(IPhysical physical, BoundingBox thisBB, GameTime gameTime);

        bool CheckMoveList(Direction directionFB, Direction directionLR, GameObject[] gameObjects, GameTime gameTime);
    }
}
