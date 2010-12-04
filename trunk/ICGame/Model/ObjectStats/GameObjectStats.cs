using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICGame.ObjectStats
{
    public class GameObjectStats
    {
        public string Name { get; set; }
        public GameObjectFactory.ObjectClass Type { get; set; }
        public List<string> Effects { get; set; }
        public List<SubElement> SubElements { get; set; }
        public GameObjectFactory GameObjectFactory { get; set; }

        public GameObjectStats()
        {
            Effects = new List<string>();
            SubElements = new List<SubElement>();
        }
    }
}
