using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICGame.ObjectStats
{
    public class VehicleStats : UnitStats
    {
        public int FrontWheelCount { get; set; }
        public int RearWheelCount { get; set; }
        public int DoorCount { get; set; }
        public bool HasTurret { get; set; }
        public int WaterSourceCount { get; set; }
    }
}
