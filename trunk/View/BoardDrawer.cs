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
            Vector3 modifiedCameraPosition = new Vector3(0,10,0);
            
          //  Matrix wMatrix = Matrix.CreateTranslation(0, -0.3f, 0) * Matrix.CreateScale(100) * Matrix.CreateTranslation();
            foreach (ModelMesh mesh in Board.SkyDomeModel.Meshes)
            {
                foreach (Effect currentEffect in mesh.Effects)
                {
                    Matrix worldMatrix = modelTransforms[mesh.ParentBone.Index]* Matrix.CreateTranslation(0.3f, -0.2f,0.3f) *Matrix.CreateScale(200)   ;
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
            effect.CurrentTechnique = effect.Techniques["Colored"];
            effect.Parameters["xWorld"].SetValue(Matrix.Identity);
            effect.Parameters["xView"].SetValue(view);
            effect.Parameters["xProjection"].SetValue(projection);
            effect.Begin();
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Begin();

                graphicsDevice.VertexDeclaration = new VertexDeclaration(graphicsDevice,VertexPositionColor.VertexElements);
                graphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList,Board.VertexPositionColor1,0,Board.VertexPositionColor1.GetLength(0),Board.Indices,0,Board.Indices.Length/3);

                pass.End();
            }
            
            effect.End();
        }
    }
}
