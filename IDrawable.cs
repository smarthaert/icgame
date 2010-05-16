using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ICGame
{
    public interface IDrawable
    {
        Matrix ModelMatrix
        {
            get;
            set;
        }

        Model Model
        {
            get;
            set;
        }

        GameObjectDrawer GetDrawer();
    }
}
