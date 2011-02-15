using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace ICGame
{
    public abstract class EffectDrawer : IParticleEffectDrawer
    {
        protected IObjectEffect objectEffect; 
        protected GameObject gameObject; 

        public EffectDrawer(IObjectEffect _objectEffect, GameObject _gameObject)
        {
            objectEffect = _objectEffect;
            gameObject = _gameObject;
        }
        

        public abstract void Draw(Microsoft.Xna.Framework.Graphics.GraphicsDevice gd, GameTime gameTime, Vector4? clipPlane = null, float? alpha = null);
    }
}
