using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace InterfaceControls
{
    public class Panel : InterfaceControl
    {
        public Texture2D Texture { get; set; }

        public Color Color { get; set; }

        public Panel()
        {
            Color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if(Texture == null)
                throw new NullReferenceException("No texture set");

            spriteBatch.Draw(Texture,
                             new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y),
                                           Convert.ToInt32(Size.X), Convert.ToInt32(Size.Y)), Color);
        }
    }
}
