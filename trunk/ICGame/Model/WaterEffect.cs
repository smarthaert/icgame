using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICGame.ParticleSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ICGame
{
    public class WaterEffect : IObjectEffect
    {
        private bool isActive;
        GameObject GameObject { get; set; }

        public ParticleEmitter particleEmmiter;
        private Game game;
        public WaterEffect(GameObject gameObject, Game game)
        {
            GameObject = gameObject;
            particleEmmiter = new ParticleEmitter();
            
            IsActive = false;
            this.game = game;


            

            particleEmmiter.MaxParticles = 1200;

            particleEmmiter.Duration = TimeSpan.FromSeconds(2);

            particleEmmiter.MinHorizontalVelocity = 1;
            particleEmmiter.MaxHorizontalVelocity = 4;

            particleEmmiter.MinVerticalVelocity = 0;
            particleEmmiter.MaxVerticalVelocity = 5;

            particleEmmiter.Gravity = new Vector3(0.3f, -10, 0.3f);

            particleEmmiter.EndVelocity = 1;

            particleEmmiter.MinRotateSpeed = -1;
            particleEmmiter.MaxRotateSpeed = 1;

            particleEmmiter.MinStartSize = 1;
            particleEmmiter.MaxStartSize = 3;

            particleEmmiter.MinColor = new Color(0, 0, 50, 0.1f);
            particleEmmiter.MaxColor = new Color(255,255,200,0.9f);
            particleEmmiter.BlendState = BlendState.AlphaBlend;

            particleEmmiter.MinEndSize = 3;
            particleEmmiter.MaxEndSize = 7;
            
            particleEmmiter.Reset();
            
            particleEmmiter.LoadContent(game.GraphicsDevice);
            particleEmmiter.LoadParticleEffect(game.Content.Load<Texture2D>("Texture2D/Particle/water"));
            

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
            return new WaterEffectDrawer(this, GameObject);
        }

        public void Update(GameTime gameTime)
        {

            for (int i = 0; i < 2; i++)
                if (this.IsActive)
                {
                    float targetAngle = (GameObject as Vehicle).TurretAngle;
                    particleEmmiter.AddParticle((GameObject as Vehicle).GetWaterSourcePosition(), new Vector3((float)(Math.Sin(targetAngle + GameObject.Angle.Y)) * 30, (float)Math.Sin(MathHelper.PiOver2), (float)(Math.Cos(targetAngle + GameObject.Angle.Y)) * 30));
                    particleEmmiter.AddParticle((GameObject as Vehicle).GetSecondWaterSourcePosition(), new Vector3((float)(Math.Sin(targetAngle + GameObject.Angle.Y)) * 30, (float)Math.Sin(MathHelper.PiOver2), (float)(Math.Cos(targetAngle + GameObject.Angle.Y)) * 30));
                }
            particleEmmiter.Update(gameTime);
        }

        #endregion
    }
}
