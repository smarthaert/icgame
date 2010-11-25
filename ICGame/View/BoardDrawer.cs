﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ICGame
{
    public class BoardDrawer
    {
        public BoardDrawer(Board board)
        {
            Board = board;
        }

        public Board Board
        {
            get;
            set;
        }

        public void DrawSkyDome(GraphicsDevice graphicsDevice, Matrix view, Matrix projection, Vector3 cameraPosition)
        {
            //graphicsDevice.RenderState.DepthBufferWriteEnable = false;
            //CONV
            //graphicsDevice.DepthStencilState = DepthStencilState.DepthRead;

            Matrix[] modelTransforms = new Matrix[Board.SkyDomeModel.Bones.Count];
            Board.SkyDomeModel.CopyAbsoluteBoneTransformsTo(modelTransforms);
            Vector3 modifiedCameraPosition = cameraPosition;
            modifiedCameraPosition.Y = -45.0f;

            //  Matrix wMatrix = Matrix.CreateTranslation(0, -0.3f, 0) * Matrix.CreateScale(100) * Matrix.CreateTranslation();

            foreach (ModelMesh mesh in Board.SkyDomeModel.Meshes)
            {
                foreach (Effect currentEffect in mesh.Effects)
                {
                    Matrix worldMatrix = modelTransforms[mesh.ParentBone.Index] * Matrix.CreateScale(400) * Matrix.CreateTranslation(modifiedCameraPosition);
                    currentEffect.CurrentTechnique = currentEffect.Techniques["SkyDome"];
                    currentEffect.Parameters["xWorld"].SetValue(worldMatrix);
                    currentEffect.Parameters["xView"].SetValue(view);
                    currentEffect.Parameters["xProjection"].SetValue(projection);
                    currentEffect.Parameters["xTexture"].SetValue(Board.CloudMap);
                    currentEffect.Parameters["xEnableLighting"].SetValue(false);
                }
                mesh.Draw();
            }
            //graphicsDevice.RenderState.DepthBufferWriteEnable = true;
            //CONV
            //graphicsDevice.DepthStencilState = DepthStencilState.Default;
        }

        public void Draw(GraphicsDevice graphicsDevice, Matrix view, Matrix projection, Vector3 cameraPosition, Vector4? clipPlane)
        {
            Effect effect = TechniqueProvider.GetEffect("MultiTextured");
            graphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            DrawSkyDome(graphicsDevice, view, projection, cameraPosition);
            graphicsDevice.RasterizerState = RasterizerState.CullClockwise;

            effect.CurrentTechnique = effect.Techniques["MultiTextured"];

            if(clipPlane == null)
            {
                effect.Parameters["xClipPlanes"].SetValue(false);
            }
            else
            {
                effect.Parameters["xClipPlanes"].SetValue(true);
                effect.Parameters["xClipPlane0"].SetValue((Vector4)clipPlane);
            }

            effect.Parameters["xTexture0"].SetValue(Board.Textures["sand"]);
            effect.Parameters["xTexture1"].SetValue(Board.Textures["grass"]);
            effect.Parameters["xTexture2"].SetValue(Board.Textures["rock"]);
            effect.Parameters["xTexture3"].SetValue(Board.Textures["snow"]);

            effect.Parameters["xWorld"].SetValue(Matrix.Identity);
            effect.Parameters["xView"].SetValue(view);
            effect.Parameters["xWorldViewProjection"].SetValue(view * projection);
            effect.Parameters["xEnableLighting"].SetValue(true);
            effect.Parameters["xAmbient"].SetValue(0.4f);
            effect.Parameters["xLightDirection"].SetValue(Board.LightDirection);
            effect.Parameters["xCameraPosition"].SetValue(cameraPosition);
            
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                graphicsDevice.SetVertexBuffer(Board.TerrainVertexBuffer);
                graphicsDevice.Indices = Board.TerrainIndexBuffer;

                int noVertices = Board.TerrainVertexBuffer.VertexCount;
                int noTriangles = Board.TerrainIndexBuffer.IndexCount / 3;
                graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, noVertices, 0, noTriangles);
                
            }
        }
    }
}
