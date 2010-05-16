using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ICGame
{
    public abstract class GameObject : IDrawable
    {
        public Matrix ModelMatrix
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public Model Model
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }
        #region IDrawable Members

        public virtual GameObjectDrawer GetDrawer()
        {
            
        }

        #endregion

        public void Update()
        {
            
        }

        #region IDrawable Members


        #endregion
    }
}
