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
                return Matrix.CreateScale(0.01f, 0.01f, 0.01f) * Matrix.CreateRotationY(3*MathHelper.PiOver2) /** Matrix.CreateRotationX(MathHelper.PiOver2) */* Matrix.CreateTranslation(Position);
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
            get; set;
        }

        #region IDrawable Members


        public Vector3 Diffuse
        {
            get; set;
        }

        public Vector3 Ambient
        {
            get; set;
        }

        public Vector3 Specular
        {
            get; set;
        }

        public Vector3 Shininess
        {
            get; set;
        }

        public float SpecularFactor
        {
            get; set;
        }

        #endregion
    }
}
