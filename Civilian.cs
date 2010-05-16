using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ICGame
{
    public class Civilian : GameObject, IAnimated, IPhysical, IDestructible, IInteractive
    {
        public Civilian(Model model)
            : base(model)
        {
            
        }
        #region IPhysical Members

        public int BoundingBox
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region IDestructible Members

        public int HP
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Destroy()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IInteractive Members

        public bool Selected
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region IDestructible Members


        public int Damage
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Fade()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
