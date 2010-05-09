using System;
using System.Collections.Generic;
using System.Text;

namespace ICGame
{
    public interface IControllable
    {
        Microsoft.Xna.Framework.Vector2 Destination
        {
            get;
            set;
        }

        Microsoft.Xna.Framework.Vector2 NextStep
        {
            get;
            set;
        }

        void CalculateNextStep();
    }
}
