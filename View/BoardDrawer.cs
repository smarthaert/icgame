using System;
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
            get; set;
        }

        public void DrawSkyDome(GraphicsDevice graphicsDevice, Effect effect, Matrix view, Matrix projection, Vector3 cameraPosition)
        {
            graphicsDevice.RenderState.DepthBufferWriteEnable = false;

            Matrix[] modelTransforms = new Matrix[Board.SkyDomeModel.Bones.Count];
            Board.SkyDomeModel.CopyAbsoluteBoneTransformsTo(modelTransforms);
            Vector3 modifiedCameraPosition = cameraPosition;
            modifiedCameraPosition.Y = -45.0f;
            
          //  Matrix wMatrix = Matrix.CreateTranslation(0, -0.3f, 0) * Matrix.CreateScale(100) * Matrix.CreateTranslation();
            foreach (ModelMesh mesh in Board.SkyDomeModel.Meshes)
            {
                foreach (Effect currentEffect in mesh.Effects)
                {
                    Matrix worldMatrix = modelTransforms[mesh.ParentBone.Index]*Matrix.CreateScale(400) * Matrix.CreateTranslation(modifiedCameraPosition)  ;
                    currentEffect.CurrentTechnique = currentEffect.Techniques["SkyDome"];
                    currentEffect.Parameters["xWorld"].SetValue(worldMatrix);
                    currentEffect.Parameters["xView"].SetValue(view);
                    currentEffect.Parameters["xProjection"].SetValue(projection);
                    currentEffect.Parameters["xTexture"].SetValue(Board.CloudMap);
                    currentEffect.Parameters["xEnableLighting"].SetValue(false);
                }
                mesh.Draw();
            }
            graphicsDevice.RenderState.DepthBufferWriteEnable = true;
        }

        public void Draw(GraphicsDevice graphicsDevice, Effect effect, Matrix view, Matrix projection, Vector3 cameraPosition)
        {
           DrawSkyDome(graphicsDevice,effect,view,projection,cameraPosition);
            effect.CurrentTechnique = effect.Techniques["MultiTextured"];
            effect.Parameters["xTexture0"].SetValue(Board.Textures["sand"]);
            effect.Parameters["xTexture1"].SetValue(Board.Textures["grass"]);
            effect.Parameters["xTexture2"].SetValue(Board.Textures["rock"]);
            effect.Parameters["xTexture3"].SetValue(Board.Textures["snow"]);
 
            effect.Parameters["xWorld"].SetValue(Matrix.Identity);
            effect.Parameters["xView"].SetValue(view);
            effect.Parameters["xProjection"].SetValue(projection);
            effect.Parameters["xEnableLighting"].SetValue(true);
            effect.Parameters["xAmbient"].SetValue(0.4f);
            effect.Parameters["xLightDirection"].SetValue(new Vector3(-0.5f, -1, -0.5f));

            effect.Begin();
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Begin();

                graphicsDevice.VertexDeclaration = new VertexDeclaration(graphicsDevice,VertexMultitextured.VertexElements);

                graphicsDevice.Vertices[0].SetSource(Board.TerrainVertexBuffer, 0, VertexMultitextured.SizeInBytes);
                graphicsDevice.Indices = Board.TerrainIndexBuffer;


                int noVertices = Board.TerrainVertexBuffer.SizeInBytes / VertexMultitextured.SizeInBytes;
                int noTriangles = Board.TerrainIndexBuffer.SizeInBytes / sizeof(int) / 3;
                graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, noVertices, 0, noTriangles);
                //graphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList,Board.VertexPositionColor1,0,Board.VertexPositionColor1.GetLength(0),Board.Indices,0,Board.Indices.Length/3);

                pass.End();
            }
            
            effect.End();
        }
    }
}
