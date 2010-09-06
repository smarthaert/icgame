using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ICGame
{
    public class Infantry : Unit
    {
        public Infantry(Model model, float speed, float turnRadius, GameTime gameTime)
            : base(model, speed, turnRadius)
        {
            
        }
    }
}
