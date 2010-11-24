using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Point = System.Drawing.Point;

namespace ICGame
{
    public enum DoorState
    {
        Closed,Closing,Open,Opening
    }
    public enum WheelState
    {
        Straight,StraightLeft,Left,LeftStraight,StraightRight,Right,RightStraight
    }

    public class Vehicle : Unit
    {
        private readonly int rearWheelsCount;
        private readonly int frontWheelsCount;
        private readonly int doorCount;
        private readonly bool hasTurret;
        private DoorState leftDoorState;
        private DoorState rightDoorState;
        private WheelState wheelState;
        private float leftDoorAngle;
        private float rightDoorAngle;
        private float wheelAngle;
        private float turretAngle = 0;
        public float turretDestination;     //TEMP!
        private GameInfo gi;    //TEMP

        #region Properties

        public DoorState LeftDoorState
        {
            get
            {
                return leftDoorState;
            }
            set
            {
                if((leftDoorState == DoorState.Open && value == DoorState.Closed)||
                    (leftDoorState == DoorState.Closed && value == DoorState.Open))
                {
                    throw new InvalidEnumArgumentException("Can't change it directly");
                }
                leftDoorState = value;
            }
        }

        public DoorState RightDoorState
        {
            get
            {
                return rightDoorState;
            }
            set
            {
                if ((rightDoorState == DoorState.Open && value == DoorState.Closed) ||
                    (rightDoorState == DoorState.Closed && value == DoorState.Open))
                {
                    throw new InvalidEnumArgumentException("Can't change it directly");
                }
                rightDoorState = value;
            }
        }

        public WheelState WheelState
        {
            get
            {
                return wheelState;
            }
            set
            {
                if((wheelState == WheelState.Straight && (value == WheelState.StraightLeft || value == WheelState.StraightRight))||
                    (wheelState == WheelState.Left && value == WheelState.LeftStraight) ||
                    (wheelState == WheelState.Right && value == WheelState.RightStraight) ||
                    (wheelState == WheelState.RightStraight && value == WheelState.StraightRight) ||
                    (wheelState == WheelState.StraightRight && value == WheelState.RightStraight) ||
                    (wheelState == WheelState.LeftStraight && value == WheelState.StraightLeft) ||
                    (wheelState == WheelState.StraightLeft && value == WheelState.LeftStraight))
                {
                    wheelState = value;
                }
                else
                {
                    throw new InvalidEnumArgumentException("Can't change it directly");
                }
                
            }
        }

        public float TurretAngle
        {
            get { return turretAngle; }
        }

        #endregion

        public Vehicle(Model model, float speed, float turnRadius, int rearWheelsCount, int frontWheelsCount, int doorCount, bool hasTurret)
            : base(model, speed, turnRadius)
        {
            this.rearWheelsCount = rearWheelsCount;
            this.frontWheelsCount = frontWheelsCount;
            this.doorCount = doorCount;
            this.hasTurret = hasTurret;

            leftDoorAngle = 0;
            rightDoorAngle = 0;
            leftDoorState = DoorState.Closed;
            rightDoorState = DoorState.Closed;

            wheelAngle = 0;
            wheelState = WheelState.Straight;

            NextStep = new Vector2(Position.X,Position.Z);

            //TEMP
            //leftDoorState = DoorState.Opening;
            //rightDoorState = DoorState.Opening;
            //wheelState = WheelState.StraightLeft;
            //gi = new GameInfo();




        }

        private void AnimateTurret(GameTime gameTime)
        {
            float curAngle = Angle.Z + turretAngle;
            const float turningSpeed = 0.003f;

            float rot = turretDestination - curAngle;
            if (rot > MathHelper.Pi)
            {
                rot -= 2 * MathHelper.Pi;
            }
            else if (rot < -MathHelper.Pi)
            {
                rot += 2 * MathHelper.Pi;
            }
            if (Math.Abs(rot) >= turningSpeed * gameTime.ElapsedGameTime.Milliseconds)
            {
                turretAngle += turningSpeed * gameTime.ElapsedGameTime.Milliseconds * Math.Sign(rot);
            }
            else
            {
                turretAngle += rot;
            }
            AnimationTransforms[Model.Bones["WaterCannonBase"].Index] = Matrix.CreateRotationZ(turretAngle);
            AnimationTransforms[Model.Bones["WaterPipe0"].Index] = Matrix.CreateRotationZ(turretAngle);
            AnimationTransforms[Model.Bones["WaterPipe1"].Index] = Matrix.CreateRotationZ(turretAngle);
            AnimationTransforms[Model.Bones["WaterSource0"].Index] = Matrix.CreateTranslation(0, -2.4f, 0.3f) * Matrix.CreateRotationZ(turretAngle) ;
            AnimationTransforms[Model.Bones["WaterSource1"].Index] =  Matrix.CreateTranslation(0, 2.4f, 0.3f)*Matrix.CreateRotationZ(turretAngle);
       
            
            
        }

