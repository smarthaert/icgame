using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ICGame
{
    public interface IObjectEffect
    {
        bool IsActive
        {
            get;
            set;
        }

        EffectDrawer GetDrawer();

        void Update(GameTime gameTime);
    }
}
