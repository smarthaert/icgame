using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ICGame
{
    public class WaterEffectDrawer : EffectDrawer
    {
        public WaterEffectDrawer(IObjectEffect objectEffect, GameObject gameObject)
          : base(objectEffect,gameObject)
        {
                
        }

        public override void Draw(Matrix projection, Camera camera,
                         Microsoft.Xna.Framework.Graphics.GraphicsDevice gd)
        {
            
        }
    }
}