        private void AnimateTurn(GameTime gameTime)
        {
            float rot = 0;
            const float turningSpeed = 0.003f;
            const float maxAngle = 0.5f*MathHelper.PiOver4;
            switch(wheelState)
            {
                case WheelState.StraightRight:
                    rot = turningSpeed * gameTime.ElapsedGameTime.Milliseconds;
                    wheelAngle += rot;
                    if (wheelAngle >= maxAngle)
                    {
                        wheelState = WheelState.Right;
                        //TEMP
                        //WheelState = WheelState.RightStraight;
                        //\TEMP
                        for (int i = 0; i < frontWheelsCount / 2; i++)
                        {
                            AnimationTransforms[Model.Bones["FrontWheel" + Convert.ToInt32(2*i)].Index] =
                                AnimationTransforms[Model.Bones["FrontWheel" + Convert.ToInt32(2*i)].Index] *
                                Matrix.CreateRotationZ(-maxAngle + wheelAngle - rot);
                            AnimationTransforms[Model.Bones["FrontWheel" + Convert.ToInt32(2*i + 1)].Index] =
                                AnimationTransforms[Model.Bones["FrontWheel" + Convert.ToInt32(2*i + 1)].Index] *
                                Matrix.CreateRotationZ(maxAngle - wheelAngle + rot);
                        }
                        wheelAngle = maxAngle;
                    }
                    else
                    {

                        for (int i = 0; i < frontWheelsCount / 2; i++)
                        {
                            AnimationTransforms[Model.Bones["FrontWheel" + Convert.ToInt32(2*i)].Index] =
                                AnimationTransforms[Model.Bones["FrontWheel" + Convert.ToInt32(2*i)].Index]*
                                Matrix.CreateRotationZ(-rot);
                            AnimationTransforms[Model.Bones["FrontWheel" + Convert.ToInt32(2*i + 1)].Index] =
                                AnimationTransforms[Model.Bones["FrontWheel" + Convert.ToInt32(2*i + 1)].Index]*
                                Matrix.CreateRotationZ(rot);
                        }
                    }
                    break;
                case WheelState.RightStraight:
                    rot = -turningSpeed * gameTime.ElapsedGameTime.Milliseconds;
                    wheelAngle += rot;
                    if (wheelAngle <= 0)
                    {
                        wheelState = WheelState.Straight;
                        //TEMP
                        //WheelState = WheelState.StraightLeft;
                        //\TEMP
                        for (int i = 0; i < frontWheelsCount / 2; i++)
                        {
                            AnimationTransforms[Model.Bones["FrontWheel" + Convert.ToInt32(2*i)].Index] =
                                AnimationTransforms[Model.Bones["FrontWheel" + Convert.ToInt32(2*i)].Index]*
                                Matrix.CreateRotationZ(wheelAngle - rot);
                            AnimationTransforms[Model.Bones["FrontWheel" + Convert.ToInt32(2*i + 1)].Index] =
                                AnimationTransforms[Model.Bones["FrontWheel" + Convert.ToInt32(2*i + 1)].Index]*
                                Matrix.CreateRotationZ(-wheelAngle + rot);
                        }
                        wheelAngle = 0;
                    }
                    else
                    {
                        for (int i = 0; i < frontWheelsCount / 2; i++)
                        {
                            AnimationTransforms[Model.Bones["FrontWheel" + Convert.ToInt32(2*i)].Index] =
                                AnimationTransforms[Model.Bones["FrontWheel" + Convert.ToInt32(2*i)].Index]*
                                Matrix.CreateRotationZ(-rot);
                            AnimationTransforms[Model.Bones["FrontWheel" + Convert.ToInt32(2*i + 1)].Index] =
                                AnimationTransforms[Model.Bones["FrontWheel" + Convert.ToInt32(2*i + 1)].Index]*
                                Matrix.CreateRotationZ(rot);
                        }
                    }
                    break;
                case WheelState.StraightLeft:
                    rot = -turningSpeed * gameTime.ElapsedGameTime.Milliseconds;
                    wheelAngle += rot;
                    if (wheelAngle <= -maxAngle)
                    {
                        wheelState = WheelState.Left;
                        //TEMP
                        //WheelState = WheelState.LeftStraight;
                        //\TEMP
                        for (int i = 0; i < frontWheelsCount / 2; i++)
                        {
                            AnimationTransforms[Model.Bones["FrontWheel" + Convert.ToInt32(2*i)].Index] =
                                AnimationTransforms[Model.Bones["FrontWheel" + Convert.ToInt32(2*i)].Index]*
                                Matrix.CreateRotationZ(maxAngle + wheelAngle - rot);
                            AnimationTransforms[Model.Bones["FrontWheel" + Convert.ToInt32(2*i + 1)].Index] =
                                AnimationTransforms[Model.Bones["FrontWheel" + Convert.ToInt32(2*i + 1)].Index]*
                                Matrix.CreateRotationZ(-maxAngle - wheelAngle + rot);
                        }
                        wheelAngle = -maxAngle;
                    }
                    else
                    {
                        for (int i = 0; i < frontWheelsCount / 2; i++)
                        {
                            AnimationTransforms[Model.Bones["FrontWheel" + Convert.ToInt32(2*i)].Index] =
                                AnimationTransforms[Model.Bones["FrontWheel" + Convert.ToInt32(2*i)].Index]*
                                Matrix.CreateRotationZ(-rot);
                            AnimationTransforms[Model.Bones["FrontWheel" + Convert.ToInt32(2*i + 1)].Index] =
                                AnimationTransforms[Model.Bones["FrontWheel" + Convert.ToInt32(2*i + 1)].Index]*
                                Matrix.CreateRotationZ(rot);
                        }
                    }
                    break;
                case WheelState.LeftStraight:
                    rot = turningSpeed * gameTime.ElapsedGameTime.Milliseconds;
                    wheelAngle += rot;
                    if (wheelAngle >= 0)
                    {
                        wheelState = WheelState.Straight;
                        //TEMP
                        //WheelState = WheelState.StraightRight;
                        //\TEMP
                        for (int i = 0; i < frontWheelsCount / 2; i++)
                        {
                            AnimationTransforms[Model.Bones["FrontWheel" + Convert.ToInt32(2*i)].Index] =
                                AnimationTransforms[Model.Bones["FrontWheel" + Convert.ToInt32(2*i)].Index]*
                                Matrix.CreateRotationZ(wheelAngle - rot);
                            AnimationTransforms[Model.Bones["FrontWheel" + Convert.ToInt32(2*i + 1)].Index] =
                                AnimationTransforms[Model.Bones["FrontWheel" + Convert.ToInt32(2*i + 1)].Index]*
                                Matrix.CreateRotationZ(-wheelAngle + rot);
                        }
                        wheelAngle = 0;
                    }
                    else
                    {
                        for (int i = 0; i < frontWheelsCount / 2; i++)
                        {
                            AnimationTransforms[Model.Bones["FrontWheel" + Convert.ToInt32(2*i)].Index] =
                                AnimationTransforms[Model.Bones["FrontWheel" + Convert.ToInt32(2*i)].Index]*
                                Matrix.CreateRotationZ(-rot);
                            AnimationTransforms[Model.Bones["FrontWheel" + Convert.ToInt32(2*i + 1)].Index] =
                                AnimationTransforms[Model.Bones["FrontWheel" + Convert.ToInt32(2*i + 1)].Index]*
                                Matrix.CreateRotationZ(+rot);
                        }
                    }
                    break;
            }
        }

