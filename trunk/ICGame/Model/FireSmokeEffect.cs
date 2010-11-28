using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICGame.ParticleSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ICGame
{
    public class FireSmokeEffect : IObjectEffect
    {
        private bool isActive;
        public GameObject GameObject { get; set; }

        public ParticleEmitter particleEmmiter;

        public FireSmokeEffect(Game game)
        {
            CreateParticleEmitter(game);
        }

        public FireSmokeEffect(GameObject gameObject, Game game)
        {
            GameObject = gameObject;
            CreateParticleEmitter(game);
        }

        private void CreateParticleEmitter(Game game)
        {
            particleEmmiter = new ParticleEmitter();

            IsActive = false;

            particleEmmiter.MaxParticles = 600;

            particleEmmiter.Duration = TimeSpan.FromSeconds(10);

            particleEmmiter.MinHorizontalVelocity = 0;
            particleEmmiter.MaxHorizontalVelocity = 5;

            particleEmmiter.MinVerticalVelocity = 3;
            particleEmmiter.MaxVerticalVelocity = 7;

            // Create a wind effect by tilting the gravity vector sideways.
            particleEmmiter.Gravity = new Vector3(7, -2, 0);

            particleEmmiter.EndVelocity = 0.75f;

            particleEmmiter.MinRotateSpeed = -1;
            particleEmmiter.MaxRotateSpeed = 1;

            particleEmmiter.MinStartSize = 2;
            particleEmmiter.MaxStartSize = 4;

            particleEmmiter.MinEndSize = 17;
            particleEmmiter.MaxEndSize = 30;


            particleEmmiter.Reset();

            particleEmmiter.LoadContent(game.GraphicsDevice);
            particleEmmiter.LoadParticleEffect(game.Content.Load<Texture2D>("Texture2D/Particle/smoke"));
        }

        #region IObjectEffect Members

        public bool IsActive
        {
            get { return isActive; }
            set
            {
                isActive = value;
                particleEmmiter.Reset();
            }
        }

        public EffectDrawer GetDrawer()
        {
            if (GameObject == null)
            {
                throw new NullReferenceException("No GameObject assigned");
            }
            return new FireSmokeEffectDrawer(this, GameObject);
        }

        public void Update(GameTime gameTime)
        {
            if (GameObject == null)
            {
                throw new NullReferenceException("No GameObject assigned");
            }
            Random random = new Random(gameTime.TotalGameTime.Milliseconds);
            for (int i = 0; i < 1; i++)
                if (this.IsActive)
                {

                    particleEmmiter.AddParticle((GameObject as Building).GetRandomPoint(random), Vector3.Zero);
                    particleEmmiter.Update(gameTime);
                }
        }

        #endregion
    }
}
