using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICGame.ParticleSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ICGame
{
    public class FireSmokeEffectDrawer : EffectDrawer
    {
        public FireSmokeEffectDrawer(IObjectEffect objectEffect, GameObject gameObject)
            : base(objectEffect, gameObject)
        {
            this.gameObject = gameObject;
            this.objectEffect = objectEffect;
        }

        public override void Draw(GraphicsDevice gd, GameTime gameTime, Vector4? clipPlane = null, float? alpha = null)
        {
            new ParticleDrawer((objectEffect as FireSmokeEffect).particleEmmiter).Draw(gd, gameTime);
        }
    }
}
