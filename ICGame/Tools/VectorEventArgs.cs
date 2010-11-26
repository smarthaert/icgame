using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ICGame
{
    public class VectorEventArgs : EventArgs
    {
        public Vector3 Vector { get; set; }

        public VectorEventArgs(Vector3 vector)
        {
            Vector = vector;
        }
    }
}
