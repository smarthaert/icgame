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
            set;
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
    }
}
