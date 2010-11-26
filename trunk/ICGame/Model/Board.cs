using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;
using Microsoft.Xna.Framework.Input;

namespace ICGame
{
    public class Board
    {
        

        public int terrainWidth;
        public int terrainHeight;
        private float[,] heightMap;
        private int[,] difficultyMap;
        private int[,] textureMap;
        private Dictionary<string,Texture2D> textures;
        private const int tileSize = 32;
        private VertexMultitextured[] vertexPositionColor;
        private int[] indices;
        private int[] reducedIndices;
        /// <summary>
        /// Stopien o jaki zmniejszana jest liczba wierzchołkow w kazdym wymiarze podczes rysowania odbicia terenu/terenu znajdujacego sie pod woda
        /// </summary>
        public const int ReduceFactor = 4;

        public VertexBuffer TerrainVertexBuffer
        { 
            get; set;
        }

        public IndexBuffer TerrainIndexBuffer
        {
            get; set;
        }

        public IndexBuffer ReducedTerrainIndexBuffer
        {
            get;
            set;
        }

        public Dictionary<string, Texture2D> Textures
        {
            get
            {
                return textures;
            }
            set
            {
                textures = value;
            }
        }

        public SkyDome SkyDome
        {
            get; set;
        }

        public TerrainWater TerrainWater
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

        public VertexMultitextured[] VertexPositionColor1
        {
            get { return vertexPositionColor; }
        }

        public Vector3 LightDirection { get; set; }

        public void CreateTerrain()
        {
            VertexMultitextured[] terrainVertices = new VertexMultitextured[terrainWidth * terrainHeight];
            for (int x = 0; x < terrainWidth; x++)
            {
                for (int y = 0; y < terrainHeight; y++)
                {
                    float height = heightMap[x, y];// GetHeight(x, y); //* 10 +20;
                    terrainVertices[x + y * terrainWidth].Position = new Vector3(x, heightMap[x, y], y);
                    terrainVertices[x + y * terrainWidth].TextureCoordinate.X = (float)x / 30.0f;
                    terrainVertices[x + y * terrainWidth].TextureCoordinate.Y = (float)y / 30.0f;

                    terrainVertices[x + y * terrainWidth].TexWeights.X = MathHelper.Clamp(1.0f - Math.Abs(height - 0) / 8.0f, 0, 1);
                    terrainVertices[x + y * terrainWidth].TexWeights.Y = MathHelper.Clamp(1.0f - Math.Abs(height - 12) / 6.0f, 0, 1);
                    terrainVertices[x + y * terrainWidth].TexWeights.Z = MathHelper.Clamp(1.0f - Math.Abs(height - 20) / 6.0f, 0, 1);
                    terrainVertices[x + y * terrainWidth].TexWeights.W = MathHelper.Clamp(1.0f - Math.Abs(height - 30) / 6.0f, 0, 1);

                    float total = terrainVertices[x + y * terrainWidth].TexWeights.X;
                    total += terrainVertices[x + y * terrainWidth].TexWeights.Y;
                    total += terrainVertices[x + y * terrainWidth].TexWeights.Z;
                    total += terrainVertices[x + y * terrainWidth].TexWeights.W;

                    terrainVertices[x + y * terrainWidth].TexWeights.X /= total;
                    terrainVertices[x + y * terrainWidth].TexWeights.Y /= total;
                    terrainVertices[x + y * terrainWidth].TexWeights.Z /= total;
                    terrainVertices[x + y * terrainWidth].TexWeights.W /= total;

                   /* vertexArray.Add(new VertexMultitextured(new Vector3(i, heightMap[i, j], j),
                                                            new Color(0, 0, (byte)(heightMap[i, j]*10+20),255),));*/
                }
            }
            SetUpIndices();
            CalculateNormals(ref terrainVertices);
            vertexPositionColor = terrainVertices;
            PrepareBuffers();

            //LightDirection = new Vector3(0.5f, -0.7f, -1.0f);
            LightDirection = new Vector3(1.0f, -1.0f, -1.0f);
            LightDirection.Normalize();
        }

        private void CalculateNormals(ref VertexMultitextured[] terrainVertices)
        {
            for (int i = 0; i < terrainVertices.Length; i++)
                terrainVertices[i].Normal = new Vector3(0, 0, 0);

            for (int i = 0; i < indices.Length / 3; i++)
            {
                int index1 = indices[i * 3];
                int index2 = indices[i * 3 + 1];
                int index3 = indices[i * 3 + 2];

                Vector3 side1 = terrainVertices[index1].Position - terrainVertices[index3].Position;
                Vector3 side2 = terrainVertices[index1].Position - terrainVertices[index2].Position;
                Vector3 normal = Vector3.Cross(side1, side2);

                terrainVertices[index1].Normal += normal;
                terrainVertices[index2].Normal += normal;
                terrainVertices[index3].Normal += normal;
            }

            for (int i = 0; i < terrainVertices.Length; i++)
                terrainVertices[i].Normal.Normalize();

        }

