using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ICGame
{
    /// <summary>
    /// Object Oriented Bounding Box
    /// </summary>
    public class OOBoundingBox
    {
        private Vector3 rotation;
        public Vector3 Position { get; set; }
        public Vector3 Rotation 
        { 
            get
            {
                return rotation;
            } 
            set
            {
                Matrix rotMatrix = Matrix.CreateRotationX(value.X)*Matrix.CreateRotationY(value.Y)*
                                   Matrix.CreateRotationZ(value.Z);
                NormalX = new Vector3(1,0,0);
                NormalX = Vector3.Transform(NormalX, rotMatrix);
                NormalY = new Vector3(0, 1, 0);
                NormalY = Vector3.Transform(NormalY, rotMatrix); 
                NormalZ = new Vector3(0, 0, 1);
                NormalZ = Vector3.Transform(NormalZ, rotMatrix);
                rotation = value;
            } 
        }
        public Vector3 Size { get; set; }
        public Vector3 NormalX { get; private set; }
        public Vector3 NormalY { get; private set; }
        public Vector3 NormalZ { get; private set; }

        public OOBoundingBox(Vector3 position, Vector3 size)
        {
            Position = position;
            Rotation = new Vector3(0,0,0);
            Size = size;
        }
        public OOBoundingBox(BoundingBox boundingBox, float scale)
        {
            Size = new Vector3(scale*(boundingBox.Max.X - boundingBox.Min.X),
                               scale*(boundingBox.Max.Y - boundingBox.Min.Y),
                               scale * (boundingBox.Max.Z - boundingBox.Min.Z));
            Rotation = new Vector3(0, 0, 0);
            Position = new Vector3(0, 0, 0);

        }

        private bool SpanOverlap(float min0, float max0, float min1, float max1)
        {
            return (min0 > max1 || max0 < min1);
        }

        private void ComputeSpan(Vector3 Axis, out float min, out float max)
        {
            float p = Vector3.Dot(Axis,Position);
            float r = Math.Abs(Vector3.Dot(Axis,NormalX)) * Size.X/2 + Math.Abs(Vector3.Dot(Axis,NormalY)) * Size.Y/2 + 
                Math.Abs(Vector3.Dot(Axis,NormalZ)) * Size.Z/2;

            min = p-r;
            max = p+r;
        }
        private bool AxisOverlap(Vector3 Axis, OOBoundingBox boundingBox)
        {
            float min0, max0;
            float min1, max1;
            ComputeSpan(Axis, out min0, out max0);
            boundingBox.ComputeSpan(Axis, out min1, out max1);

            return SpanOverlap(min0, max0, min1, max1);
        }

        /// <summary>
        /// Sprawdza, czy dwa OOBoundingBox'y koliduja.
        /// </summary>
        /// <param name="boundingBox">OOBoundingBox, kolizja z ktorym jest sprawdzana</param>
        /// <returns></returns>
        public bool Intersects(OOBoundingBox boundingBox)
        {
            if (AxisOverlap(NormalX, boundingBox))
                return true;
            if (AxisOverlap(NormalY, boundingBox))
                return true;
            if (AxisOverlap(NormalZ, boundingBox))
                return true;

            if (AxisOverlap(Vector3.Cross(NormalX, boundingBox.NormalX), boundingBox))
                return true;
            if (AxisOverlap(Vector3.Cross(NormalX, boundingBox.NormalY), boundingBox))
                return true;
            if (AxisOverlap(Vector3.Cross(NormalX, boundingBox.NormalZ), boundingBox))
                return true;
            
            if (AxisOverlap(Vector3.Cross(NormalY, boundingBox.NormalX), boundingBox))
                return true;
            if (AxisOverlap(Vector3.Cross(NormalY, boundingBox.NormalY), boundingBox))
                return true;
            if (AxisOverlap(Vector3.Cross(NormalY, boundingBox.NormalZ), boundingBox))
                return true;
            
            if (AxisOverlap(Vector3.Cross(NormalZ, boundingBox.NormalX), boundingBox))
                return true;
            if (AxisOverlap(Vector3.Cross(NormalZ, boundingBox.NormalY), boundingBox))
                return true;
            if (AxisOverlap(Vector3.Cross(NormalZ, boundingBox.NormalZ), boundingBox))
                return true;

            return false;
        }


    }
}
