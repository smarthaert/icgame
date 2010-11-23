using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ICGame.ParticleSystem
{
    public class ParticleDrawer
    {
        private Effect effect;
        private ParticleEmitter emmiter;

        public ParticleDrawer (ParticleEmitter particleEmitter)
        {
            this.effect = particleEmitter.particleEffect;
            emmiter = particleEmitter;
        }

        public void Draw(Matrix projection, Camera camera, GraphicsDevice gd, GameTime gameTime)
        {
            GraphicsDevice device = gd;
            device.RasterizerState = RasterizerState.CullNone;

            if (emmiter.vertexBuffer.IsContentLost)
            {
                emmiter.vertexBuffer.SetData(emmiter.ParticleList);
            }

            if (emmiter.firstNewParticle != emmiter.firstFreeParticle)
            {
                emmiter.AddNewParticlesToVertexBuffer();
            }

            // If there are any active particles, draw them now!
            if (emmiter.firstActiveParticle != emmiter.firstFreeParticle)
            {
                device.BlendState = emmiter.BlendState;
                device.DepthStencilState = DepthStencilState.DepthRead;
                
                effect.Parameters["ViewProjection"].SetValue(camera.CameraMatrix * projection);
                effect.Parameters["Projection"].SetValue(projection);

                effect.Parameters["ViewportScale"].SetValue(new Vector2(0.5f / device.Viewport.AspectRatio, -0.5f));

                effect.Parameters["CurrentTime"].SetValue(emmiter.currentTime);

                device.SetVertexBuffer(emmiter.vertexBuffer);
                device.Indices = emmiter.indexBuffer;


                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    if (emmiter.firstActiveParticle < emmiter.firstFreeParticle)
                    {
                        device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0,
                                                      emmiter.firstActiveParticle * 4, (emmiter.firstFreeParticle - emmiter.firstActiveParticle) * 4,
                                                      emmiter.firstActiveParticle * 6, (emmiter.firstFreeParticle - emmiter.firstActiveParticle) * 2);
                    }
                    else
                    {

                        device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0,
                                                     emmiter.firstActiveParticle * 4, (emmiter.MaxParticles - emmiter.firstActiveParticle) * 4,
                                                     emmiter.firstActiveParticle * 6, (emmiter.MaxParticles - emmiter.firstActiveParticle) * 2);

                        if (emmiter.firstFreeParticle > 0)
                        {
                            device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0,
                                                         0, emmiter.firstFreeParticle * 4,
                                                         0, emmiter.firstFreeParticle * 2);
                        }
                    }
                }

                device.DepthStencilState = DepthStencilState.Default;
            }

            emmiter.drawCounter++;
        }


    }
}
