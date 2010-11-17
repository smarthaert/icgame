using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICGame.ParticleSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ICGame
{
    public class FireEffectDrawer : EffectDrawer
    {
        private Effect effect;

        public FireEffectDrawer(IObjectEffect objectEffect, GameObject gameObject, Effect effect)
            : base(objectEffect, gameObject)
        {
            this.gameObject = gameObject;
            this.objectEffect = objectEffect;
            this.effect = effect;
        }

        public override void Draw(Matrix projection, Camera camera,
                         Microsoft.Xna.Framework.Graphics.GraphicsDevice gd, GameTime gameTime)
        {
            new ParticleDrawer((objectEffect as FireEffect).particleEmmiter).Draw(projection, camera, gd, gameTime);
        }
    }
}
