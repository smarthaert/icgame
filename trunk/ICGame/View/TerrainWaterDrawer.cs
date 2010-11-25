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

        public void DrawRefractionMap(GraphicsDevice device, List<GameObject> gameObjects)
        {
            Vector4 refractionPlane = CreatePlane(terrainWater.WaterHeight + 6.5f, new Vector3(0, -1, 0), false);
            Vector4 vRefractionPlane = CreatePlane(terrainWater.WaterHeight, new Vector3(0, -1, 0), false);

            device.SetRenderTarget(terrainWater.RefractionRenderTarget);
            device.Clear(ClearOptions.Target, Color.White, 1.0f, 0);
            terrainWater.Board.GetDrawer().Draw(device, terrainWater.Camera.CameraMatrix, terrainWater.ProjectionMatrix, 
                                                terrainWater.Camera.CameraPosition, refractionPlane);

            foreach (GameObject o in gameObjects)
            {
                if(o is StaticObject || o is Building)
                {
                    o.GetDrawer().Draw(terrainWater.ProjectionMatrix, terrainWater.Camera.CameraMatrix, 
                        terrainWater.Camera.CameraPosition, device, vRefractionPlane);
                }
            }
            device.SetRenderTarget(null);
            terrainWater.RefractionMap = terrainWater.RefractionRenderTarget;
            /*using (FileStream fileStream = File.OpenWrite("refractionmap.jpg"))
            {
                terrainWater.RefractionMap.SaveAsJpeg(fileStream, terrainWater.RefractionMap.Width, terrainWater.RefractionMap.Height);
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

        public void DrawReflectionMap(GraphicsDevice device, List<GameObject> gameObjects)
        {
            UpdateReflectionViewMatrix(terrainWater.Camera);

            Vector4 reflectionPlane = CreatePlane(terrainWater.WaterHeight, new Vector3(0, -1, 0), true);

            device.SetRenderTarget(terrainWater.ReflectionRenderTarget);
            
            device.Clear(ClearOptions.Target, Color.Black, 1.0f, 0);
            terrainWater.Board.GetDrawer().Draw(device, terrainWater.ReflectionViewMatrix,
                                                terrainWater.ProjectionMatrix, terrainWater.ReflCameraPosition, reflectionPlane);
            foreach (GameObject o in gameObjects)
            {
                o.GetDrawer().Draw(terrainWater.ProjectionMatrix, terrainWater.ReflectionViewMatrix, terrainWater.ReflCameraPosition, device, reflectionPlane);
            }
            device.SetRenderTarget(null);
            terrainWater.ReflectionMap = terrainWater.ReflectionRenderTarget;
            /*using (FileStream fileStream = File.OpenWrite("reflectionmap.jpg"))
            {
                terrainWater.ReflectionMap.SaveAsJpeg(fileStream, terrainWater.ReflectionMap.Width, terrainWater.ReflectionMap.Height);
                fileStream.Close();
            }*/
        }


        public void DrawWater(GraphicsDevice device)
        {
            Effect effect = TechniqueProvider.GetEffect("Water");
            effect.CurrentTechnique = effect.Techniques["Water"];
            Matrix worldMatrix = Matrix.Identity;
            effect.Parameters["xWorld"].SetValue(worldMatrix);
            effect.Parameters["xWorldViewProjection"].SetValue(worldMatrix * terrainWater.Camera.CameraMatrix * terrainWater.ProjectionMatrix);
            effect.Parameters["xWorldReflectionViewProjection"].SetValue(worldMatrix * terrainWater.ReflectionViewMatrix * terrainWater.ProjectionMatrix);
            effect.Parameters["xReflectionMap"].SetValue(terrainWater.ReflectionMap);
            effect.Parameters["xRefractionMap"].SetValue(terrainWater.RefractionMap); 
            effect.Parameters["xWaterBumpMap"].SetValue(terrainWater.Waves);
            effect.Parameters["xWaveLength"].SetValue(terrainWater.WaveLength);
            effect.Parameters["xWaveHeight"].SetValue(terrainWater.WaveHeight);
            effect.Parameters["xCameraPosition"].SetValue(terrainWater.Camera.CameraPosition);
            effect.Parameters["xTime"].SetValue(terrainWater.Time);
            effect.Parameters["xWindForce"].SetValue(terrainWater.WindForce);
            effect.Parameters["xWindDirection"].SetValue(terrainWater.WindDirection);
            effect.Parameters["xLightDirection"].SetValue(terrainWater.Board.LightDirection);
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                device.SetVertexBuffer(terrainWater.WaterVertexBuffer);
                int noVertices = terrainWater.WaterVertexBuffer.VertexCount;
                device.DrawPrimitives(PrimitiveType.TriangleList, 0, noVertices / 3);

            }
        }

        public void Draw(GraphicsDevice device, List<GameObject> gameObjects, GameTime gameTime)
        {
            terrainWater.Time = (float)(gameTime.TotalGameTime.TotalMilliseconds) / 2000000.0f;
            //if(terrainWater.RefractionMap == null)
            //{
                DrawRefractionMap(device, gameObjects);
            //}
            DrawReflectionMap(device, gameObjects);
            DrawWater(device);
        }
    }
}
