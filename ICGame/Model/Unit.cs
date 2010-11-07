﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using ICGame.Tools;
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
            //moving = Direction.Forward;
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
            Vector3 min = new Vector3(float.MaxValue,float.MaxValue,float.MaxValue);
            Vector3 max = new Vector3(float.MinValue,float.MinValue,float.MinValue);
            for(int i = 0; i < this.Model.Meshes.Count;++i)
            {
                BoundingBox bb;
                BoundingBoxTools.CalculateBoundingBox(Model.Meshes[i],out bb);
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

        public Mission Mission       //jak jest lepszy pomysł na przekazanie Board'a to jestem otwarty na propozycje
        {
            get;
            set;
        }

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

        public virtual void Animate(GameTime gameTime, List<GameObject> gameObjects)
        {
            CalculateNextStep(gameTime, gameObjects);
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

        public virtual void CalculateNextStep(GameTime gameTime, List<GameObject> gameObjects)
        {
            /*NextStep = new Vector2(Destination.X - Position.X, Destination.Y - Position.Z);
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
            }*/
        }

        public virtual void Move(Direction direction, GameTime gameTime)
        {
            switch(direction)
            {
                case Direction.Forward:
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

        #region IInteractive Members


        public bool CheckMove(IPhysical physical, BoundingBox thisBB, GameTime gameTime)
        {
            GameObject go = physical as GameObject;
            BoundingBox checkBB = BoundingBoxTools.TransformBoundingBox(physical.BoundingBox, go.ModelMatrix);

            return !thisBB.Intersects(checkBB);
        }

        public bool CheckMoveList(Direction directionFB, Direction directionLR, List<GameObject> gameObjects, GameTime gameTime)
        {
            float spd = 0;
            if (directionFB == Direction.Forward)
            {
                spd = Speed;
            }
            else
            {
                spd = -Speed;
            }
            Vector3 oldpos = new Vector3(Position.X, Position.Y, Position.Z);
            Vector3 oldangle = new Vector3(Angle.X, Angle.Y, Angle.Z);
            Position = new Vector3(Position.X + Convert.ToSingle(spd * Math.Sin(Angle.Y) * gameTime.ElapsedGameTime.Milliseconds),
                Position.Y, Position.Z + Convert.ToSingle(spd * Math.Cos(Angle.Y) * gameTime.ElapsedGameTime.Milliseconds));
            if (directionLR == Direction.Left)
            {
                Angle = new Vector3(Angle.X, Angle.Y + Speed * gameTime.ElapsedGameTime.Milliseconds / TurnRadius, Angle.Z);
            }
            else if (directionLR == Direction.Right)
            {
                Angle = new Vector3(Angle.X, Angle.Y - Speed * gameTime.ElapsedGameTime.Milliseconds / TurnRadius, Angle.Z);
            } 
            float sinl = Convert.ToSingle(Math.Sin(Angle.Y) * Length / 2);
            float cosl = Convert.ToSingle(Math.Cos(Angle.Y) * Length / 2);
            float sinw = Convert.ToSingle(Math.Sin(Angle.Y) * Width / 2);
            float cosw = Convert.ToSingle(Math.Cos(Angle.Y) * Width / 2);
            
            AdjustToGround(
                        Mission.Board.GetHeight(
                            Position.X + sinl,
                            Position.Z + cosl
                            ),
                        Mission.Board.GetHeight(
                            Position.X - sinl,
                            Position.Z - cosl
                            ),
                        Mission.Board.GetHeight(
                            Position.X + cosw,
                            Position.Z - sinw
                            ),
                        Mission.Board.GetHeight(
                            Position.X - cosw,
                            Position.Z + sinw
                            ),
                        Length,
                        Width);
            BoundingBox thisBB = BoundingBoxTools.TransformBoundingBox(BoundingBox, ModelMatrix);
            foreach (GameObject gameObject in gameObjects)
            {
                if(gameObject is IPhysical && gameObject != this)
                {
                    IPhysical physical = gameObject as IPhysical;
                    if(!CheckMove(physical, thisBB, gameTime))
                    {
                        Position = oldpos;
                        Angle = oldangle;
                        return false;
                    }
                }
            }
            Position = oldpos;
            Angle = oldangle;
            return true;
        }

        #endregion
    }

}