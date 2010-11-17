using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ICGame
{
    public class TerrainWater
    {
        private float waterHeight = 5.0f;
        public Board Board { get; set; }
        VertexBuffer waterVertexBuffer;
        private RenderTarget2D refractionRenderTarget;
        private Texture2D refractionMap;
        private RenderTarget2D reflectionRenderTarget;
        private Texture2D reflectionMap;
        public Camera Camera { get; set; }
        public Matrix ProjectionMatrix { get; set; }
        public Matrix ReflectionViewMatrix { get; set; }
        public Vector3 ReflCameraPosition { get; set; }

        public VertexBuffer WaterVertexBuffer
        {
            get { return waterVertexBuffer; }
        }

        public Texture2D ReflectionMap
        {
            get { return reflectionMap; }
            set { reflectionMap = value; }
        }

        public RenderTarget2D ReflectionRenderTarget
        {
            get { return reflectionRenderTarget; }
            set { reflectionRenderTarget = value; }
        }

        public Texture2D RefractionMap
        {
            get { return refractionMap; }
            set { refractionMap = value; }
        }

        public RenderTarget2D RefractionRenderTarget
        {
            get { return refractionRenderTarget; }
            set { refractionRenderTarget = value; }
        }

        public float WaterHeight
        {
            get { return waterHeight; }
        }

        public TerrainWater(GraphicsDevice device, Camera camera, Matrix projectionMatrix, Board board)
        {
            Board = board;
            Camera = camera;
            ProjectionMatrix = projectionMatrix;
            SetUpWaterVertices(device);

            refractionRenderTarget = new RenderTarget2D(device, device.PresentationParameters.BackBufferWidth,
                                                device.PresentationParameters.BackBufferHeight);

            reflectionRenderTarget = new RenderTarget2D(device, device.PresentationParameters.BackBufferWidth, device.PresentationParameters.BackBufferHeight);
        }

        public TerrainWaterDrawer GetDrawer()
        {
            return new TerrainWaterDrawer(this);
        }

        private void SetUpWaterVertices(GraphicsDevice device)
        {
            VertexPositionTexture[] waterVertices = new VertexPositionTexture[6];

            waterVertices[0] = new VertexPositionTexture(new Vector3(0, WaterHeight, 0), new Vector2(0, 1));
            waterVertices[2] = new VertexPositionTexture(new Vector3(Board.terrainWidth, WaterHeight, Board.terrainHeight), new Vector2(1, 0));
            waterVertices[1] = new VertexPositionTexture(new Vector3(0, WaterHeight, Board.terrainHeight), new Vector2(0, 0));

            waterVertices[3] = new VertexPositionTexture(new Vector3(0, WaterHeight, 0), new Vector2(0, 1));
            waterVertices[5] = new VertexPositionTexture(new Vector3(Board.terrainWidth, WaterHeight, 0), new Vector2(1, 1));
            waterVertices[4] = new VertexPositionTexture(new Vector3(Board.terrainWidth, WaterHeight, Board.terrainHeight), new Vector2(1, 0));

            waterVertexBuffer = new VertexBuffer(device, typeof(VertexPositionTexture), waterVertices.Length, BufferUsage.WriteOnly);
            waterVertexBuffer.SetData(waterVertices);

        }

    }
}