        private void SetUpIndices()
        {
            indices = new int[(terrainWidth - 1) * (terrainHeight - 1) * 6];
            reducedIndices = new int[(terrainWidth/ReduceFactor-1) * (terrainHeight/ReduceFactor-1) * 6];
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
            counter = 0;
            for (int y = 0; y < terrainHeight - ReduceFactor; y+=ReduceFactor)
            {
                for (int x = 0; x < terrainWidth - ReduceFactor; x+=ReduceFactor)
                {
                    int lowerLeft = x + y * terrainWidth;
                    int lowerRight = (x + ReduceFactor) + y * terrainWidth;
                    int topLeft = x + (y + ReduceFactor) * terrainWidth;
                    int topRight = (x + ReduceFactor) + (y + ReduceFactor) * terrainWidth;

                    reducedIndices[counter++] = topLeft;
                    reducedIndices[counter++] = lowerRight;
                    reducedIndices[counter++] = lowerLeft;

                    reducedIndices[counter++] = topLeft;
                    reducedIndices[counter++] = topRight;
                    reducedIndices[counter++] = lowerRight;
                }
            }
        }

        public BoardDrawer GetDrawer()
        {
            return new BoardDrawer(this);
        }

        public int[,] GetDifficultyMap()
        {
            return difficultyMap;
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
            return (heightMap[lx, ly] + (heightMap[hx, ly] - heightMap[lx, ly]) * x + (heightMap[lx, hy] - heightMap[lx, ly]) * y + (heightMap[lx, ly] - heightMap[hx,ly] - heightMap[lx,hy] + heightMap[hx,hy])*x*y);
        }

        public Vector3 GetNormal(float X, float Y, float objectWidth, float objectLength, float angle)
        {
            return new Vector3();
        }

        //TEMP
        public void LoadHeightData(Texture2D heightMap)
        {
            terrainWidth = heightMap.Width;
            terrainHeight = heightMap.Height;
            float minimumHeight = 0;
            float maximumHeight = 0;
            Color[] heightMapColors = new Color[terrainWidth * terrainHeight];
            heightMap.GetData(heightMapColors);

            this.heightMap = new float[terrainWidth, terrainHeight];
            for (int x = 0; x < terrainWidth; x++)
                for (int y = 0; y < terrainHeight; y++)
                {
                    this.heightMap[x, y] = heightMapColors[x + y*terrainWidth].R;
                    minimumHeight = this.heightMap[x, y] < minimumHeight ? this.heightMap[x, y] : minimumHeight;
                    maximumHeight = this.heightMap[x, y] > maximumHeight ? this.heightMap[x, y] : maximumHeight;
                }

            for (int x = 0; x < terrainWidth; x++)
                for (int y = 0; y < terrainHeight; y++)
                     this.heightMap[x, y] = (this.heightMap[x, y] - minimumHeight) / (maximumHeight - minimumHeight) * 30.0f;
            //TEMP TEMP TEMP!
            difficultyMap = new int[terrainWidth,terrainHeight];
            for (int x = 0; x < terrainWidth; x++)
                for (int y = 0; y < terrainHeight; y++)
                    difficultyMap[x, y] = 1;
            CreateTerrain();
        }

        public void LoadTextures(Texture2D grass, Texture2D snow, Texture2D sand, Texture2D rock)
        {
            textures = new Dictionary<string, Texture2D>();
            textures.Add("grass", grass);
            textures.Add("snow", snow);
            textures.Add("sand", sand);
            textures.Add("rock", rock);
        }
        //\TEMP

        public void PrepareSkyDome( Model skyDome, GraphicsDevice device)
        {
          //  CloudMap = cloudMap;
            SkyDomeModel = skyDome;
            //SkyDomeModel.Meshes[0].MeshParts[0].Effect = effect.Clone(device);
            //CONV
            SkyDomeModel.Meshes[0].MeshParts[0].Effect = TechniqueProvider.GetEffect("effects").Clone();
            SkyDome = new SkyDome(device);
        }

        public void PrepareTerrainWater(Camera camera, Matrix projectionMatrix)
        {
            TerrainWater = new TerrainWater(MainGame.GraphicsDevice, camera, projectionMatrix, this);
        }

