using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;

namespace ICGame
{
    public class TerrainWaterDrawer
    {
        private static Vector3 reflCameraPosition;

        private TerrainWater terrainWater;

        public TerrainWaterDrawer(TerrainWater terrainWater)
        {
            this.terrainWater = terrainWater;
        }

        private Vector4 CreatePlane(float height, Vector3 planeNormalDirection, bool clipSide)
        {
            planeNormalDirection.Normalize();
            Vector4 planeCoeffs = new Vector4(planeNormalDirection, height);
            if (clipSide)
                planeCoeffs *= -1;

            return planeCoeffs;
        }

        public void DrawRefractionMap(GraphicsDevice device, Effect effect)
        {
            Vector4 refractionPlane = CreatePlane(terrainWater.WaterHeight, new Vector3(0, -1, 0), false);

            device.SetRenderTarget(terrainWater.RefractionRenderTarget);
            device.Clear(ClearOptions.Target, Color.Black, 1.0f, 0);
            terrainWater.Board.GetDrawer().Draw(device, effect, terrainWater.Camera.CameraMatrix, terrainWater.ProjectionMatrix, 
                                                terrainWater.Camera.CameraPosition, refractionPlane);

            device.SetRenderTarget(null);
            terrainWater.RefractionMap = terrainWater.RefractionRenderTarget;
            /*using (FileStream fileStream = File.OpenWrite("refractionmap.jpg"))
            {
                refractionMap.SaveAsJpeg(fileStream, refractionMap.Width, refractionMap.Height);
                fileStream.Close();
            } */
        }

        public void UpdateReflectionViewMatrix(Camera camera)
        {
            reflCameraPosition = camera.CameraPosition;
            reflCameraPosition.Y = -camera.CameraPosition.Y + terrainWater.WaterHeight * 2;

            Vector3 reflTargetPos = camera.CameraLookAt;
            reflTargetPos.Y = -camera.CameraLookAt.Y + terrainWater.WaterHeight * 2;

            Vector3 cameraRight = Vector3.Transform(new Vector3(1, 0, 0), camera.CameraRotation);

            Vector3 invUpVector = Vector3.Cross(cameraRight, reflTargetPos - reflCameraPosition);

            terrainWater.ReflectionViewMatrix = Matrix.CreateLookAt(reflCameraPosition, reflTargetPos, invUpVector);
        }

        public void DrawReflectionMap(GraphicsDevice device, Effect effect)
        {
            UpdateReflectionViewMatrix(terrainWater.Camera);

            Vector4 reflectionPlane = CreatePlane(terrainWater.WaterHeight, new Vector3(0, -1, 0), true);

            device.RasterizerState = RasterizerState.CullClockwise;
            device.SetRenderTarget(terrainWater.ReflectionRenderTarget);
            device.Clear(ClearOptions.Target, Color.Black, 1.0f, 0);
            terrainWater.Board.GetDrawer().Draw(device, effect, terrainWater.ReflectionViewMatrix, 
                                                terrainWater.ProjectionMatrix, reflCameraPosition, reflectionPlane);

            device.SetRenderTarget(null);
            terrainWater.ReflectionMap = terrainWater.ReflectionRenderTarget;
            /*using (FileStream fileStream = File.OpenWrite("reflectionmap.jpg"))
            {
                terrainWater.ReflectionMap.SaveAsJpeg(fileStream, terrainWater.ReflectionMap.Width, terrainWater.ReflectionMap.Height);
                fileStream.Close();
            }*/
        }


        public void DrawWater(GraphicsDevice device, Effect effect)
        {
            effect.CurrentTechnique = effect.Techniques["Water"];
            Matrix worldMatrix = Matrix.Identity;
            effect.Parameters["xWorld"].SetValue(worldMatrix);
            effect.Parameters["xView"].SetValue(terrainWater.Camera.CameraMatrix);
            effect.Parameters["xReflectionView"].SetValue(terrainWater.ReflectionViewMatrix);
            effect.Parameters["xProjection"].SetValue(terrainWater.ProjectionMatrix);
            effect.Parameters["xReflectionMap"].SetValue(terrainWater.ReflectionMap);
            effect.Parameters["xRefractionMap"].SetValue(terrainWater.RefractionMap);
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                device.SetVertexBuffer(terrainWater.WaterVertexBuffer);
                int noVertices = terrainWater.WaterVertexBuffer.VertexCount;
                device.DrawPrimitives(PrimitiveType.TriangleList, 0, noVertices / 3);

            }
        }

        public void Draw(GraphicsDevice device, Effect effect)
        {
            DrawRefractionMap(device, effect);
            DrawReflectionMap(device, effect);
            DrawWater(device, effect);
        }
    }
}
