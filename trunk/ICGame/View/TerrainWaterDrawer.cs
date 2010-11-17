﻿using System;
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
        private TerrainWater terrainWater;

        public TerrainWaterDrawer(TerrainWater terrainWater)
        {
            this.terrainWater = terrainWater;
        }

        public Vector4 CreatePlane(float height, Vector3 planeNormalDirection, bool clipSide)
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
                terrainWater.RefractionMap.SaveAsJpeg(fileStream, terrainWater.RefractionMap.Width, refractionMap.Height);
                fileStream.Close();
            } */
        }

        public void UpdateReflectionViewMatrix(Camera camera)
        {
            terrainWater.ReflCameraPosition = new Vector3(camera.CameraPosition.X, -camera.CameraPosition.Y + terrainWater.WaterHeight * 2, 
                                                            camera.CameraPosition.Z);
            
            Vector3 reflTargetPos = camera.CameraLookAt;
            reflTargetPos.Y = -camera.CameraLookAt.Y + terrainWater.WaterHeight * 2;

            Vector3 cameraRight = Vector3.Transform(new Vector3(1, 0, 0), camera.CameraRotation);

            Vector3 invUpVector = Vector3.Cross(cameraRight, reflTargetPos - terrainWater.ReflCameraPosition);

            terrainWater.ReflectionViewMatrix = Matrix.CreateLookAt(terrainWater.ReflCameraPosition, reflTargetPos, invUpVector);
        }

        public void DrawReflectionMap(GraphicsDevice device, Effect effect, List<GameObject> gameObjects)
        {
            UpdateReflectionViewMatrix(terrainWater.Camera);

            Vector4 reflectionPlane = CreatePlane(terrainWater.WaterHeight, new Vector3(0, -1, 0), true);

            device.SetRenderTarget(terrainWater.ReflectionRenderTarget);
            device.RasterizerState = RasterizerState.CullCounterClockwise;
            device.DepthStencilState = DepthStencilState.Default;
            device.Clear(ClearOptions.Target, Color.Black, 1.0f, 0);
            terrainWater.Board.GetDrawer().Draw(device, effect, terrainWater.ReflectionViewMatrix,
                                                terrainWater.ProjectionMatrix, terrainWater.ReflCameraPosition, reflectionPlane);
            foreach (GameObject o in gameObjects)
            {
                o.GetDrawer().Draw(terrainWater.ProjectionMatrix, terrainWater.ReflectionViewMatrix, terrainWater.ReflCameraPosition, device, reflectionPlane);
            }
            //device.DepthStencilState = DepthStencilState.Default;
            device.SetRenderTarget(null);
            terrainWater.ReflectionMap = terrainWater.ReflectionRenderTarget;
            device.DepthStencilState = DepthStencilState.Default;
            /*using (FileStream fileStream = File.OpenWrite("reflectionmap.jpg"))
            {
                terrainWater.ReflectionMap.SaveAsJpeg(fileStream, terrainWater.ReflectionMap.Width, terrainWater.ReflectionMap.Height);
                fileStream.Close();
            }*/
        }


        public void DrawWater(GraphicsDevice device, Effect effect)
        {
            device.RasterizerState = RasterizerState.CullClockwise;
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

        public void Draw(GraphicsDevice device, Effect effect, List<GameObject> gameObjects)
        {
            DrawRefractionMap(device, effect);
            DrawReflectionMap(device, effect, gameObjects);
            DrawWater(device, effect);
        }
    }
}