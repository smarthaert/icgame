using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ICGame.ObjectStats
{
    public class SubElement
    {
        public string Name { get; set; }
        public Vector3 Position { get; set;}
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; }

        public SubElement(string name, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            Name = name;
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }
    }
}
