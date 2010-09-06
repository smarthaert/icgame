using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        private readonly int wheelsCount;
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

        #endregion

        public Vehicle(Model model, float speed, float turnRadius, int wheelsCount)
            : base(model, speed, turnRadius)
        {
            this.wheelsCount = wheelsCount;

            leftDoorAngle = 0;
            rightDoorAngle = 0;
            leftDoorState = DoorState.Closed;
            rightDoorState = DoorState.Closed;

            wheelAngle = 0;
            wheelState = WheelState.Straight;

            Destination = new Vector2(Position.X,Position.Z);

            //TEMP
            leftDoorState = DoorState.Opening;
            rightDoorState = DoorState.Opening;
            //wheelState = WheelState.StraightLeft;
            gi = new GameInfo();
            //\TEMP

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
                        AnimationTransforms[Model.Bones["Wheel0"].Index] =
                            AnimationTransforms[Model.Bones["Wheel0"].Index] *
                            Matrix.CreateRotationZ(-maxAngle + wheelAngle-rot);
                        AnimationTransforms[Model.Bones["Wheel1"].Index] =
                            AnimationTransforms[Model.Bones["Wheel1"].Index] *
                            Matrix.CreateRotationZ(maxAngle - wheelAngle+rot);
                        wheelAngle = maxAngle;
                    }
                    else
                    {
                        
                        AnimationTransforms[Model.Bones["Wheel0"].Index] =
                            AnimationTransforms[Model.Bones["Wheel0"].Index] * 
                            Matrix.CreateRotationZ(-rot);
                        AnimationTransforms[Model.Bones["Wheel1"].Index] =
                            AnimationTransforms[Model.Bones["Wheel1"].Index] *
                            Matrix.CreateRotationZ(rot);
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
                        AnimationTransforms[Model.Bones["Wheel0"].Index] =
                            AnimationTransforms[Model.Bones["Wheel0"].Index] *
                            Matrix.CreateRotationZ(wheelAngle-rot);
                        AnimationTransforms[Model.Bones["Wheel1"].Index] =
                            AnimationTransforms[Model.Bones["Wheel1"].Index] *
                            Matrix.CreateRotationZ(-wheelAngle+rot);
                        wheelAngle = 0;
                    }
                    else
                    {
                        AnimationTransforms[Model.Bones["Wheel0"].Index] =
                            AnimationTransforms[Model.Bones["Wheel0"].Index]*
                            Matrix.CreateRotationZ(-rot);
                        AnimationTransforms[Model.Bones["Wheel1"].Index] =
                            AnimationTransforms[Model.Bones["Wheel1"].Index]*
                            Matrix.CreateRotationZ(rot);
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
                        AnimationTransforms[Model.Bones["Wheel0"].Index] =
                            AnimationTransforms[Model.Bones["Wheel0"].Index] *
                            Matrix.CreateRotationZ(maxAngle + wheelAngle - rot);
                        AnimationTransforms[Model.Bones["Wheel1"].Index] =
                            AnimationTransforms[Model.Bones["Wheel1"].Index] *
                            Matrix.CreateRotationZ(-maxAngle - wheelAngle + rot);
                        wheelAngle = -maxAngle;
                    }
                    else
                    {
                        AnimationTransforms[Model.Bones["Wheel0"].Index] =
                            AnimationTransforms[Model.Bones["Wheel0"].Index]*
                            Matrix.CreateRotationZ(-rot);
                        AnimationTransforms[Model.Bones["Wheel1"].Index] =
                            AnimationTransforms[Model.Bones["Wheel1"].Index]*
                            Matrix.CreateRotationZ(rot);
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
                        AnimationTransforms[Model.Bones["Wheel0"].Index] =
                            AnimationTransforms[Model.Bones["Wheel0"].Index] *
                            Matrix.CreateRotationZ(wheelAngle-rot);
                        AnimationTransforms[Model.Bones["Wheel1"].Index] =
                            AnimationTransforms[Model.Bones["Wheel1"].Index] *
                            Matrix.CreateRotationZ(-wheelAngle+rot);
                        wheelAngle = 0;
                    }
                    else
                    {
                        AnimationTransforms[Model.Bones["Wheel0"].Index] =
                            AnimationTransforms[Model.Bones["Wheel0"].Index]*
                            Matrix.CreateRotationZ(-rot);
                        AnimationTransforms[Model.Bones["Wheel1"].Index] =
                            AnimationTransforms[Model.Bones["Wheel1"].Index]*
                            Matrix.CreateRotationZ(+rot);
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
                    leftDoorState = DoorState.Closing;
                    //\TEMP
                    AnimationTransforms[Model.Bones["DoorLeft"].Index] =
                        Matrix.CreateRotationY(MathHelper.PiOver2);
                }
                else
                {
                    AnimationTransforms[Model.Bones["DoorLeft"].Index] =
                        Matrix.CreateRotationY(rot)*
                        AnimationTransforms[Model.Bones["DoorLeft"].Index];
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
                    leftDoorState = DoorState.Opening;
                    //\TEMP
                    AnimationTransforms[Model.Bones["DoorLeft"].Index] =
                        Matrix.Identity;
                }
                AnimationTransforms[Model.Bones["DoorLeft"].Index] =
                    Matrix.CreateRotationY(rot) *
                    AnimationTransforms[Model.Bones["DoorLeft"].Index];
            }
            if (rightDoorState == DoorState.Opening)
            {
                float rot = doorSpeed * gameTime.ElapsedGameTime.Milliseconds;
                rightDoorAngle += rot;
                if (rightDoorAngle >= MathHelper.PiOver2)
                {
                    rightDoorState = DoorState.Open;
                    //TEMP
                    rightDoorState = DoorState.Closing;
                    //\TEMP
                    AnimationTransforms[Model.Bones["DoorRight"].Index] =
                        Matrix.CreateRotationY(MathHelper.PiOver2);
                }
                AnimationTransforms[Model.Bones["DoorRight"].Index] =
                    Matrix.CreateRotationY(rot) *
                    AnimationTransforms[Model.Bones["DoorRight"].Index];
            }
            else if (rightDoorState == DoorState.Closing)
            {
                float rot = -doorSpeed * gameTime.ElapsedGameTime.Milliseconds;
                rightDoorAngle += rot;
                if (rightDoorAngle <= 0)
                {
                    rightDoorState = DoorState.Closed;
                    //TEMP
                    rightDoorState = DoorState.Opening;
                    //\TEMP
                    AnimationTransforms[Model.Bones["DoorRight"].Index] =
                        Matrix.Identity;
                }
                AnimationTransforms[Model.Bones["DoorRight"].Index] =
                    Matrix.CreateRotationY(rot) *
                    AnimationTransforms[Model.Bones["DoorRight"].Index];
            }
        }

        private void AnimateWheel(GameTime gameTime, int wheelIndex)
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
            AnimationTransforms[Model.Bones["Wheel" + Convert.ToString(wheelIndex)].Index] =
                Matrix.CreateRotationY(0.01f*gameTime.ElapsedGameTime.Milliseconds*Speed*sign)*
                AnimationTransforms[Model.Bones["Wheel" + Convert.ToString(wheelIndex)].Index];
        }

        private void AnimateDriving(GameTime gameTime)
        {
            for(int i =0; i < wheelsCount; ++i)
            {
                AnimateWheel(gameTime,i);
            }
        }

        public override void Animate(GameTime gameTime)
        {
            
            AnimateTurn(gameTime);
            AnimateDriving(gameTime);
            AnimateDoor(gameTime);
            AnimateTurret(gameTime);
            base.Animate(gameTime);
        }

        public override GameObjectDrawer GetDrawer()
        {
            return new VehicleDrawer(this);
        }

        const float toleranceBig = 2f;
        const float toleranceSmall = 2f;

        private float toleranceX = toleranceSmall;
        private float toleranceY = toleranceSmall;

        public override void CalculateNextStep(GameTime gameTime)
        {
            NextStep = new Vector2(Destination.X - Position.X, Destination.Y - Position.Z);
            NextStep = new Vector2(NextStep.X * Convert.ToSingle(Math.Cos(Angle.Y)) - NextStep.Y * Convert.ToSingle(Math.Sin(Angle.Y)),
                NextStep.Y * Convert.ToSingle(Math.Cos(Angle.Y)) + NextStep.X * Convert.ToSingle(Math.Sin(Angle.Y)));

            gi.ShowInfo("NextStep: " + NextStep.ToString() + "\r\nAngle: " + Angle.Y.ToString() + "\r\n WheelState" + WheelState.ToString());

            if (NextStep.X > toleranceX)
            {
                toleranceX = toleranceSmall;
                if (NextStep.Y > toleranceY)
                {
                    toleranceY = toleranceSmall;
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
                        Move(Direction.Forward, gameTime);
                        Turn(Direction.Left, gameTime);
                    }
                }
                else if (NextStep.Y < toleranceY)
                {
                    toleranceY = toleranceSmall;
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
                        Move(Direction.Backward, gameTime);
                        Turn(Direction.Left, gameTime);
                    }
                }
                else
                {
                    toleranceY = toleranceBig;
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
                        Move(Direction.Backward, gameTime);
                        Turn(Direction.Right, gameTime);
                    }
                }
            }
            else if (Math.Abs(NextStep.X) <= toleranceX)
            {
                toleranceX = toleranceBig;
                if (Math.Abs(NextStep.Y) <= toleranceY)
                {
                    toleranceY = toleranceBig;
                    Move(Direction.None, gameTime);
                    Turn(Direction.None, gameTime);
                }
                else if (NextStep.Y > toleranceY)
                {
                    toleranceY = toleranceSmall;
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
                        Move(Direction.Forward, gameTime);
                    }
                }
                else if (NextStep.Y < toleranceY)
                {
                    toleranceY = toleranceSmall;

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
                        Move(Direction.Backward, gameTime);
                    }
                }
            }
            else
            {
                toleranceX = toleranceSmall;
                if (NextStep.Y < toleranceY)
                {
                    toleranceY = toleranceSmall;
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
                        Move(Direction.Backward, gameTime);
                        Turn(Direction.Left, gameTime);
                    }
                }
                else if (NextStep.Y > toleranceY)
                {
                    toleranceY = toleranceSmall;
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
                        Move(Direction.Forward, gameTime);
                        Turn(Direction.Right, gameTime);
                    }
                }
                else
                {
                    toleranceY = toleranceBig;
                    if (WheelState == WheelState.Straight)
                    {
                        WheelState = WheelState.StraightLeft;
                    }
                    else if (WheelState == WheelState.Right)
                    {
                        WheelState = WheelState.RightStraight;
                    }
                    if (WheelState == WheelState.StraightLeft || WheelState == WheelState.Left)
                    {
                        Move(Direction.Backward, gameTime);
                        Turn(Direction.Left, gameTime);
                    }
                }
            }
        }
        public void Move(Direction direction, GameTime gameTime)
        {
            moving = direction;
            base.Move(direction,gameTime);
        }
    }
}
