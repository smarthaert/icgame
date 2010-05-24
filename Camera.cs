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
        private Vector3 cameraPosition=new Vector3(0,0,0);

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
                CalculateCamera();
                return Matrix.CreateLookAt(cameraPosition, new Vector3(lookAtPosition.X,0,lookAtPosition.Z), Vector3.Transform(new Vector3(0,1,0),Matrix.CreateFromQuaternion(rotation)));
                
            }
        }

        private void CalculateCamera()
        {
            cameraPosition = Vector3.Transform(new Vector3(0, 0, 20.0f), Matrix.CreateFromQuaternion(rotation))+ lookAtPosition;

        }

        public void Rotate(float angle)
        {
            Quaternion additionalRot = Quaternion.CreateFromAxisAngle(new Vector3(0, 1, 0),angle);     
            rotation *= additionalRot;            

        }

        /// <summary>
        /// Oblicza nowe wartości pozycji kamery, na podstawie wyznaczonych wektorów.
        /// Przed obliczeniem uruchamia CalculateCamera.
        /// </summary>
        /// <param name="dX">Przesunięcie na osi X</param>
        /// <param name="dY">Przesunięcie na osi Z</param>

        public void Move(float dX, float dZ)
        {
            lookAtPosition += dZ * Vector3.Transform(new Vector3(0, 0, -1), Matrix.CreateFromQuaternion(rotation));
            lookAtPosition += dX * Vector3.Transform(new Vector3(1, 0, 0), Matrix.CreateFromQuaternion(rotation)); 
        }

        public void ChangeHeight(float dY)
        {
            
            lookAtPosition += dY * Vector3.Transform(new Vector3(0, 1, 0), Matrix.CreateFromQuaternion(rotation));
            Move(0,-dY);


        }

	}
}


