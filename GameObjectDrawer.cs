using System;
using System.Collections.Generic;
using System.Text;

namespace ICGame
{
    public class GameObjectDrawer
    {
        public GameObjectDrawer(GameObject gameObject)
        {
            GameObject = gameObject;
        }

        public GameObject GameObject
        {
            get; set;
        }

        public virtual void Draw()
        {
            
        }
    }
}
