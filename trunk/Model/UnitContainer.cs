using System;
using System.Collections.Generic;
using System.Text;

namespace ICGame
{
    public class UnitContainer
    {
        public List<Unit> Units
        {
            get; set;
        }

        public UnitContainer()
        {
            Units = new List<Unit>();
        }

    }
}