        private void AnimateDoor(GameTime gameTime)
        {
            const float doorSpeed = 0.0018f;
            if(leftDoorState == DoorState.Opening)
            {
                float rot = doorSpeed*gameTime.ElapsedGameTime.Milliseconds;
                leftDoorAngle += rot;
                if (leftDoorAngle >= MathHelper.PiOver2)
                {
                    leftDoorState = DoorState.Open;
                    //TEMP
                    //leftDoorState = DoorState.Closing;
                    //\TEMP
                    for (int i = 0; i < doorCount; i++)
                    {
                        AnimationTransforms[Model.Bones["DoorLeft" + Convert.ToString(i)].Index] =
                            Matrix.CreateRotationZ(MathHelper.PiOver2);
                    }
                }
                else
                {
                    for (int i = 0; i < doorCount; i++)
                    {
                        AnimationTransforms[Model.Bones["DoorLeft" + Convert.ToString(i)].Index] =
                            Matrix.CreateRotationZ(rot)*
                            AnimationTransforms[Model.Bones["DoorLeft" + Convert.ToString(i)].Index];
                    }
                }
            }
            else if (leftDoorState == DoorState.Closing)
            {
                float rot = -doorSpeed * gameTime.ElapsedGameTime.Milliseconds;
                leftDoorAngle += rot;
                if (leftDoorAngle <= 0)
                {
                    leftDoorState = DoorState.Closed;
                    //TEMP
                    //leftDoorState = DoorState.Opening;
                    //\TEMP
                    for (int i = 0; i < doorCount; ++i)
                    {
                        AnimationTransforms[Model.Bones["DoorLeft" + Convert.ToString(i)].Index] =
                            Matrix.Identity;
                    }
                }
                for (int i = 0; i < doorCount; i++)
                {
                    AnimationTransforms[Model.Bones["DoorLeft" + Convert.ToString(i)].Index] =
                        Matrix.CreateRotationZ(rot)*
                        AnimationTransforms[Model.Bones["DoorLeft" + Convert.ToString(i)].Index];
                }
            }
            if (rightDoorState == DoorState.Opening)
            {
                float rot = doorSpeed * gameTime.ElapsedGameTime.Milliseconds;
                rightDoorAngle += rot;
                if (rightDoorAngle >= MathHelper.PiOver2)
                {
                    rightDoorState = DoorState.Open;
                    //TEMP
                    //rightDoorState = DoorState.Closing;
                    //\TEMP
                    for (int i = 0; i < doorCount; i++)
                    {
                        AnimationTransforms[Model.Bones["DoorRight" + Convert.ToString(i)].Index] =
                            Matrix.CreateRotationZ(MathHelper.PiOver2);
                    }
                }
                for (int i = 0; i < doorCount; i++)
                {
                    AnimationTransforms[Model.Bones["DoorRight" + Convert.ToString(i)].Index] =
                        Matrix.CreateRotationZ(rot)*
                        AnimationTransforms[Model.Bones["DoorRight" + Convert.ToString(i)].Index];
                }
            }
            else if (rightDoorState == DoorState.Closing)
            {
                float rot = -doorSpeed * gameTime.ElapsedGameTime.Milliseconds;
                rightDoorAngle += rot;
                if (rightDoorAngle <= 0)
                {
                    rightDoorState = DoorState.Closed;
                    //TEMP
                    //rightDoorState = DoorState.Opening;
                    //\TEMP
                    for (int i = 0; i < doorCount; i++)
                    {
                        AnimationTransforms[Model.Bones["DoorRight" + Convert.ToString(i)].Index] =
                            Matrix.Identity;
                    }
                }
                for (int i = 0; i < doorCount; i++)
                {
                    AnimationTransforms[Model.Bones["DoorRight" + Convert.ToString(i)].Index] =
                        Matrix.CreateRotationZ(rot)*
                        AnimationTransforms[Model.Bones["DoorRight" + Convert.ToString(i)].Index];
                }
            }
        }

