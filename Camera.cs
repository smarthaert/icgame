using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace ICGame
{
    public class Camera
    {
        private float pitch=0;
        private float yaw=0;
        private float roll=0;

        private Vector3 forward=new Vector3(1,0,0);
        private Vector3 up=new Vector3(0,1,0);
        private Vector3 side=new Vector3(0,0,0);

        private Vector3 position=new Vector3(0,0,50);

        public Camera()
        {
            
        }

        public Camera(Vector3 forward, Vector3 up, Vector3 position)
        {
            this.forward = forward;
            this.position = position;
            this.up = up;
        }


        /// <summary>
        /// Zwraca macierz kamery obliczoną dla aktualnej pozycji na .
        /// </summary>
        
        public Matrix CameraMatrix
        {
            get
            {
                CalculateCamera();
                Vector3 temporary = position + forward;
                temporary.Y = 0;
                temporary.Z -= position.Y;
                return Matrix.CreateLookAt(position, temporary, up);
            }
        }

        private void CalculateCamera()
        {
            float cosY = (float)Math.Cos(yaw);
            float cosP = (float)Math.Cos(pitch);
            float cosR = (float)Math.Cos(roll);

            float sinY = (float)Math.Sin(yaw);
            float sinP = (float)Math.Sin(pitch);
            float sinR = (float)Math.Sin(roll);

            forward.X = sinY * cosP;
          //  forward.Y = sinP;
            forward.Z = cosP * -cosY;

            up.X = -cosY * sinR - sinY * sinP * cosR;
            up.Y = cosP * cosR;
            up.Z = -sinY * sinR - sinP * cosR * -cosY;

            side = Vector3.Cross(forward, up);

        }

        public void Rotate(float angle)
        {
            yaw += angle;
        }

        /// <summary>
        /// Oblicza nowe wartości pozycji kamery, na podstawie wyznaczonych wektorów.
        /// Przed obliczeniem uruchamia CalculateCamera.
        /// </summary>
        /// <param name="dX">Przesunięcie na osi X</param>
        /// <param name="dY">Przesunięcie na osi Z</param>

        public void Move(float dX, float dZ)
        {
            position += dZ*forward;
            position += dX*side;
         

        }

        public void ChangeHeight(float delta)
        {
           // pitch -= delta;
            //if ((position.Y >= 0-delta && delta >= 0) || (position.Y <= 4.0f && delta < 0+delta)) 
            {
                position.Y += delta*5; //Do policzenia...  
                Move(0, -delta/2);
            }


        }
    }
}
