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

        Vector3 Diffuse
        {
            get;
            set;
        }

        Microsoft.Xna.Framework.Vector3 Ambient
        {
            get;
            set;
        }

        Microsoft.Xna.Framework.Vector3 Specular
        {
            get;
            set;
        }

        Microsoft.Xna.Framework.Vector3 Shininess
        {
            get;
            set;
        }

        float SpecularFactor
        {
            get;
            set;
        }
    
        GameObjectDrawer GetDrawer();

    }
}
