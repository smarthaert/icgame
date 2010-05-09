using System;
using System.Collections.Generic;
using System.Text;

namespace ICGame
{
    public class GameObject:IDrawable
    {
        #region IDrawable Members

        public void Draw()
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
