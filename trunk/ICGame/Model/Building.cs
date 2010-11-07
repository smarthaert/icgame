﻿using System;
using System.Collections.Generic;
using System.Text;
using ICGame.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ICGame
{
    public class Building : GameObject, IAnimated, IPhysical, IDestructible, IInteractive 
    {
        public Building(Model model)
            : base(model)
        {
            //Position = new Vector3(10,0,16);
            PhysicalTransforms = Matrix.Identity;// +Matrix.CreateTranslation(Position);

            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
            for (int i = 0; i < Model.Meshes.Count; ++i)
            {
                BoundingBox bb;
                BoundingBoxTools.CalculateBoundingBox(Model.Meshes[i], out bb);
                if (min.X > bb.Min.X)
                {
                    min.X = bb.Min.X;
                }
                if (min.Y > bb.Min.Y)
                {
                    min.Y = bb.Min.Y;
                }
                if (min.Z > bb.Min.Z)
                {
                    min.Z = bb.Min.Z;
                }
                if (max.X < bb.Max.X)
                {
                    max.X = bb.Max.X;
                }
                if (max.Y < bb.Max.Y)
                {
                    max.Y = bb.Max.Y;
                }
                if (max.Z < bb.Max.Z)
                {
                    max.Z = bb.Max.Z;
                }
            }
            BoundingBox = new BoundingBox(min, max);

            Length = scale * (BoundingBox.Max.Z - BoundingBox.Min.Z);
            Width = scale * (BoundingBox.Max.Y - BoundingBox.Min.Y);
            Height = scale * (BoundingBox.Max.X - BoundingBox.Min.X);
            PhysicalTransforms = Matrix.Identity;
        }

        #region IPhysical Members

        public BoundingBox BoundingBox
        {
            get;
            set;
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

        #region IInteractive Members

        public bool Selected
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

        #endregion

        #region IAnimated Members

        public void Animate(GameTime gameTime, List<GameObject> gameObjects)
        {
            
        }

        #endregion

        #region IPhysical Members


        public void AdjustToGround(float front, float back, float left, float right, float length, float width)
        {
            return;
        }

        public Matrix PhysicalTransforms
        {
            get; set;
        }

        public float Width
        {
            get; set;
        }

        public float Height
        {
            get; set;
        }

        public float Length
        {
            get; set;
        }

        public float? CheckClicked(int x, int y, Camera camera, Matrix projection, Microsoft.Xna.Framework.Graphics.GraphicsDevice gd)
        {
            return null;
        }

        #endregion

        #region IInteractive Members

        public bool CheckMove(IPhysical physical, BoundingBox thisBB, GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public bool CheckMoveList(Direction directionFB, Direction directionLR, List<GameObject> gameObjects, GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}