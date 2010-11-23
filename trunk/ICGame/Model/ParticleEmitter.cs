using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;

namespace ICGame.ParticleSystem
{
    public class ParticleEmitter
    {
        public Vector3 Position { get; set; }

        public Effect particleEffect;

        // Name of the texture used by this particle system.
        public string TextureName = null;
        
        public int MaxParticles = 100;

        public TimeSpan Duration = TimeSpan.FromSeconds(1);

        public float DurationRandomness = 0;

        public float EmitterVelocitySensitivity = 1;

        public float MinHorizontalVelocity = 0;
        public float MaxHorizontalVelocity = 0;

        public float MinVerticalVelocity = 0;
        public float MaxVerticalVelocity = 0;
        
        public Vector3 Gravity = Vector3.Zero;
        
        public float EndVelocity = 1;

        public Color MinColor = Color.White;
        public Color MaxColor = Color.White;

        public float MinRotateSpeed = 0;
        public float MaxRotateSpeed = 0;

        public float MinStartSize = 100;
        public float MaxStartSize = 100;
        
        public float MinEndSize = 100;
        public float MaxEndSize = 100;

        public BlendState BlendState = BlendState.NonPremultiplied;

        // Pomocnicze przy kolejkowaniu
        public int firstActiveParticle;
        public int firstNewParticle;
        public int firstFreeParticle;
        public int firstRetiredParticle;

        public int drawCounter;
        public float currentTime;


        static Random random = new Random();


        public Particle[] ParticleList;

        public DynamicVertexBuffer vertexBuffer;
        public IndexBuffer indexBuffer;

        public ParticleEmitter()
        {
          
        }
        
        public void Reset()
        {
            ParticleList = new Particle[MaxParticles * 4];

            for (int i = 0; i < MaxParticles; i++)
            {
                ParticleList[i * 4 + 0].Corner = new Short2(-1, -1);
                ParticleList[i * 4 + 1].Corner = new Short2(1, -1);
                ParticleList[i * 4 + 2].Corner = new Short2(1, 1);
                ParticleList[i * 4 + 3].Corner = new Short2(-1, 1);
            }

        }

        public void LoadContent(GraphicsDevice graphicsDevice)
        {
          //  LoadParticleEffect();


            vertexBuffer = new DynamicVertexBuffer(graphicsDevice, Particle.VertexDeclaration,
                                                   MaxParticles * 4, BufferUsage.WriteOnly);

 
            ushort[] indices = new ushort[MaxParticles * 6];

            for (int i = 0; i < MaxParticles; i++)
            {
                indices[i * 6 + 0] = (ushort)(i * 4 + 0);
                indices[i * 6 + 1] = (ushort)(i * 4 + 1);
                indices[i * 6 + 2] = (ushort)(i * 4 + 2);

                indices[i * 6 + 3] = (ushort)(i * 4 + 0);
                indices[i * 6 + 4] = (ushort)(i * 4 + 2);
                indices[i * 6 + 5] = (ushort)(i * 4 + 3);
            }

            indexBuffer = new IndexBuffer(graphicsDevice, typeof(ushort), indices.Length, BufferUsage.WriteOnly);

            indexBuffer.SetData(indices);
        }

        public void LoadParticleEffect(Texture2D texture)
        {
            particleEffect = TechniqueProvider.GetEffect("ParticleEffect").Clone();

            EffectParameterCollection parameters = particleEffect.Parameters;

            parameters["Duration"].SetValue((float)Duration.TotalSeconds);
            parameters["DurationRandomness"].SetValue(DurationRandomness);
            parameters["Gravity"].SetValue(Gravity);
            parameters["EndVelocity"].SetValue(EndVelocity);
            parameters["MinColor"].SetValue(MinColor.ToVector4());
            parameters["MaxColor"].SetValue(MaxColor.ToVector4());

            parameters["RotateSpeed"].SetValue(
                new Vector2(MinRotateSpeed, MaxRotateSpeed));

            parameters["StartSize"].SetValue(
                new Vector2(MinStartSize, MaxStartSize));

            parameters["EndSize"].SetValue(
                new Vector2(MinEndSize, MaxEndSize));

            parameters["Texture"].SetValue(texture);
        }

