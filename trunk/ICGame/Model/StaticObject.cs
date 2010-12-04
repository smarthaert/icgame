﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ICGame
{
    public class StaticObject : GameObject, IPhysical, IDestructible
    {
        public StaticObject(Model model, ObjectStats.StaticObjectStats staticObjectStats)
            : base(model, staticObjectStats)
        {
           /* if(parent!=null)
                PhysicalTransforms =parent.PhysicalTransforms;
            else*/ PhysicalTransforms = Matrix.Identity;
            OOBoundingBox = new OOBoundingBox(new Vector3(0,0,0), new Vector3(0,0,0));
            PositionChanged += OnPositionChanged;
            AngleChanged += OnAngleChanged;
        }

        void OnPositionChanged(object sender, VectorEventArgs e)
        {
            OOBoundingBox.Position = e.Vector;
        }

        void OnAngleChanged(object sender, VectorEventArgs e)
        {
            OOBoundingBox.Rotation = e.Vector;
        }

        #region IPhysical Members

        public BoundingBox BoundingBox
        {
            get; set;
        }


        public OOBoundingBox OOBoundingBox
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

        public float Width { get; set; }

        public float Height { get; set; }

        public float Length { get; set; }

		public float? CheckClicked(int x, int y, Camera camera, Matrix projection, GraphicsDevice gd)
        {
            Vector3 near = new Vector3((float)x, (float)y, 0f);
            Vector3 far = new Vector3((float)x, (float)y, 1f);

            Vector3 nearpt = gd.Viewport.Unproject(near, projection, camera.CameraMatrix, AbsoluteModelMatrix);
            Vector3 farpt = gd.Viewport.Unproject(far, projection, camera.CameraMatrix, AbsoluteModelMatrix);

            Vector3 direction = farpt - nearpt;

            direction.Normalize();

            Ray ray = new Ray(nearpt, direction);

            return BoundingBox.Intersects(ray);
        }

        #endregion
    }
}
