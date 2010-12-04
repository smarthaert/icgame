using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICGame.Tools
{
    public class BoolEventArgs
    {
        public bool Arg { get; set; }

        public BoolEventArgs(bool arg)
        {
            Arg = arg;
        }
    }
}
