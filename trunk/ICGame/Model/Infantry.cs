using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ICGame
{
    public class Infantry : Unit
    {
        public Infantry(Model model, float speed, float turnRadius)
            : base(model, speed, turnRadius)
        {
            
        }

        public override void Animate(GameTime gameTime, List<GameObject> gameObjects)
        {
            
            base.Animate(gameTime, gameObjects);
        }
    }
}
