using System;
using System.Collections.Generic;
using System.Text;

namespace ICGame
{
    public interface IDestructible
    {
        int HP
        {
            get;
            set;
        }

        int Damage
        {
            get;
            set;
        }

        void Destroy();

        void Fade();
    }
}