        public void PrepareBuffers()
        {
            //TerrainVertexBuffer = new VertexBuffer(MainGame.GraphicsDevice, vertexPositionColor.Length * VertexMultitextured.SizeInBytes, BufferUsage.WriteOnly);
            //CONV
            TerrainVertexBuffer = new VertexBuffer(MainGame.GraphicsDevice, typeof(VertexMultitextured),
                vertexPositionColor.Length, BufferUsage.WriteOnly);
            TerrainVertexBuffer.SetData(vertexPositionColor);

            TerrainIndexBuffer = new IndexBuffer(MainGame.GraphicsDevice, typeof(int), indices.Length, BufferUsage.WriteOnly);
            TerrainIndexBuffer.SetData(indices);

            ReducedTerrainIndexBuffer = new IndexBuffer(MainGame.GraphicsDevice, typeof(int), reducedIndices.Length, BufferUsage.WriteOnly);
            ReducedTerrainIndexBuffer.SetData(reducedIndices);
        }

        

        private Ray GetPointerRay(Vector2 pointerPosition)
        {
            Vector3 nearScreenPoint = new Vector3(pointerPosition.X, pointerPosition.Y, 0);
            Vector3 farScreenPoint = new Vector3(pointerPosition.X, pointerPosition.Y, 1);

            Vector3 near3DWorldPoint = MainGame.GraphicsDevice.Viewport.Unproject(nearScreenPoint, MainGame.Display.Projection, MainGame.Display.Camera.CameraMatrix, Matrix.Identity);
            Vector3 far3DWorldPoint = MainGame.GraphicsDevice.Viewport.Unproject(farScreenPoint, MainGame.Display.Projection, MainGame.Display.Camera.CameraMatrix, Matrix.Identity);

            Vector3 pointerRayDirection = far3DWorldPoint - near3DWorldPoint;
            //pointerRayDirection.Normalize();
            Ray pointerRay = new Ray(near3DWorldPoint, pointerRayDirection);
            return pointerRay;
        }

        public Vector3 GetPosition(int posX, int posY)
        {
            return BinarySearch(LinearSearch(ClipRay(GetPointerRay(new Vector2(posX, posY)), 30, -30)));
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
            float heightAtNextPoint = GetHeight(nextPoint.X, nextPoint.Z);//terrain.GetExactHeightAt(nextPoint.X, -nextPoint.Z);
            while (heightAtNextPoint < nextPoint.Y)
            {
                ray.Position = nextPoint;

                nextPoint = ray.Position + ray.Direction;
                heightAtNextPoint = GetHeight(nextPoint.X, nextPoint.Z); //terrain.GetExactHeightAt(nextPoint.X, -nextPoint.Z);
            }
            return ray;
        }

        private Vector3 BinarySearch(Ray ray)
        {
            float accuracy = 0.01f;
            float heightAtStartingPoint = GetHeight(ray.Position.X, ray.Position.Z);//terrain.GetExactHeightAt(ray.Position.X, -ray.Position.Z);
            float currentError = ray.Position.Y - heightAtStartingPoint;
            int counter = 0;
            while (currentError > accuracy)
            {
                ray.Direction /= 2.0f;
                Vector3 nextPoint = ray.Position + ray.Direction;
                float heightAtNextPoint = GetHeight(nextPoint.X, nextPoint.Z); //terrain.GetExactHeightAt(nextPoint.X, -nextPoint.Z);
                if (nextPoint.Y > heightAtNextPoint)
                {
                    ray.Position = nextPoint;
                    currentError = ray.Position.Y - heightAtNextPoint;
                }
                if (counter++ == 1000) 
                    break;
            }
            return ray.Position;
        }
    }


    public struct VertexMultitextured : IVertexType
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector4 TextureCoordinate;
        public Vector4 TexWeights;

        public static int SizeInBytes = (3 + 3 + 4 + 4) * sizeof(float);
        public static VertexElement[] VertexElements = new VertexElement[]
     {
         new VertexElement( 0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0 ),
         new VertexElement(/* 0, */sizeof(float) * 3, VertexElementFormat.Vector3/*, VertexElementMethod.Default*/, VertexElementUsage.Normal, 0 ),
         new VertexElement(/* 0, */sizeof(float) * 6, VertexElementFormat.Vector4/*, VertexElementMethod.Default*/, VertexElementUsage.TextureCoordinate, 0 ),
         new VertexElement(/* 0, */sizeof(float) * 10, VertexElementFormat.Vector4/*, VertexElementMethod.Default*/, VertexElementUsage.TextureCoordinate, 1 ),
     };
        public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration(VertexElements);

        VertexDeclaration IVertexType.VertexDeclaration { get { return VertexDeclaration; } }

    }
}
