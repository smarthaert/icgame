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
        private Vector3 cameraPosition=new Vector3(0,0,0);

        private const float rotationSpeed = 0.01f;
        private const float heightChangeSpeed = 0.3f;
        private const float movementSpeed = 0.2f;

        public Camera()
        {
        }

        public Camera(Vector3 position)
        {
            lookAtPosition = position;   
        }


        /// <summary>
        /// Zwraca macierz kamery obliczoną dla aktualnej pozycji na .
        /// </summary>

        public Matrix CameraMatrix
        {
            get
            {
                return Matrix.CreateLookAt(cameraPosition, new Vector3(lookAtPosition.X,0,lookAtPosition.Z), Vector3.Transform(new Vector3(0,1,0),Matrix.CreateFromQuaternion(rotation)));
                
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

        public void CalculateCamera()
        {
            cameraPosition = Vector3.Transform(new Vector3(0, 80, 20.0f), Matrix.CreateFromQuaternion(rotation*rotation2))+ lookAtPosition;

        }

        protected void Rotate(float angle)
        {
            Quaternion additionalRot = Quaternion.CreateFromAxisAngle(new Vector3(0, 1, 0),angle);     
            rotation *= additionalRot;            

        }

        protected void RotateX(float angle)
        {
            Quaternion additionalRot = Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), angle);
            rotation2 *= additionalRot;

        }

        /// <summary>
        /// Oblicza nowe wartości pozycji kamery, na podstawie wyznaczonych wektorów.
        /// Nie do użytku poza klasą. Korzystać przez akcesory.
        /// </summary>
        /// <param name="dX">Przesunięcie na osi X</param>
        /// <param name="dY">Przesunięcie na osi Z</param>

        protected void Move(float dX, float dZ)
        {
            lookAtPosition += dZ * Vector3.Transform(new Vector3(0, 0, -1), Matrix.CreateFromQuaternion(rotation));
            lookAtPosition += dX * Vector3.Transform(new Vector3(1, 0, 0), Matrix.CreateFromQuaternion(rotation)); 
        }

        protected void ChangeHeight(float dY)
        {
            lookAtPosition += dY * Vector3.Transform(new Vector3(0, 1, 0), Matrix.CreateFromQuaternion(rotation));
            //Cofnij odrobinę, dla lepszego efektu...
            Move(0,-dY/3);
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

        public void ChangeHeightAccordingToMouseWheel(int dScroll)
        {
            ChangeHeight(dScroll*heightChangeSpeed/30);
        }
        #endregion
    }
}


