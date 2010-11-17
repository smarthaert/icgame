using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;

namespace ICGame
{
    public class SkyDome
    {
        
        private GraphicsDevice device;
        private Effect effect;
        private RenderTarget2D cloudsRenderTarget;
        private Texture2D cloudStaticMap;
        private VertexPositionTexture[] fullScreenVertices;
        private VertexDeclaration fullScreenVertexDeclaration;

        public Texture2D CloudMap
        {
            get; set;
        }

        public SkyDome(GraphicsDevice device, Effect effect)
        {
            this.device = device;
            this.effect = effect;

            //cloudsRenderTarget = new RenderTarget2D(device, device.PresentationParameters.BackBufferWidth,  device.PresentationParameters.BackBufferHeight, 1, device.DisplayMode.Format);
            //CONV
            cloudsRenderTarget = new RenderTarget2D(device, device.PresentationParameters.BackBufferWidth, device.PresentationParameters.BackBufferHeight);
            fullScreenVertices = SetUpFullscreenVertices();
            //fullScreenVertexDeclaration = new VertexDeclaration(device, VertexPositionTexture.VertexElements);
            //CONV
            fullScreenVertexDeclaration = new VertexDeclaration(VertexPositionTexture.VertexDeclaration.GetVertexElements());
            cloudStaticMap = CreateStaticMap(32);
        }

        private Texture2D CreateStaticMap(int resolution)
        {
            Random rand = new Random();
            Color[] noisyColors = new Color[resolution * resolution];
            for (int x = 0; x < resolution; x++)
                for (int y = 0; y < resolution; y++)
                    noisyColors[x + y * resolution] = new Color(new Vector3((float)rand.Next(1000) / 1000.0f, 0, 0));

            //Texture2D noiseImage = new Texture2D(device, resolution, resolution, 1, TextureUsage.None, SurfaceFormat.Color);
            //CONV
            Texture2D noiseImage = new Texture2D(device, resolution, resolution, false, SurfaceFormat.Color);
            noiseImage.SetData(noisyColors);
            return noiseImage;
        }

        private VertexPositionTexture[] SetUpFullscreenVertices()
        {
            VertexPositionTexture[] vertices = new VertexPositionTexture[4];

            vertices[0] = new VertexPositionTexture(new Vector3(-1, 1, 0f), new Vector2(0, 1));
            vertices[1] = new VertexPositionTexture(new Vector3(1, 1, 0f), new Vector2(1, 1));
            vertices[2] = new VertexPositionTexture(new Vector3(-1, -1, 0f), new Vector2(0, 0));
            vertices[3] = new VertexPositionTexture(new Vector3(1, -1, 0f), new Vector2(1, 0));

            return vertices;
        }

         public void GeneratePerlinNoise(float time)
         {
             //device.SetRenderTarget(0, cloudsRenderTarget);
             //CONV
             device.RasterizerState = RasterizerState.CullCounterClockwise;
             device.SetRenderTarget(cloudsRenderTarget);
             device.Clear(ClearOptions.Target/* | ClearOptions.DepthBuffer*/, Color.Black, 1.0f, 0);
                                                //CONV
             effect.CurrentTechnique = effect.Techniques["PerlinNoise"];
             effect.Parameters["xTexture"].SetValue(cloudStaticMap);
             effect.Parameters["xOvercast"].SetValue(1.1f);
             effect.Parameters["xTime"].SetValue(time/1000.0f);
             //effect.Begin();
             //CONV
             foreach (EffectPass pass in effect.CurrentTechnique.Passes)
             {
                 pass.Apply();//.Begin();
 
                 //device.VertexDeclaration = fullScreenVertexDeclaration;
                 //CONV
                 
                 device.DrawUserPrimitives(PrimitiveType.TriangleStrip, fullScreenVertices, 0, 2);
 
                 //pass.End();
             }
             //effect.End();
 
             //device.SetRenderTarget(0, null);
             //CONV
             device.SetRenderTarget(null);
             //CloudMap = cloudsRenderTarget.GetTexture();
             //CONV
             CloudMap = cloudsRenderTarget;
             
             //CloudMap.SetData();
         }
     
    }
}
