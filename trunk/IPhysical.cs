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
    }
}
