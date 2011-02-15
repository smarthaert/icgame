using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ICGame.ObjectStats
{
    public class SubElement
    {
        public string Name { get; set; }
        public Vector3 Position { get; set;}
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; }

        private GameObject gameObject;
        public GameObject GameObject 
        {
            get
            {
                if(gameObject == null)
                {
                    throw new NullReferenceException("Nie ma modelu");
                }
                return gameObject;
            }
            set
            {
                gameObject = value;
            }
        }

        public SubElement(string name, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            Name = name;
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }
    }
}
