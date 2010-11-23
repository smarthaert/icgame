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
        GameObject GameObject { get; set; }

        public ParticleEmitter particleEmmiter;
        private Game game;
        public FireSmokeEffect(GameObject gameObject, Game game)
        {
            GameObject = gameObject;
            particleEmmiter = new ParticleEmitter();

            IsActive = false;
            this.game = game;

            particleEmmiter.MaxParticles = 600;

            particleEmmiter.Duration = TimeSpan.FromSeconds(10);

            particleEmmiter.MinHorizontalVelocity = 0;
            particleEmmiter.MaxHorizontalVelocity = 10;

            particleEmmiter.MinVerticalVelocity = 6;
            particleEmmiter.MaxVerticalVelocity = 13;

            // Create a wind effect by tilting the gravity vector sideways.
            particleEmmiter.Gravity = new Vector3(7, -2, 0);

            particleEmmiter.EndVelocity = 0.75f;

            particleEmmiter.MinRotateSpeed = -1;
            particleEmmiter.MaxRotateSpeed = 1;

            particleEmmiter.MinStartSize = 4;
            particleEmmiter.MaxStartSize = 7;

            particleEmmiter.MinEndSize = 35;
            particleEmmiter.MaxEndSize = 60;


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
            return new FireSmokeEffectDrawer(this, GameObject, TechniqueProvider.GetEffect("ParticleEffect"));
        }

        public void Update(GameTime gameTime)
        {
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
