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
        public const float waterHeight = 5.0f;
        public Board Board { get; set; }
        VertexBuffer waterVertexBuffer;
        public Camera Camera { get; set; }
        public Matrix ProjectionMatrix { get; set; }
        public Matrix ReflectionViewMatrix { get; set; }
        public Vector3 ReflCameraPosition { get; set; }
        public Texture2D Waves { get; set; }

        public Vector3 WindDirection { get; set; }

        public float WindForce { get; set; }

        public float Time { get; set; }

        public float WaveLength { get; set; }

        public float WaveHeight { get; set; }

        public VertexBuffer WaterVertexBuffer
        {
            get { return waterVertexBuffer; }
        }

        public Texture2D ReflectionMap { get; set; }

        public RenderTarget2D ReflectionRenderTarget { get; set; }

        public Texture2D RefractionMap { get; set; }

        public RenderTarget2D RefractionRenderTarget { get; set; }

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
            WindDirection = new Vector3(-1, 0, -0.2f);
            WindDirection.Normalize();
            WindForce = 20.0f;
            Time = 0;
            WaveLength = 0.1f;
            WaveHeight = 0.3f;

            RefractionRenderTarget = new RenderTarget2D(device, device.PresentationParameters.BackBufferWidth,
                                                device.PresentationParameters.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);

            ReflectionRenderTarget = new RenderTarget2D(device, device.PresentationParameters.BackBufferWidth, 
                                                device.PresentationParameters.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
        }

        public void LoadTextures(Texture2D waves)
        {
            Waves = waves;
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
