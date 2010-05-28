using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;

namespace ICGame
{
    public class Board
    {
        private int terrainWidth;
        private int terrainHeight;
        private float[,] heightMap;
        private int[,] difficultyMap;
        private int[,] textureMap;
        private List<Microsoft.Xna.Framework.Graphics.Texture2D> textures;
        private const int tileSize = 32;
        private VertexPositionColor[] vertexPositionColor;
        private int[] indices;
    
        public Board()
        {

        }

        public VertexPositionColor[] VertexPositionColor1
        {
            get { return vertexPositionColor; }
        }
        public int[] Indices
        {
            get { return indices; }
        }

        public void CreateTerrain()
        {
             ArrayList vertexArray = new ArrayList();
            for (int i = 0; i < terrainWidth; i++)
            {
                for (int j = 0; j < terrainHeight; j++)
                {
                    vertexArray.Add(new VertexPositionColor(new Vector3(i, heightMap[i, j], j),
                                                            new Color(0, 0, (byte)(heightMap[i, j]*10+20), 1)));
                }
            }
            vertexPositionColor = (VertexPositionColor[]) vertexArray.ToArray(typeof (VertexPositionColor));
            SetUpIndices();
        }

        private void SetUpIndices()
        {
            indices = new int[(terrainWidth - 1) * (terrainHeight - 1) * 6];
            int counter = 0;
            for (int y = 0; y < terrainHeight - 1; y++)
            {
                for (int x = 0; x < terrainWidth - 1; x++)
                {
                    int lowerLeft = x + y * terrainWidth;
                    int lowerRight = (x + 1) + y * terrainWidth;
                    int topLeft = x + (y + 1) * terrainWidth;
                    int topRight = (x + 1) + (y + 1) * terrainWidth;

                    indices[counter++] = topLeft;
                    indices[counter++] = lowerRight;
                    indices[counter++] = lowerLeft;

                    indices[counter++] = topLeft;
                    indices[counter++] = topRight;
                    indices[counter++] = lowerRight;
                }
            }
        }

        public BoardDrawer GetDrawer()
        {
            return new BoardDrawer(this);
        }

        public void GetTileInfo(int X, int Y)
        {
            
        }

        public float GetHeight(int x, int y)
        {
            //TODO: Interpolacja
            if (x < 0 || y < 0 || x >= terrainWidth || y >= terrainHeight)
                return 0;
            return heightMap[x, y];
        }

        public Microsoft.Xna.Framework.Vector3 GetNormal(int X, int Y, int objectWidth, int objectLength, float angle)
        {
            return new Vector3();
        }

        //TEMP
        public void LoadHeightData(Texture2D heightMap)
        {
            terrainWidth = heightMap.Width;
            terrainHeight = heightMap.Height;

            Color[] heightMapColors = new Color[terrainWidth * terrainHeight];
            heightMap.GetData(heightMapColors);

            this.heightMap = new float[terrainWidth, terrainHeight];
            for (int x = 0; x < terrainWidth; x++)
                for (int y = 0; y < terrainHeight; y++)
                    this.heightMap[x, y] = heightMapColors[x + y * terrainWidth].R / 5.0f;
            CreateTerrain();
        }
        //\TEMP
    }
}