        private void AnimateWheel(GameTime gameTime, int wheelIndex, bool front)
        {
            int sign;
            switch (moving)
            {
                case Direction.Forward:
                    sign = 1;
                    break;
                case Direction.Backward:
                    sign = -1;
                    break;
                default:
                    sign = 0;
                    break;
            }

            string fr;
            if(front)
            {
                fr = "Front";
            }
            else
            {
                fr = "Rear";
            }

            AnimationTransforms[Model.Bones[fr+"Wheel" + Convert.ToString(wheelIndex)].Index] =
                Matrix.CreateRotationY(0.6f*gameTime.ElapsedGameTime.Milliseconds*Speed*sign)*
                AnimationTransforms[Model.Bones[fr+"Wheel" + Convert.ToString(wheelIndex)].Index];
        }

        private void AnimateDriving(GameTime gameTime)
        {
            for(int i =0; i < rearWheelsCount; ++i)
            {
                AnimateWheel(gameTime, i, false);
            }
            for(int i = 0; i < frontWheelsCount; ++i)
            {
                AnimateWheel(gameTime, i, true);
            }
        }

        public override void Animate(GameTime gameTime, List<GameObject> gameObjects)
        {
            
            AnimateTurn(gameTime);
            AnimateDriving(gameTime);
            AnimateDoor(gameTime);
            if (hasTurret)
            {
                AnimateTurret(gameTime);
            }
            base.Animate(gameTime, gameObjects);
        }

