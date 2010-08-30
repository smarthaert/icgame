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

        public void Draw(GraphicsDevice graphicsDevice, Effect effect, Matrix view, Matrix projection)
        {
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
