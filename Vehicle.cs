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

        public Vehicle(Model model, float speed, int wheelsCount)
            : base(model, speed)
        {
            this.wheelsCount = wheelsCount;

            leftDoorAngle = 0;
            rightDoorAngle = 0;
            leftDoorState = DoorState.Closed;
            rightDoorState = DoorState.Closed;

            wheelAngle = 0;
            wheelState = WheelState.Straight;

            //TEMP
            leftDoorState = DoorState.Opening;
            rightDoorState = DoorState.Opening;
            wheelState = WheelState.StraightLeft;
            //\TEMP
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
                        WheelState = WheelState.RightStraight;
                        //\TEMP
                        AnimationTransforms[Model.Bones["Wheel0"].Index] =
                            AnimationTransforms[Model.Bones["Wheel0"].Index] *
                            Matrix.CreateRotationZ(maxAngle - wheelAngle+rot);
                        AnimationTransforms[Model.Bones["Wheel1"].Index] =
                            AnimationTransforms[Model.Bones["Wheel1"].Index] *
                            Matrix.CreateRotationZ(maxAngle - wheelAngle+rot);
                        wheelAngle = maxAngle;
                    }
                    else
                    {
                        
                        AnimationTransforms[Model.Bones["Wheel0"].Index] =
                            AnimationTransforms[Model.Bones["Wheel0"].Index] * 
                            Matrix.CreateRotationZ(rot);
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
                        WheelState = WheelState.StraightLeft;
                        //\TEMP
                        AnimationTransforms[Model.Bones["Wheel0"].Index] =
                            AnimationTransforms[Model.Bones["Wheel0"].Index] *
                            Matrix.CreateRotationZ(-wheelAngle+rot);
                        AnimationTransforms[Model.Bones["Wheel1"].Index] =
                            AnimationTransforms[Model.Bones["Wheel1"].Index] *
                            Matrix.CreateRotationZ(-wheelAngle+rot);
                        wheelAngle = 0;
                    }
                    else
                    {
                        AnimationTransforms[Model.Bones["Wheel0"].Index] =
                            AnimationTransforms[Model.Bones["Wheel0"].Index]*
                            Matrix.CreateRotationZ(rot);
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
                        WheelState = WheelState.LeftStraight;
                        //\TEMP
                        AnimationTransforms[Model.Bones["Wheel0"].Index] =
                            AnimationTransforms[Model.Bones["Wheel0"].Index] *
                            Matrix.CreateRotationZ(-maxAngle - wheelAngle + rot);
                        AnimationTransforms[Model.Bones["Wheel1"].Index] =
                            AnimationTransforms[Model.Bones["Wheel1"].Index] *
                            Matrix.CreateRotationZ(-maxAngle - wheelAngle + rot);
                        wheelAngle = -maxAngle;
                    }
                    else
                    {
                        AnimationTransforms[Model.Bones["Wheel0"].Index] =
                            AnimationTransforms[Model.Bones["Wheel0"].Index]*
                            Matrix.CreateRotationZ(rot);
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
                        WheelState = WheelState.StraightRight;
                        //\TEMP
                        AnimationTransforms[Model.Bones["Wheel0"].Index] =
                            AnimationTransforms[Model.Bones["Wheel0"].Index] *
                            Matrix.CreateRotationZ(-wheelAngle+rot);
                        AnimationTransforms[Model.Bones["Wheel1"].Index] =
                            AnimationTransforms[Model.Bones["Wheel1"].Index] *
                            Matrix.CreateRotationZ(-wheelAngle+rot);
                        wheelAngle = 0;
                    }
                    else
                    {
                        AnimationTransforms[Model.Bones["Wheel0"].Index] =
                            AnimationTransforms[Model.Bones["Wheel0"].Index]*
                            Matrix.CreateRotationZ(rot);
                        AnimationTransforms[Model.Bones["Wheel1"].Index] =
                            AnimationTransforms[Model.Bones["Wheel1"].Index]*
                            Matrix.CreateRotationZ(rot);
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
                Matrix.CreateRotationY(0.01f*gameTime.ElapsedGameTime.Milliseconds*speed*sign)*
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
            base.Animate(gameTime);
        }
    }
}