        public override GameObjectDrawer GetDrawer()
        {
            return new VehicleDrawer(this);
        }

        const float toleranceBig = 2f;
        const float toleranceSmall = 2f;

        private float toleranceX = toleranceSmall;
        private float toleranceY = toleranceSmall;

        public override void CalculateNextStep(GameTime gameTime, List<GameObject> gameObjects)
        {
            if(Path.Count > 0)
            {
                toleranceX = toleranceBig;
                toleranceY = toleranceBig;
            }
            else
            {
                toleranceX = toleranceSmall;
                toleranceY = toleranceSmall;
            }

            Vector2 curStep = new Vector2(NextStep.X - Position.X, NextStep.Y - Position.Z);

            curStep = new Vector2(curStep.X * Convert.ToSingle(Math.Cos(Angle.Y)) - curStep.Y * Convert.ToSingle(Math.Sin(Angle.Y)),
                curStep.Y * Convert.ToSingle(Math.Cos(Angle.Y)) + curStep.X * Convert.ToSingle(Math.Sin(Angle.Y)));

            //gi.ShowInfo("curStep: " + curStep.ToString() + "\r\nAngle: " + Angle.Y.ToString() + "\r\n WheelState" + WheelState.ToString() + "\r\n" + 
            //    Path.ToArray().ToString());

            if(Math.Abs(curStep.X) <= toleranceX && Math.Abs(curStep.Y) <= toleranceY)
            {
                if(Path != null && Path.Count > 0)
                {
                    NextStep = new Vector2(Path[0].X,Path[0].Y);
                    Path.RemoveAt(0);
                }
                else
                {
                    Move(Direction.None, gameTime);
                    Turn(Direction.None, gameTime);
                    return;
                }
            }

            if (Math.Abs(curStep.X) <= toleranceX)
            {
                if (Math.Abs(curStep.Y) <= toleranceY)
                {
                    Move(Direction.None, gameTime);
                    Turn(Direction.None, gameTime);
                }
                else if (curStep.Y > toleranceY)
                {
                    if (WheelState == WheelState.Left)
                    {
                        WheelState = WheelState.LeftStraight;
                    }
                    else if (WheelState == WheelState.Right)
                    {
                        WheelState = WheelState.RightStraight;
                    }

                    if (WheelState == WheelState.Straight || WheelState == WheelState.LeftStraight || WheelState == WheelState.RightStraight)
                    {
                        if (CheckMoveList(Direction.Forward, Direction.None, gameObjects, gameTime))
                        {
                            Move(Direction.Forward, gameTime);
                        }
                    }
                }
                else
                {
                    if (WheelState == WheelState.Left)
                    {
                        WheelState = WheelState.LeftStraight;
                    }
                    else if (WheelState == WheelState.Right)
                    {
                        WheelState = WheelState.RightStraight;
                    }

                    if (WheelState == WheelState.Straight || WheelState == WheelState.LeftStraight || WheelState == WheelState.RightStraight)
                    {
                        if (CheckMoveList(Direction.Backward, Direction.None, gameObjects, gameTime))
                        {
                            Move(Direction.Backward, gameTime);
                        }
                    }

                }
            }
            else if (curStep.X > toleranceX)
            {
                if(Math.Abs(curStep.Y) <= toleranceY)
                {
                    Path.Insert(0, new Point(Convert.ToInt32(NextStep.X), Convert.ToInt32(NextStep.Y)));

                    NextStep = CalculateBackingUp(Angle.Y, new Vector2(Position.X, Position.Z), 25.0);
                }
                else if (curStep.Y > toleranceY)
                {
                    if(WheelState == WheelState.Straight)
                    {
                        WheelState = WheelState.StraightLeft;
                    }
                    else if (WheelState == WheelState.Right)
                    {
                        WheelState = WheelState.RightStraight;
                    }
                    if (WheelState == WheelState.StraightLeft || WheelState == WheelState.Left)
                    {
                        if (CheckMoveList(Direction.Forward, Direction.Left, gameObjects, gameTime))
                        {
                            Move(Direction.Forward, gameTime);
                            Turn(Direction.Left, gameTime);
                        }
                    }
                }
                else 
                {
                    if(WheelState == WheelState.Straight)
                    {
                        WheelState = WheelState.StraightRight;
                    }
                    else if (WheelState == WheelState.Left)
                    {
                        WheelState = WheelState.LeftStraight;
                    }
                    if (WheelState == WheelState.StraightRight || WheelState == WheelState.Right)
                    {
                        if (CheckMoveList(Direction.Backward, Direction.Left, gameObjects, gameTime))
                        {
                            Move(Direction.Backward, gameTime);
                            Turn(Direction.Left, gameTime);
                        }
                    }
                }
            }
            else
            {
                if (Math.Abs(curStep.Y) <= toleranceY)
                {
                    Path.Insert(0, new Point(Convert.ToInt32(NextStep.X), Convert.ToInt32(NextStep.Y)));

                    NextStep = CalculateBackingUp(Angle.Y, new Vector2(Position.X, Position.Z), 25.0);
                }
                else if (curStep.Y > toleranceY)
                {
                    if (WheelState == WheelState.Straight)
                    {
                        WheelState = WheelState.StraightRight;
                    }
                    else if (WheelState == WheelState.Left)
                    {
                        WheelState = WheelState.LeftStraight;
                    }
                    if (WheelState == WheelState.StraightRight || WheelState == WheelState.Right)
                    {
                        if (CheckMoveList(Direction.Forward, Direction.Right, gameObjects, gameTime))
                        {
                            Move(Direction.Forward, gameTime);
                            Turn(Direction.Right, gameTime);
                        }
                    }
                }
                else
                {
                    if (WheelState == WheelState.Straight)
                    {
                        WheelState = WheelState.StraightRight;
                    }
                    else if (WheelState == WheelState.Left)
                    {
                        WheelState = WheelState.LeftStraight;
                    }
                    if (WheelState == WheelState.StraightRight || WheelState == WheelState.Right)
                    {
                        if (CheckMoveList(Direction.Backward, Direction.Left, gameObjects, gameTime))
                        {
                            Move(Direction.Backward, gameTime);
                            Turn(Direction.Left, gameTime);
                        }
                    }
                }
            }
        }
        public override void Move(Direction direction, GameTime gameTime)
        {
            moving = direction;
            base.Move(direction,gameTime);
        }

