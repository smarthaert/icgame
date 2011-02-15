using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ICGame
{
    public class VehicleDrawer : GameObjectDrawer
    {
        public VehicleDrawer(GameObject gameObject)
            : base(gameObject)

        {
           
        }

        public override void Draw(GraphicsDevice gd, GameTime gameTime, Vector4? clipPlane, float? alpha = null)
        {
            base.Draw(gd, gameTime, clipPlane, alpha);
        }
    }
}
