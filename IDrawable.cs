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
        
        }

        Model Model
        {
            get; set;
        }

        List<Texture2D> Textures
        {
            get;
            set;
        }
    
        GameObjectDrawer GetDrawer();

    }
}
