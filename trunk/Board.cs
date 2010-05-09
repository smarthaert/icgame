using System;
using System.Collections.Generic;
using System.Text;

namespace ICGame
{
    public class Board
    {
        private int[,] heightMap;
        private int[,] difficultyMap;
        private int[,] textureMap;
        private List<Microsoft.Xna.Framework.Graphics.Texture2D> textures;
    
        public Board(Microsoft.Xna.Framework.GraphicsDeviceManager graphicsDevice)
        {
            throw new System.NotImplementedException();
        }

        public void Draw()
        {
            throw new System.NotImplementedException();
        }

        public void GetTileInfo(int X, int Y)
        {
            throw new System.NotImplementedException();
        }

        public Microsoft.Xna.Framework.Vector3 GetNormal(int X, int Y, int objectWidth, int objectLength, float angle)
        {
            throw new System.NotImplementedException();
        }
    }
}
