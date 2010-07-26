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

        List<Vector3> DiffuseColor
        {
            get;
            set;
        }
        
        List<float> DiffuseFactor
        {
            get;
            set;
        }

        List<float> Ambient
        {
            get;
            set;
        }

        List<Vector3> Specular
        {
            get;
            set;
        }

        List<Vector3> Shininess
        {
            get;
            set;
        }

        List<float> SpecularFactor
        {
            get;
            set;
        }
    
        GameObjectDrawer GetDrawer();

    }
}
