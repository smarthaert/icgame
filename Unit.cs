using System;
using System.Collections.Generic;
using System.Text;

namespace ICGame
{
    public class Unit : GameObject, IAnimated, IPhysical, IInteractive, IDestructible, IControllable
    {
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

        public void Destroy()
        {
            throw new NotImplementedException();
        }

        public void Fade()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IControllable Members

        public Microsoft.Xna.Framework.Vector2 Destination
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

        public Microsoft.Xna.Framework.Vector2 NextStep
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

        public void CalculateNextStep()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