        //Probably temp... Ale potrzebne na zewnatrz!!!
        //BUG EPIC HACK
        public Vector3 GetWaterSourcePosition()
        {
            /*Matrix[] matrices = new Matrix[Model.Bones.Count];
            Model.CopyAbsoluteBoneTransformsTo(matrices);*/
            Matrix test = Model.Bones["WaterSource0"].Transform;
         //   test *= Matrix.CreateTranslation(0, -2.4f, 1);
            Vector3 Result = (test*Model.Bones[0].Transform*ModelMatrix).Translation;
            return Result;
        }
        //Temp jak diabli
        public Vector3 GetSecondWaterSourcePosition()
        {
            Matrix test = Model.Bones["WaterSource1"].Transform;
            
            Vector3 Result = (test * Model.Bones[0].Transform * ModelMatrix).Translation;
            return Result;
        }

        //Temp
        public void ActivateSpecialAction()
        {
            foreach (IObjectEffect objectEffect in EffectList)
            {
                if (objectEffect is WaterEffect)
                {
                    objectEffect.IsActive = true;
                }
            }
        }

        public void PointTurretToGameObject(GameObject gameObject)
        {
            var longitudinalDifference = gameObject.Position.X - Position.X;
            var latitudinalDifference = gameObject.Position.Z - Position.Z;
            double result = 0;
            var azimuth = (Math.PI * .5d) - Math.Atan(latitudinalDifference / longitudinalDifference);
            if (longitudinalDifference > 0) result = azimuth;
            else if (longitudinalDifference < 0) result = azimuth + Math.PI;
            else if (latitudinalDifference < 0)
                result = Math.PI;
            else
                result = 0d;
            turretDestination = (float)(result - Angle.Y);
        }
    }
}
