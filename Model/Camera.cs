using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace ICGame
{
    public class Camera
    {
       
        private Vector3 lookAtPosition = new Vector3(0, 0, 0);
        private Quaternion rotation = Quaternion.Identity;
        private Quaternion rotation2 = Quaternion.Identity;
        private Vector3 cameraPosition=new Vector3(250,0,164);
        private Vector3 cameraAdditionalPosition = new Vector3(0,70,20);
        
        //Kolizje
        private Vector3 collisionFreeLookAt=new Vector3(0,0,0);
        private Vector3 collisionFreeCameraAdditionalPosition = new Vector3(0, 0, 0);

        private const float rotationSpeed = 0.01f;
        private const float heightChangeSpeed = 0.3f;
        private const float movementSpeed = 0.2f;

        
        public Camera(Vector3 position, MissionController missionController)
        {
            lookAtPosition = position;
            MissionController = missionController;
            collisionFreeLookAt = position;
            
            
        }

        public MissionController MissionController
        {
            get; set;
        }

        /// <summary>
        /// Zwraca macierz kamery obliczoną dla aktualnej pozycji na .
        /// </summary>

        public Matrix CameraMatrix
        {
            get
            {
                return Matrix.CreateLookAt(cameraPosition, lookAtPosition, Vector3.Transform(new Vector3(0,5,0),Matrix.CreateFromQuaternion(rotation)));
                
            }
        }

        public Vector3 CameraPosition
        {
            get
            {
                return cameraPosition;
            }
            set
            {
                cameraPosition = value;
            }
        }

        public Vector3 CalculateCamera()
        {
            
            CameraPosition = Vector3.Transform(cameraAdditionalPosition, Matrix.CreateFromQuaternion(rotation*rotation2)) + lookAtPosition;
            return CameraPosition;
        }

        protected void Rotate(float angle)
        {
            
            if (!TerrainCollision(0))
            {
                Quaternion additionalRot = Quaternion.CreateFromAxisAngle(new Vector3(0, 1, 0), angle);
                rotation *= additionalRot;
                CalculateCamera();
            }

        }

        protected void RotateX(float angle)
        {
            Quaternion additionalRot = Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), angle);
            if (!TerrainCollision(-angle))
            {
                //Quaternion tempRotation = rotation2;
              
                rotation2 *= additionalRot;
                CalculateCamera();
         //       if ((Math.Abs(CameraPosition.X - lookAtPosition.X) < Math.Abs(angle) + 10) && (Math.Abs(CameraPosition.Z - lookAtPosition.Z) < Math.Abs(angle) + 10))
                //if (rotation2.W - 0.1f < 0 && angle<0)
//                return atan2(2 * (y * z + w * x), w * w - x * x - y * y + z * z);
                double quatAngle = Math.Atan2(2*(rotation2.Y*rotation2.Z+rotation2.W*rotation2.X),rotation2.W*rotation2.W-rotation2.X*rotation2.X-rotation2.Y*rotation2.Y+rotation2.Z*rotation2.Z);
                if(quatAngle<0)
                {
                    rotation2 /= additionalRot;
                    CalculateCamera();
                }
            }

            if(TerrainCollision(-angle))
            {
                rotation2 /= additionalRot;
                CalculateCamera();
            }
        }

        /// <summary>
        /// Oblicza nowe wartości pozycji kamery, na podstawie wyznaczonych wektorów.
        /// Nie do użytku poza klasą. Korzystać przez akcesory.
        /// </summary>
        /// <param name="dX">Przesunięcie na osi X</param>
        /// <param name="dY">Przesunięcie na osi Z</param>

        protected void Move(float dX, float dZ)
        {
            if(!TerrainCollision(0))
                collisionFreeLookAt = new Vector3(lookAtPosition.X,lookAtPosition.Y, lookAtPosition.Z);
            
            lookAtPosition += dZ * Vector3.Transform(new Vector3(0, 0, -1), Matrix.CreateFromQuaternion(rotation));
            lookAtPosition += dX * Vector3.Transform(new Vector3(1, 0, 0), Matrix.CreateFromQuaternion(rotation));
            CalculateCamera();
            if (TerrainCollision(0))
            {
                lookAtPosition = collisionFreeLookAt;
                
                CalculateCamera();
            }
        }

        public bool TerrainCollision(float dY)
        {
            if ((MissionController.Mission.Board.GetHeight(CameraPosition.X, CameraPosition.Z) + 10.0f<= CameraPosition.Y)||dY>0)
                return false;
            return true;
        }

        protected void ChangeHeight(float dY)
        {
            if(!TerrainCollision(dY))
                collisionFreeCameraAdditionalPosition = cameraAdditionalPosition;
            Vector3 heightValue = cameraAdditionalPosition + dY * Vector3.Transform(new Vector3(0, 1, 0), Matrix.CreateFromQuaternion(rotation));

            if (heightValue.Y < 200.0f)
            {
                cameraAdditionalPosition = heightValue;

            }
            //Cofnij odrobinę, dla lepszego efektu...
            CalculateCamera();

            if (TerrainCollision(dY))
            {
                cameraAdditionalPosition = collisionFreeCameraAdditionalPosition;
          
                CalculateCamera();
            }

        }


        /// <summary>
        /// Akcesory kamery
        /// </summary>
        #region AkcesoryKamery
        public void MoveForward()
        {
            Move(0,movementSpeed);
        }

        public void MoveBack()
        {
            Move(0,-movementSpeed);
        }

        public void MoveLeft()
        {
            Move(-movementSpeed,0);
        }

        public void MoveRight()
        {
            Move(movementSpeed, 0);
        }

        public void MoveUp()
        {
            ChangeHeight(heightChangeSpeed);
        }

        public void MoveDown()
        {
            ChangeHeight(-heightChangeSpeed);
        }

        public void RotateLeft()
        {
            Rotate(-rotationSpeed);
        }

        public void RotateRight()
        {
            Rotate(rotationSpeed);
        }

        public void TransformCameraAccordingToMouseTravel(int dx, int dy)
        {
            Move(dx*-movementSpeed,dy*movementSpeed);
        }

        public void RotateCameraAccordingToMouseTravel(int dx, int dy)
        {
            Rotate(dx*rotationSpeed);
            RotateX(dy*rotationSpeed);
        }

        public void ChangeHeightAccordingToMouseWheel(int dScroll)
        {
            ChangeHeight(dScroll*heightChangeSpeed/30);
        }
        #endregion
    }
}


