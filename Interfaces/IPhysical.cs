using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace ICGame
{
    public interface IPhysical
    {
        BoundingBox BoundingBox
        {
            get;
        }

        float Width
        {
            get;
            set;
        }

        float Height
        {
            get;
            set;
        }

        float Length
        {
            get;
            set;
        }

        Matrix PhysicalTransforms
        {
            get;
            set;
        }

        void AdjustToGround(float front, float back, float left, float right, float length, float width);

        float? CheckClicked(int x, int y, Camera camera, Matrix projection, Microsoft.Xna.Framework.Graphics.GraphicsDevice gd);

        bool CheckMove(IPhysical physical, Direction directionFB, Direction directionLR, GameTime gameTime);

        bool CheckMoveList(Direction directionFB, Direction directionLR, List<GameObject> gameObjects, GameTime gameTime);
    }
}
