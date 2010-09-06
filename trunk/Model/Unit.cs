using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
        private BoundingBox boundingBox;

        protected Matrix[] BasicTransforms
        {
            get; set;
        }

        protected Matrix[] AnimationTransforms
        {
            get; set;
        }

        public Unit(Model model, float speed, float turnRadius):
            base(model)
        {
            moving = Direction.None;
            turning = Direction.None;
            Speed = speed;
            TurnRadius = turnRadius;
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
            boundingBox = new BoundingBox(min,max);     //Siedlisko śmierdzącego zła... Zatkaj nos Boo...

            Length = scale*(boundingBox.Max.Z - boundingBox.Min.Z);
            Width = scale*(boundingBox.Max.Y - boundingBox.Min.Y);
            Height = scale*(boundingBox.Max.X - boundingBox.Min.X);
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
            get
            {
                return boundingBox;
            }
            set
            {
                boundingBox = value;
            }
        }

        #endregion

        /*
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
        */

        #region IInteractive Members

        public bool Selected
        {
			get;set;
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


        #region IAnimated Members

        public virtual void Animate(GameTime gameTime)
        {
            CalculateNextStep(gameTime);
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
        protected GameTime lastGameTime;
        protected float dstAngle;

        #region IControllable Members

        public Microsoft.Xna.Framework.Vector2 Destination
        {
            get; set;
        }

        public Microsoft.Xna.Framework.Vector2 NextStep
        {
            get; set;
        }

        public virtual void CalculateNextStep(GameTime gameTime)
        {
            NextStep = new Vector2(Destination.X - Position.X, Destination.Y - Position.Z);
            NextStep = new Vector2(NextStep.X*Convert.ToSingle(Math.Cos(Angle.Y)) + NextStep.Y*Convert.ToSingle(Math.Sin(Angle.Y)),
                NextStep.Y * Convert.ToSingle(Math.Cos(Angle.Y)) + NextStep.X * Convert.ToSingle(Math.Sin(Angle.Y)));

            if(NextStep.X > 0)
            {
                Move(Direction.Forward, gameTime);
                if(NextStep.Y > 0)
                {
                    Turn(Direction.Left, gameTime);
                }
                else if(NextStep.Y < 0)
                {
                    Turn(Direction.Right, gameTime);
                }
            }
            else if(NextStep.X == 0)
            {
                if (NextStep.Y == 0)
                {
                    Move(Direction.None, gameTime);
                    Turn(Direction.None, gameTime);
                }
                else if(NextStep.Y > 0)
                {
                    Move(Direction.Backward, gameTime);
                    Turn(Direction.Right, gameTime);
                }
                else if (NextStep.Y < 0)
                {
                    Move(Direction.Backward, gameTime);
                    Turn(Direction.Left, gameTime);
                }
            }
            else
            {
                Move(Direction.Backward, gameTime);
                if (NextStep.Y > 0)
                {
                    Turn(Direction.Left, gameTime);
                }
                else if (NextStep.Y < 0)
                {
                    Turn(Direction.Right, gameTime);
                }
            }
        }

        public virtual void Move(Direction direction, GameTime gameTime)
        {
            switch(direction)
            {
                case Direction.Forward:
                    float x = Convert.ToSingle(Speed*Math.Cos(Angle.Y)*gameTime.ElapsedGameTime.Milliseconds);
                    float z = Convert.ToSingle(Speed*Math.Sin(Angle.Y)*gameTime.ElapsedGameTime.Milliseconds);
                    Position = new Vector3(Position.X + Convert.ToSingle(Speed*Math.Sin(Angle.Y)*gameTime.ElapsedGameTime.Milliseconds),
                        Position.Y, Position.Z + Convert.ToSingle(Speed*Math.Cos(Angle.Y)*gameTime.ElapsedGameTime.Milliseconds));
                    break;
                case Direction.Backward:
                    Position = new Vector3(Position.X - Convert.ToSingle(Speed * Math.Sin(Angle.Y) * gameTime.ElapsedGameTime.Milliseconds), 
                        Position.Y, Position.Z - Convert.ToSingle(Speed * Math.Cos(Angle.Y) * gameTime.ElapsedGameTime.Milliseconds));
                    break;
                case Direction.None:
                    break;
                default:
                    throw new InvalidEnumArgumentException("Ruch tylko do przodu lub do tylu");
            }
        }

        public void Turn(Direction direction, GameTime gameTime)
        {
            switch (direction)
            {
                case Direction.Left:
                    Angle = new Vector3(Angle.X, Angle.Y + Speed * gameTime.ElapsedGameTime.Milliseconds / TurnRadius, Angle.Z);
                    break;
                case Direction.Right:
                    Angle = new Vector3(Angle.X, Angle.Y - Speed * gameTime.ElapsedGameTime.Milliseconds / TurnRadius, Angle.Z);
                    break;
                case Direction.None:
                    break;
                default:
                    throw new InvalidEnumArgumentException("Skret tylko w lewo lub w prawo");
            }
        }

        #endregion

        #region IPhysical Members


        public void AdjustToGround(float front, float back, float left, float right, float length, float width)
        {
            float pitch = Convert.ToSingle(Math.Atan((front - back) / length));
            float roll = Convert.ToSingle(Math.Atan((left - right) / width));

            PhysicalTransforms = Matrix.CreateRotationX(-pitch) *Matrix.CreateRotationZ(roll);
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

        public float? CheckClicked(int x, int y, Camera camera, Matrix projection, Microsoft.Xna.Framework.Graphics.GraphicsDevice gd)
        {
            Vector3 near = new Vector3((float)x, (float)y, 0f);
            Vector3 far = new Vector3((float)x, (float)y, 1f);

            Vector3 nearpt = gd.Viewport.Unproject(near, projection, camera.CameraMatrix, ModelMatrix);
            Vector3 farpt = gd.Viewport.Unproject(far, projection, camera.CameraMatrix, ModelMatrix);

            Vector3 direction = farpt - nearpt;

            direction.Normalize();

            Ray ray = new Ray(nearpt, direction);

            return BoundingBox.Intersects(ray);
        }

        #endregion

        #region IControllable Members


        public float Speed
        {
            get; set;
        }

        public float TurnRadius
        {
            get; set;
        }

        #endregion
    }
}
