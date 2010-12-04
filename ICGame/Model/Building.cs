using System;
using System.Collections.Generic;
using System.Text;
using ICGame.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ICGame
{
    public class Building : GameObject, IAnimated, IPhysical, IDestructible, IInteractive 
    {
        public Building(Model model, ObjectStats.BuildingStats buildingStats)
            : base(model, buildingStats)
        {
            PhysicalTransforms = Matrix.Identity;

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
            OOBoundingBox = new OOBoundingBox(BoundingBox, scale);

            Length = scale * (BoundingBox.Max.Z - BoundingBox.Min.Z);
            Width = scale * (BoundingBox.Max.Y - BoundingBox.Min.Y);
            Height = scale * (BoundingBox.Max.X - BoundingBox.Min.X);
            PhysicalTransforms = Matrix.Identity;
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
        public Vector3 GetRandomPoint(Random random)
        {
            float x = (float)random.NextDouble() * Height / 2;
            float y = (float)random.NextDouble() * Width/2;
            float z = (float)random.NextDouble() * Length / 2;
            x = random.NextDouble() > 0.5f ? x : -x;
            z = random.NextDouble() > 0.5f ? z : -z;

            float rand = (float)random.NextDouble();

            x = rand > 0.5f ?
                (rand > 0.75f ?
                    Position.X + Height / 2 :
                    Position.X - Height / 2) :
                Position.X + x;
            z = rand > 0.5f ?
                Position.Z + z : 
                (rand > 0.25f ?
                    Position.Z + Length / 2:
                    Position.Z - Length / 2);


            return new Vector3(x, Position.Y + y, z);
        }

        #region IPhysical Members

        public BoundingBox BoundingBox
        {
            get;
            set;
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

        #region IInteractive Members

        public bool Selected { get; set; }

        #endregion

        #region IAnimated Members

        public void Animate(GameTime gameTime, GameObject[] gameObjects)
        {
            
        }

        public Matrix[] GetBasicTransforms()
        {
            Matrix[] transforms = new Matrix[Model.Bones.Count];
            Model.CopyAbsoluteBoneTransformsTo(transforms);
            return transforms;
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

        #region IInteractive Members

        public bool CheckMove(IPhysical physical, BoundingBox thisBB, GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public bool CheckMoveList(Direction directionFB, Direction directionLR, GameObject[] gameObjects, GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
