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

        public void DrawSkyDome(GraphicsDevice graphicsDevice, Matrix view, Matrix projection, Vector3 cameraPosition, float? alpha = null)
        {
            Matrix[] modelTransforms = new Matrix[Board.SkyDomeModel.Bones.Count];
            Board.SkyDomeModel.CopyAbsoluteBoneTransformsTo(modelTransforms);
            Vector3 modifiedCameraPosition = cameraPosition;
            modifiedCameraPosition.Y = -45.0f;
            
            foreach (ModelMesh mesh in Board.SkyDomeModel.Meshes)
            {
                foreach (Effect currentEffect in mesh.Effects)
                {
                    Matrix worldMatrix = modelTransforms[mesh.ParentBone.Index] * Matrix.CreateScale(400) * Matrix.CreateTranslation(modifiedCameraPosition);
                    currentEffect.CurrentTechnique = currentEffect.Techniques["SkyDome"];
                    currentEffect.Parameters["xWorldViewProjection"].SetValue(worldMatrix * view * projection);
                    currentEffect.Parameters["xTexture"].SetValue(Board.CloudMap);
                    currentEffect.Parameters["xEnableLighting"].SetValue(false);
                    
                    if (alpha == null)
                    {
                        currentEffect.Parameters["xSetAlpha"].SetValue(false);
                    }
                    else
                    {
                        currentEffect.Parameters["xSetAlpha"].SetValue(true);
                        currentEffect.Parameters["xAlpha"].SetValue((float)alpha);
                    }
                }
                mesh.Draw();
            }
        }

        /// <summary>
        /// Rysuje plansze.
        /// </summary>
        /// <param name="graphicsDevice">GraphicsDevice</param>
        /// <param name="view">Macierz widoku/kamery</param>
        /// <param name="projection">Macierz przyciecia</param>
        /// <param name="cameraPosition">Pozycja kamery</param>
        /// <param name="clipPlane">Plan, wzdluz ktorego nastepuje przyciecie. Jezeli clipPlane == null, przyciecie nie nastepuje.
        ///                         Jezeli clipPlane != null nastepuje rowniez zmniejszenie w kazdym wymiarze liczby wierzcholkow o Board.ReduceFactor</param>
        public void Draw(GraphicsDevice graphicsDevice, Matrix view, Matrix projection, Vector3 cameraPosition, Vector4? clipPlane, float? alpha = null)
        {
            Effect effect = TechniqueProvider.GetEffect("MultiTextured");
            graphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            DrawSkyDome(graphicsDevice, view, projection, cameraPosition, alpha);
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

            if (alpha == null)
            {
                effect.Parameters["xSetAlpha"].SetValue(false);
            }
            else
            {
                effect.Parameters["xSetAlpha"].SetValue(true);
                effect.Parameters["xAlpha"].SetValue((float)alpha);
            }

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                int noVertices;
                int noTriangles;

                graphicsDevice.SetVertexBuffer(Board.TerrainVertexBuffer);
                if (clipPlane == null)
                {
                    graphicsDevice.Indices = Board.TerrainIndexBuffer;

                    noVertices = Board.TerrainVertexBuffer.VertexCount;
                    noTriangles = Board.TerrainIndexBuffer.IndexCount / 3;
                }
                else
                {
                    graphicsDevice.Indices = Board.ReducedTerrainIndexBuffer;

                    noVertices = Board.TerrainVertexBuffer.VertexCount/(Board.ReduceFactor * Board.ReduceFactor);
                    noTriangles = Board.ReducedTerrainIndexBuffer.IndexCount/3;
                }
                graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, noVertices, 0, noTriangles);
                
            }
        }
    }
}
