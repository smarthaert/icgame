﻿using System;
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
                return Matrix.CreateScale(0.01f, 0.01f, 0.01f) /** Matrix.CreateRotationY(3*MathHelper.PiOver2)*/ * Matrix.CreateRotationZ(-MathHelper.PiOver2) * Matrix.CreateTranslation(Position);
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