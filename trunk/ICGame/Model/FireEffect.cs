using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICGame.ParticleSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ICGame
{
    public class FireEffect : IObjectEffect
    {
        private bool isActive;
        public GameObject GameObject { get; set; }

        public ParticleEmitter particleEmmiter;

        public FireEffect(Game game)
        {
            CreateParticleEmitter(game);
        }

        public FireEffect(GameObject gameObject, Game game)
        {
            GameObject = gameObject;
            CreateParticleEmitter(game);
        }

        private void CreateParticleEmitter(Game game)
        {

            particleEmmiter = new ParticleEmitter();

            IsActive = false;

            particleEmmiter.MaxParticles = 10000;

            particleEmmiter.Duration = TimeSpan.FromSeconds(3);

            particleEmmiter.DurationRandomness = 1;

            particleEmmiter.MinHorizontalVelocity = 0;
            particleEmmiter.MaxHorizontalVelocity = 3;

            particleEmmiter.MinVerticalVelocity = -5;
            particleEmmiter.MaxVerticalVelocity = 5;

            // Set gravity upside down, so the flames will 'fall' upward.
            particleEmmiter.Gravity = new Vector3(0, 5, 0);

            particleEmmiter.MinColor = new Color(255, 255, 255, 10);
            particleEmmiter.MaxColor = new Color(255, 255, 255, 40);

            particleEmmiter.MinStartSize = 5;
            particleEmmiter.MaxStartSize = 8;

            particleEmmiter.MinEndSize = 15;
            particleEmmiter.MaxEndSize = 30;

            // Use additive blending.
            particleEmmiter.BlendState = BlendState.Additive;


            particleEmmiter.Reset();

            particleEmmiter.LoadContent(game.GraphicsDevice);
            particleEmmiter.LoadParticleEffect(game.Content.Load<Texture2D>("Texture2D/Particle/fire"));
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
            return new FireEffectDrawer(this, GameObject);
        }

        public void Update(GameTime gameTime)
        {
            if (GameObject == null)
            {
                throw new NullReferenceException("No GameObject assigned");
            }
            Random random = new Random(gameTime.TotalGameTime.Milliseconds);
            for (int i = 0; i < 2; i++)
                if (this.IsActive)
                {

                    particleEmmiter.AddParticle((GameObject as Building).GetRandomPoint(random), Vector3.Zero);
                    particleEmmiter.Update(gameTime);
                }
        }

        #endregion
    }
}
