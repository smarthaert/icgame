﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ICGame.Tools;
namespace ICGame
{
    public class StaticObject : GameObject, IPhysical, IDestructible
    {
        public StaticObject(Model model)
            : base(model)
        {
           /* if(parent!=null)
                PhysicalTransforms =parent.PhysicalTransforms;
            else*/ PhysicalTransforms = Matrix.Identity;
            BoundingBox bb;
            BoundingBoxTools.CalculateBoundingBox(model, out bb);
            BoundingBox = bb;

        }
        #region IPhysical Members

        public BoundingBox BoundingBox
        {
            get; set;
        }

        #endregion

        #region IDestructible Members

        public int HP
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int Damage
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Destroy()
        {
            throw new NotImplementedException();
        }

        public void Fade()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IPhysical Members


        public void AdjustToGround(float front, float back, float left, float right, float length, float width)
        {
            
        }

        public Matrix PhysicalTransforms
        {
            get; set;
        }

        public float Width
        {
            get
            {
                return 0;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public float Height
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public float Length
        {
            get
            {
                return 0;
            }
            set
            {
                
            }
        }

		public float? CheckClicked(int x, int y, Camera camera, Matrix projection, Microsoft.Xna.Framework.Graphics.GraphicsDevice gd)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
