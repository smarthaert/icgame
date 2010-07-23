using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ICGame
{
    public enum Direction
    {
        Forward,Backward,Left,Right,None
    }
    
    public abstract class GameObject : IDrawable
    {
        protected const float scale = 0.01f;

        private Vector3 position;
        
        #region IDrawable Members

        public GameObject(Model model)
        {
            Model = model;
            Position = new Vector3(0, 0, 0);
            
        }
        public virtual GameObjectDrawer GetDrawer()
        {
            return new GameObjectDrawer(this);
        }


        public void Update()
        {
            
        }


        public Matrix ModelMatrix
        {
            get
            {
                Matrix result = Matrix.Identity;
                result *= Matrix.CreateScale(scale,scale,scale) /** Matrix.CreateRotationY(3*MathHelper.PiOver2)*/ * Matrix.CreateRotationZ(-MathHelper.PiOver2);// *result;
                if (this is IPhysical)
                {
                    result *= ((IPhysical)this).PhysicalTransforms;// *result;
                }
                result *= Matrix.CreateTranslation(Position);
                return result;
            }
            

        }

        public Model Model
        {
            get; set;
        }

        public List<Texture2D> Textures
        { 
            get; set;
        }

        #endregion

        public Vector3 Position
        {
            get
            {
                return position;
            } 
            set
            {
                position = new Vector3(value.X,value.Y,value.Z);
            }
        }

        public Vector3 Angle
        {
            get;
            set;
        }

        #region IDrawable Members

        public List<float> Opacity
        {
            get; set;
        }

        public List<float> Transparency
        {
            get; set;
        }


        public List<Vector3> DiffuseColor
        {
            get;
            set;
        }
        
        public List<float> DiffuseFactor
        {
            get;
            set;
        }

        public List<Vector3> Ambient
        {
            get;
            set;
        }

        public List<Vector3> Specular
        {
            get;
            set;
        }

        public List<Vector3> Shininess
        {
            get;
            set;
        }

        public List<float> SpecularFactor
        {
            get;
            set;
        }

        #endregion

     
    }
}
