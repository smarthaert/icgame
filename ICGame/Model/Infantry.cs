using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ICGame
{
    public class Infantry : Unit
    {
        public Infantry(Model model, ObjectStats.InfantryStats infantryStats)
            : base(model, infantryStats)
        {
            
        }

        public override void Animate(GameTime gameTime, IEnumerable<GameObject> gameObjects)
        {
            
            base.Animate(gameTime, gameObjects);
        }
    }
}
