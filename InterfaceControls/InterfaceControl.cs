using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace InterfaceControls
{
    public abstract class InterfaceControl
    {
        public Vector2 Position { get; set; }

        public Vector2 Size { get; set; }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            
        }
    }
}
