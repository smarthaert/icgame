using System;
using System.Collections.Generic;
using System.Text;

namespace ICGame
{
    public abstract class GameObject : IDrawable
    {
        #region IDrawable Members

        public virtual void GetDrawer()
        {
            throw new NotImplementedException();
        }

        #endregion

        public void Update()
        {
            throw new System.NotImplementedException();
        }

        #region IDrawable Members


        #endregion
    }
}
