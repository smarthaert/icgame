﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;

namespace ICGame
{
    public class Board
    {
        

        public int terrainWidth;
        public int terrainHeight;
        private float[,] heightMap;
        private int[,] difficultyMap;
        private int[,] textureMap;
        private List<Microsoft.Xna.Framework.Graphics.Texture2D> textures;
        private const int tileSize = 32;
        private VertexPositionColor[] vertexPositionColor;
        private int[] indices;

        public SkyDome SkyDome
        {
            get; set;
        }

        public Game MainGame
        {
            get; set;
        }
    
        public Board(Game game)
        {
            MainGame = game;
        }

        public Model SkyDomeModel
        {
            get; set;
        }

        public Texture2D CloudMap
        {
            get
            {
                return SkyDome.CloudMap;
            }
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
                                                            new Color(0, 0, (byte)(heightMap[i, j]*10+20),255)));
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

        public float GetHeight(float x, float y)
        {
            if (x < 0 || y < 0 || x >= terrainWidth || y >= terrainHeight)
                return 0;
            int lx, ly, hx, hy;
            if(x==0)
            {
                lx = 0;
                hx = 0;
            }
            else
            {
                lx = Convert.ToInt32(Math.Floor(Convert.ToDouble(x)));
                if(lx+1 < terrainWidth)
                {
                    hx = lx + 1;
                }
                else
                {
                    hx = lx;
                }
            }
            if (y == 0)
            {
                ly = 0;
                hy = 0;
            }
            else
            {
                ly = Convert.ToInt32(Math.Floor(Convert.ToDouble(y)));
                if (ly + 1 < terrainWidth)
                {
                    hy = ly + 1;
                }
                else
                {
                    hy = ly;
                }
            }
            x = x - lx;
            y = y - ly;
            return heightMap[lx, ly] + (heightMap[hx, ly] - heightMap[lx, ly]) * x + (heightMap[lx, hy] - heightMap[lx, ly]) * y + (heightMap[lx, ly] - heightMap[hx,ly] - heightMap[lx,hy] + heightMap[hx,hy])*x*y;
        }

        public Microsoft.Xna.Framework.Vector3 GetNormal(float X, float Y, float objectWidth, float objectLength, float angle)
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

        public void PrepareSkyDome( Model skyDome, Effect effect, GraphicsDevice device)
        {
          //  CloudMap = cloudMap;
            SkyDomeModel = skyDome;
            SkyDomeModel.Meshes[0].MeshParts[0].Effect = effect.Clone(device);
            SkyDome = new SkyDome(device,effect);
        }



        private Ray GetPointerRay(Vector2 pointerPosition)
        {
            Vector3 nearScreenPoint = new Vector3(pointerPosition.X, pointerPosition.Y, 0);
            Vector3 farScreenPoint = new Vector3(pointerPosition.X, pointerPosition.Y, 1);

            Vector3 near3DWorldPoint = MainGame.GraphicsDevice.Viewport.Unproject(nearScreenPoint, MainGame.Display.Projection, MainGame.Display.Camera.CameraMatrix, Matrix.Identity);
            Vector3 far3DWorldPoint = MainGame.GraphicsDevice.Viewport.Unproject(farScreenPoint, MainGame.Display.Projection, MainGame.Display.Camera.CameraMatrix, Matrix.Identity);

            Vector3 pointerRayDirection = far3DWorldPoint - near3DWorldPoint;
            Ray pointerRay = new Ray(near3DWorldPoint, pointerRayDirection);

            return pointerRay;
        }

        public Vector3 GetPosition(int posX, int posY)
        {
            return BinarySearch(LinearSearch(ClipRay(GetPointerRay(new Vector2(posX, posY)), 30, 0)));
        }

        private Ray ClipRay(Ray ray, float highest, float lowest)
        {
            Vector3 oldStartPoint = ray.Position;

            float factorH = -(oldStartPoint.Y - highest) / ray.Direction.Y;
            Vector3 pointA = oldStartPoint + factorH * ray.Direction;

            float factorL = -(oldStartPoint.Y - lowest) / ray.Direction.Y;
            Vector3 pointB = oldStartPoint + factorL * ray.Direction;

            Vector3 newDirection = pointB - pointA;
            return new Ray(pointA, newDirection);
        }

        private Ray LinearSearch(Ray ray)
        {
            ray.Direction /= 50.0f;

            Vector3 nextPoint = ray.Position + ray.Direction;
            float heightAtNextPoint = GetHeight(nextPoint.X, -nextPoint.Z);//terrain.GetExactHeightAt(nextPoint.X, -nextPoint.Z);
            while (heightAtNextPoint < nextPoint.Y)
            {
                ray.Position = nextPoint;

                nextPoint = ray.Position + ray.Direction;
                heightAtNextPoint = GetHeight(nextPoint.X, -nextPoint.Z); //terrain.GetExactHeightAt(nextPoint.X, -nextPoint.Z);
            }
            return ray;
        }

        private Vector3 BinarySearch(Ray ray)
        {
            float accuracy = 0.01f;
            float heightAtStartingPoint = GetHeight(ray.Position.X, -ray.Position.Z);//terrain.GetExactHeightAt(ray.Position.X, -ray.Position.Z);
            float currentError = ray.Position.Y - heightAtStartingPoint;
            int counter = 0;
            while (currentError > accuracy)
            {
                ray.Direction /= 2.0f;
                Vector3 nextPoint = ray.Position + ray.Direction;
                float heightAtNextPoint = GetHeight(nextPoint.X, -nextPoint.Z); //terrain.GetExactHeightAt(nextPoint.X, -nextPoint.Z);
                if (nextPoint.Y > heightAtNextPoint)
                {
                    ray.Position = nextPoint;
                    currentError = ray.Position.Y - heightAtNextPoint;
                }
                if (counter++ == 1000) break;
            }
            return ray.Position;
        }
    }
}
