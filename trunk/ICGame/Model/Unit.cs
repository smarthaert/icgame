using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using ICGame.ObjectStats;
using ICGame.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Point = System.Drawing.Point;

namespace ICGame
{
    public class Unit : GameObject, IAnimated, IPhysical, IInteractive, IDestructible, IControllable
    {
        protected StaticObject selectionRing;
        private BoundingBox boundingBox;

        protected Matrix[] BasicTransforms
        {
            get; set;
        }

        protected Matrix[] AnimationTransforms
        {
            get; set;
        }

        public DateTime PathTime
        {
            get;
            set;
        }

        public Effect[] MiniModelEffects
        {
            get; set;
        }

        public Unit(Model model, UnitStats unitStats):
            base(model, unitStats)
        {
            moving = Direction.None;
            turning = Direction.None;
            Speed = unitStats.Speed;
            TurnRadius = unitStats.TurnRadius;

            if (children == null)
            {
                children = new List<GameObject>();
            }

            for (int i = 0; i < unitStats.SubElements.Count; ++i )
            {
                if (unitStats.SubElements[i].Name == "selection_ring")
                {
                    selectionRing = (StaticObject)children[i];
                    selectionRing.Visible = false;
                    
                    break;
                }
            }

            Path = new List<Point>();
            lastGameTime = new GameTime();
            moving = Direction.None;

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

            BoundingBoxTools.CalculateBoundingBox(Model, out boundingBox);
            OOBoundingBox = new OOBoundingBox(boundingBox, scale);
            Length = scale*(boundingBox.Max.Z - boundingBox.Min.Z);
            Width = scale*(boundingBox.Max.Y - boundingBox.Min.Y);
            Height = scale*(boundingBox.Max.X - boundingBox.Min.X);
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

        public OOBoundingBox OOBoundingBox
        {
            get; set;
        }

        #endregion

        #region IInteractive Members

        private bool selected;

        public bool Selected
        {
			get
			{
			    return selected;
			}
            set
			{
			    selectionRing.Visible = value;
			    selected = value;
			}
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

        public virtual void Animate(GameTime gameTime, IEnumerable<GameObject> gameObjects)
        {
            CalculateNextStep(gameTime, gameObjects);
            for(int i = 0; i < Model.Bones.Count; ++i)
            {
                Model.Bones[i].Transform = AnimationTransforms[i]*BasicTransforms[i];
            }
        }

        public Matrix[] GetBasicTransforms()
        {
            Matrix[] transforms = new Matrix[Model.Bones.Count];
            for (int i = 0; i < Model.Bones.Count; ++i)
            {
                Model.Bones[i].Transform = BasicTransforms[i];
            }
            Model.CopyAbsoluteBoneTransformsTo(transforms);
            for (int i = 0; i < Model.Bones.Count; ++i)
            {
                Model.Bones[i].Transform = AnimationTransforms[i] * BasicTransforms[i];
            }
            return transforms;
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

        public Vector2 Destination
        {
            get; set;
        }

        public Vector2 NextStep
        {
            get; set;
        }

        public virtual void CalculateNextStep(GameTime gameTime, IEnumerable<GameObject> gameObjects)
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

        public float? CheckClicked(int x, int y, GraphicsDevice gd)
        {
            Vector3 near = new Vector3((float)x, (float)y, 0f);
            Vector3 far = new Vector3((float)x, (float)y, 1f);

            Vector3 nearpt = gd.Viewport.Unproject(near, DisplayController.Projection, DisplayController.Camera.CameraMatrix, AbsoluteModelMatrix);
            Vector3 farpt = gd.Viewport.Unproject(far, DisplayController.Projection, DisplayController.Camera.CameraMatrix, AbsoluteModelMatrix);

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

        public List<Point> Path
        {
            get; set;
        }

        #endregion

        #region IInteractive Members

        /// <summary>
        /// Deprecated
        /// </summary>
        /// <param name="physical"></param>
        /// <param name="thisBB"></param>
        /// <param name="gameTime"></param>
        /// <returns></returns>
        public bool CheckMove(IPhysical physical, BoundingBox thisBB, GameTime gameTime)
        {
            GameObject go = physical as GameObject;
            BoundingBox checkBB = BoundingBoxTools.TransformBoundingBox(physical.BoundingBox, go.AbsoluteModelMatrix);
            
            return !thisBB.Intersects(checkBB);
        }

        public bool CheckMove(IPhysical physical)
        {
            return OOBoundingBox.Intersects(physical.OOBoundingBox);
        }

        public bool CheckMoveList(Direction directionFB, Direction directionLR, IEnumerable<GameObject> gameObjects, GameTime gameTime)
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
            Position = new Vector3(Position.X, Mission.Board.GetHeight(Position.X, Position.Z), Position.Z);
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

            foreach (GameObject gameObject in gameObjects)
            {
                if(gameObject is IPhysical && gameObject != this)
                {
                    IPhysical physical = gameObject as IPhysical;
                    if(!CheckMove(physical))
                    {
                        Position = oldpos;
                        Angle = oldangle;

                        Path.Insert(0,new Point(Convert.ToInt32(NextStep.X),Convert.ToInt32(NextStep.Y)));

                        NextStep = CalculateBackingUp(Angle.Y, new Vector2(Position.X, Position.Z), 25.0, directionFB);
                        return false;
                    }
                }
            }
            Position = oldpos;
            Angle = oldangle;
            return true;
        }

        #endregion

        #region IInteractive submethods

        protected Vector2 CalculateBackingUp(double Angle, Vector2 Position, double distance, Direction fbDirection)
        {
            //Angle = (Angle / 180.0) * Math.PI;
            if (Angle % (2 * Math.PI) == 0)
            {
                return new Vector2(Position.X, Convert.ToSingle(Position.Y - distance));
            }
            if (Angle % Math.PI == 0)
            {
                return new Vector2(Position.X, Convert.ToSingle(Position.Y + distance));
            }
            Angle *= -1;
            Angle -= Math.PI / 2.0;
            double a = Math.Tan(Angle);
            //double b = Position.Z - a*Position.X;
            double sign = Math.Sign(Math.Cos(Angle)) != 0 ? Math.Sign(Math.Cos(Angle)) : 1;
            if(fbDirection == Direction.Backward)
            {
                sign *= -1;
            }
            double x2 = Position.X + sign * distance / Math.Sqrt(a * a + 1);      //creepy
            double y2 = a * (x2 - Position.X) + Position.Y;

            return new Vector2(Convert.ToSingle(x2), Convert.ToSingle(y2));

        }

        #endregion
    }
}
