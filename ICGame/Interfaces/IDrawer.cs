using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ICGame
{
    public interface IDrawer
    {
        void Draw(GraphicsDevice device, GameTime gameTime, Vector4? clipPlane = null, float? alpha = null);
    }
}