        public ParticleDrawer GetDrawer()
        {
            return new ParticleDrawer(this);
        }

        public void Update(GameTime gameTime)
        {
            if (gameTime == null)
                throw new ArgumentNullException("gameTime");

            currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            RetireActiveParticles();
            FreeRetiredParticles();
            
            if (firstActiveParticle == firstFreeParticle)
                currentTime = 0;

            if (firstRetiredParticle == firstActiveParticle)
                drawCounter = 0;
        }

        void RetireActiveParticles()
        {
            float particleDuration = (float)Duration.TotalSeconds;

            while (firstActiveParticle != firstNewParticle)
            {
                float particleAge = currentTime - ParticleList[firstActiveParticle * 4].Time;

                if (particleAge < particleDuration)
                    break;

                ParticleList[firstActiveParticle * 4].Time = drawCounter;

                firstActiveParticle++;

                if (firstActiveParticle >= MaxParticles)
                    firstActiveParticle = 0;
            }
        }

        void FreeRetiredParticles()
        {
            while (firstRetiredParticle != firstActiveParticle)
            {
                int age = drawCounter - (int)ParticleList[firstRetiredParticle * 4].Time;

                if (age < 3)
                    break;

                firstRetiredParticle++;

                if (firstRetiredParticle >= MaxParticles)
                    firstRetiredParticle = 0;
            }
        }

        public void AddNewParticlesToVertexBuffer()
        {
            int stride = Particle.SizeInBytes;

            if (firstNewParticle < firstFreeParticle)
            {
                vertexBuffer.SetData(firstNewParticle * stride * 4, ParticleList,
                                     firstNewParticle * 4,
                                     (firstFreeParticle - firstNewParticle) * 4,
                                     stride, SetDataOptions.NoOverwrite);
            }
            else
            {
                vertexBuffer.SetData(firstNewParticle * stride * 4, ParticleList,
                                     firstNewParticle * 4,
                                     (MaxParticles - firstNewParticle) * 4,
                                     stride, SetDataOptions.NoOverwrite);

                if (firstFreeParticle > 0)
                {
                    vertexBuffer.SetData(0, ParticleList,
                                         0, firstFreeParticle * 4,
                                         stride, SetDataOptions.NoOverwrite);
                }
            }

            firstNewParticle = firstFreeParticle;
        }


        public void AddParticle(Vector3 position, Vector3 velocity)
        {

            int nextFreeParticle = firstFreeParticle + 1;

            if (nextFreeParticle >= MaxParticles)
                nextFreeParticle = 0;


            if (nextFreeParticle == firstRetiredParticle)
                return;


            velocity *= EmitterVelocitySensitivity;

            float horizontalVelocity = MathHelper.Lerp(MinHorizontalVelocity,
                                                       MaxHorizontalVelocity,
                                                       (float)random.NextDouble());

            double horizontalAngle = random.NextDouble() * MathHelper.TwoPi;

            velocity.X += horizontalVelocity * (float)Math.Cos(horizontalAngle);
            velocity.Z += horizontalVelocity * (float)Math.Sin(horizontalAngle);

            velocity.Y += MathHelper.Lerp(MinVerticalVelocity,
                                          MaxVerticalVelocity,
                                          (float)random.NextDouble());


            Color randomValues = new Color((byte)random.Next(255),
                                           (byte)random.Next(255),
                                           (byte)random.Next(255),
                                           (byte)random.Next(255));

            for (int i = 0; i < 4; i++)
            {
                ParticleList[firstFreeParticle * 4 + i].Position = position;
                ParticleList[firstFreeParticle * 4 + i].Velocity = velocity;
                ParticleList[firstFreeParticle * 4 + i].Random = randomValues;
                ParticleList[firstFreeParticle * 4 + i].Time = currentTime;
            }

            firstFreeParticle = nextFreeParticle;
        }
    }
}
