using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ICGame
{
    public class WaterEffect : IObjectEffect
    {

        GameObject GameObject { get; set; }

        public WaterEffect(GameObject gameObject)
        {
            GameObject = gameObject;
        }

        #region IObjectEffect Members

        public bool IsActive
        {
            get; set;
        }

        public EffectDrawer GetDrawer()
        {
            return new WaterEffectDrawer(this,GameObject);
        }

        public void Update(GameTime gameTime)
        {
            
        }

        #endregion
    }
}
