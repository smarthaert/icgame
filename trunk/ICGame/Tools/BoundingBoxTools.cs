using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ICGame.Tools
{
    static class BoundingBoxTools
    {
        public static BoundingBox TransformBoundingBox(BoundingBox boundingBox, Matrix matrix)
        {
            Vector3[] nodes = new Vector3[8];

            nodes[0] = boundingBox.Min;
            nodes[1] = new Vector3(boundingBox.Min.X, boundingBox.Min.Y, boundingBox.Max.Z);
            nodes[2] = new Vector3(boundingBox.Min.X, boundingBox.Max.Y, boundingBox.Min.Z);
            nodes[3] = new Vector3(boundingBox.Min.X, boundingBox.Max.Y, boundingBox.Max.Z);
            nodes[4] = new Vector3(boundingBox.Max.X, boundingBox.Min.Y, boundingBox.Min.Z);
            nodes[5] = new Vector3(boundingBox.Max.X, boundingBox.Min.Y, boundingBox.Max.Z);
            nodes[6] = new Vector3(boundingBox.Max.X, boundingBox.Max.Y, boundingBox.Min.Z);
            nodes[7] = boundingBox.Max;

            for (int i = 0; i < 8; ++i)
            {
                nodes[i] = Vector3.Transform(nodes[i], matrix);
            }
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            for (int i = 0; i < 8; i++)
            {
                if (nodes[i].X < min.X)
                {
                    min.X = nodes[i].X;
                }
                else if (nodes[i].X > max.X)
                {
                    max.X = nodes[i].X;
                }
                if (nodes[i].Y < min.Y)
                {
                    min.Y = nodes[i].Y;
                }
                else if (nodes[i].Y > max.Y)
                {
                    max.Y = nodes[i].Y;
                }
                if (nodes[i].Z < min.Z)
                {
                    min.Z = nodes[i].Z;
                }
                else if (nodes[i].Z > max.Z)
                {
                    max.Z = nodes[i].Z;
                }
            }

            
            return new BoundingBox(min, max);
        }

        private static Vector3[] tempVecs3 = new Vector3[512];
        private static ushort[] tempUshorts = new ushort[512 * 3];

        public static void CalculateBoundingBox(Model model, out BoundingBox boundingBox)
        {
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);      //23 - alertbar
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
            
            for (int i = 0; i < model.Meshes.Count; ++i)
            {
                if (model.Meshes[i].BoundingSphere.Radius > 0)
                {
                    BoundingBox bb;
                    BoundingBoxTools.CalculateBoundingBox(model.Meshes[i], out bb);

                    if (min.X > bb.Min.X)
                    {
                        min.X = bb.Min.X;
                    }
                    if (min.Y > bb.Min.Y)
                    {
                        min.Y = bb.Min.Y;
                    }
                    if (min.Z > bb.Min.Z)
                    {
                        min.Z = bb.Min.Z;
                    }
                    if (max.X < bb.Max.X)
                    {
                        max.X = bb.Max.X;
                    }
                    if (max.Y < bb.Max.Y)
                    {
                        max.Y = bb.Max.Y;
                    }
                    if (max.Z < bb.Max.Z)
                    {
                        max.Z = bb.Max.Z;
                    }
                }
            }
            boundingBox = new BoundingBox(min, max);     //Siedlisko śmierdzącego zła... Zatkaj nos Boo...
        }

        public static void CalculateBoundingBox(ModelMesh mm, out BoundingBox bb)
        {
            bb = new BoundingBox(new Vector3(float.MaxValue, float.MaxValue, float.MaxValue), new Vector3(float.MinValue, float.MinValue, float.MinValue));
            bool first = true;
            Matrix x = Matrix.Identity;
            ModelBone mb = mm.ParentBone;
            while (mb != null)
            {
                x = x * mb.Transform;
                mb = mb.Parent;
            }
            
            foreach (ModelMeshPart mp in mm.MeshParts)
            {
                
                int n = mp.NumVertices;
                if (n > tempVecs3.Length)
                    tempVecs3 = new Vector3[n + 128];
                int l = mp.PrimitiveCount * 3;
                if (l + mp.StartIndex > tempUshorts.Length)
                    tempUshorts = new ushort[l + mp.StartIndex + 128];
                if (n == 0 || l == 0)
                    continue;
                
                //mm.IndexBuffer.GetData<ushort>(tempUshorts, mp.StartIndex, l);
                //mm.VertexBuffer.GetData<Vector3>(mp.StreamOffset, tempVecs3, mp.BaseVertex, n, mp.VertexStride);
                //CONV
                mp.IndexBuffer.GetData(tempUshorts, 0, l);
                //mp.VertexBuffer.GetData(tempVecs3);
                //mp.VertexBuffer.GetData(mp.VertexOffset,tempVecs3, mp.StartIndex, n, mp.VertexBuffer.VertexDeclaration.VertexStride);
                mp.VertexBuffer.GetData(mp.VertexOffset*mp.VertexBuffer.VertexDeclaration.VertexStride, 
                    tempVecs3, 0, n, mp.VertexBuffer.VertexDeclaration.VertexStride);
               
                if (first)
                {
                    bb.Min = Vector3.Transform(tempVecs3[tempUshorts[0]], x);
                    bb.Max = bb.Min;
                    first = false;
                }

                for (int i = 0; i != l; ++i)
                {
                    ushort us = tempUshorts[i];
                    Vector3 v = Vector3.Transform(tempVecs3[us], x);
                    
                    Vector3.Max(ref v, ref bb.Max, out bb.Max);
                    Vector3.Min(ref v, ref bb.Min, out bb.Min);
                }
            }
        }
    }
}
