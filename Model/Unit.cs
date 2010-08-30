using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ICGame
{
    public class Unit : GameObject, IAnimated, IPhysical, IInteractive, IDestructible, IControllable
    {
        public StaticObject SelectionRing
        {
            get; set;
        }

        protected Matrix[] BasicTransforms
        {
            get; set;
        }

        protected Matrix[] AnimationTransforms
        {
            get; set;
        }

        public Unit(Model model, float speed):
            base(model)
        {
            moving = Direction.None;
            turning = Direction.None;
            this.speed = speed;
            lastGameTime = new GameTime();
            moving = Direction.None;
            //TEMP
            moving = Direction.Forward;
            //\TEMP

            BasicTransforms = new Matrix[Model.Bones.Count];
            int j = 0;
            foreach (ModelBone bone in Model.Bones)
            {
                BasicTransforms[j++] = bone.Transform;
            }

            AnimationTransforms = new Matrix[Model.Bones.Count];
            for(int i = 0; i < AnimationTransforms.GetLength(0);++i)
            {
                AnimationTransforms[i] = Matrix.Identity;
            }
            //BoundingBox = new BoundingBox(GetMinVertex(),GetMaxVertex());
            Vector3 min = new Vector3(0,0,0);
            Vector3 max = new Vector3(0,0,0);
            for(int i = 0; i < this.Model.Meshes.Count;++i)
            {
                BoundingBox bb;
                CalculateBoundingBox(this.Model.Meshes[i],out bb);
                if(min.X > bb.Min.X)
                {
                    min.X = bb.Min.X;
                }
                if(min.Y > bb.Min.Y)
                {
                    min.Y = bb.Min.Y;
                }
                if(min.Z > bb.Min.Z)
                {
                    min.Z = bb.Min.Z;
                }
                if(max.X < bb.Max.X)
                {
                    max.X = bb.Max.X;
                }
                if(max.Y < bb.Max.Y)
                {
                    max.Y = bb.Max.Y;
                }
                if(max.Z < bb.Max.Z)
                {
                    max.Z = bb.Max.Z;
                }
            }
            BoundingBox = new BoundingBox(scale*min,scale*max);

            Length = (BoundingBox.Max.Z - BoundingBox.Min.Z);
            Width = BoundingBox.Max.Y - BoundingBox.Min.Y;
            Height = BoundingBox.Max.X - BoundingBox.Min.X;
            PhysicalTransforms = Matrix.Identity;

            
        }

        #region BoundingBox calculations

        protected Vector3[] tempVecs3 = new Vector3[512]; 
        protected ushort[] tempUshorts = new ushort[512 * 3]; 
     
        protected void CalculateBoundingBox(ModelMesh mm, out BoundingBox bb) 
        { 
          bb = new BoundingBox(); 
          bool first = true; 
          Matrix x = Matrix.Identity; 
          ModelBone mb = mm.ParentBone; 
          while (mb != null) 
          { 
            x = x * mb.Transform; 
            mb = mb.Parent; 
          } 
          foreach (ModelMeshPart mp in mm.MeshParts) 
          { 
            int n = mp.NumVertices; 
            if (n > tempVecs3.Length) 
              tempVecs3 = new Vector3[n + 128]; 
            int l = mp.PrimitiveCount * 3; 
            if (l > tempUshorts.Length) 
              tempUshorts = new ushort[l + 128]; 
            if (n == 0 || l == 0) 
              continue; 
            mm.IndexBuffer.GetData<ushort>(tempUshorts, mp.StartIndex, l); 
            mm.VertexBuffer.GetData<Vector3>(mp.StreamOffset, tempVecs3, mp.BaseVertex, n, mp.VertexStride); 
            if (first) 
            { 
              bb.Min = Vector3.Transform(tempVecs3[tempUshorts[0]], x); 
              bb.Max = bb.Min; 
              first = false; 
            } 
            for (int i = 0; i != l; ++i) 
            { 
              ushort us = tempUshorts[i]; 
              Vector3 v = Vector3.Transform(tempVecs3[us], x); 
              Vector3.Max(ref v, ref bb.Max, out bb.Max); 
              Vector3.Min(ref v, ref bb.Min, out bb.Min); 
            } 
          }
        }

        #endregion

        #region IPhysical Members

        public BoundingBox BoundingBox
        {
            get; set;
        }
 

        private Vector3 GetMinVertex()
        {
            Vector3 result = new Vector3(0,0,0);
            foreach (ModelMesh mesh in Model.Meshes)
            {
                if(mesh.BoundingSphere.Center.X - mesh.BoundingSphere.Radius < result.X)
                {
                    result.X = mesh.BoundingSphere.Center.X - mesh.BoundingSphere.Radius;
                }
                if (mesh.BoundingSphere.Center.Y - mesh.BoundingSphere.Radius < result.Y)
                {
                    result.Y = mesh.BoundingSphere.Center.Y - mesh.BoundingSphere.Radius;
                }
                if (mesh.BoundingSphere.Center.Z - mesh.BoundingSphere.Radius < result.Z)
                {
                    result.Z = mesh.BoundingSphere.Center.Z - mesh.BoundingSphere.Radius;
                }
                
            }
            return result;
        }

        private Vector3 GetMaxVertex()
        {
            Vector3 result = new Vector3(0, 0, 0);
            foreach (ModelMesh mesh in Model.Meshes)
            {
                if (mesh.BoundingSphere.Center.X - mesh.BoundingSphere.Radius > result.X)
                {
                    result.X = mesh.BoundingSphere.Center.X - mesh.BoundingSphere.Radius;
                }
                if (mesh.BoundingSphere.Center.Y - mesh.BoundingSphere.Radius > result.Y)
                {
                    result.Y = mesh.BoundingSphere.Center.Y - mesh.BoundingSphere.Radius;
                }
                if (mesh.BoundingSphere.Center.Z - mesh.BoundingSphere.Radius > result.Z)
                {
                    result.Z = mesh.BoundingSphere.Center.Z - mesh.BoundingSphere.Radius;
                }
            }
            return result;
        }

        #endregion

        #region IInteractive Members

        public bool Selected
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

        #region IControllable Members

        public Microsoft.Xna.Framework.Vector2 Destination
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

        public Microsoft.Xna.Framework.Vector2 NextStep
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

        public virtual void Animate(GameTime gameTime)
        {
            for(int i = 0; i < Model.Bones.Count; ++i)
            {
                Model.Bones[i].Transform = AnimationTransforms[i]*BasicTransforms[i];
            }
        }

        #endregion

        public Direction Moving
        {
            get { return moving; }
            set { moving = value; }
        }

        protected Direction moving;
        protected Direction turning;
        protected float speed;
        protected GameTime lastGameTime;
        protected float dstAngle;

        #region IControllable Members


        public void CalculateNextStep()
        {
            throw new NotImplementedException();
        }

        public void Move(Direction direction)
        {
            throw new NotImplementedException();
        }

        public void Turn(Direction direction)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IPhysical Members


        public void AdjustToGround(float front, float back, float left, float right, float length, float width)
        {
            /*Vector3 u = new Vector3(-1, 0, topLeft - topRight);
            Vector3 v = new Vector3(0, 1, downLeft - topRight);

            Vector3 normal = Vector3.Cross(v, u);

            float pitch = Convert.ToSingle(Math.Atan2(normal.X, Math.Sqrt(normal.Z * normal.Z + normal.Y * normal.Y)));
            float roll = Convert.ToSingle(Math.Atan2(-normal.Y, Math.Sqrt(normal.X * normal.X + normal.Z * normal.Z)));

            this.PhysicalTransforms = Matrix.CreateRotationX(pitch) *Matrix.CreateRotationY(roll);*/

            float pitch = Convert.ToSingle(Math.Atan((front - back) / length));
            float roll = Convert.ToSingle(Math.Atan((left - right) / width));

            this.PhysicalTransforms = Matrix.CreateRotationX(-pitch) *Matrix.CreateRotationZ(roll);
        }

        public Matrix PhysicalTransforms
        {
            get;
            set;
        }

        public float Width
        {
            get;
            set;
        }

        public float Height
        {
            get;
            set;
        }

        public float Length
        {

            get;
            set;
        }

        #endregion
    }
}
