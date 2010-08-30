using System;
using System.Collections.Generic;
using System.Text;

namespace ICGame
{
    public class VehicleDrawer : GameObjectDrawer
    {
        public VehicleDrawer(GameObject gameObject)
            : base(gameObject)

        {
           
        }

        public override void Draw(Microsoft.Xna.Framework.Matrix projection, Camera camera, Microsoft.Xna.Framework.Graphics.GraphicsDevice gd)
        {
            Vehicle vehicle = GameObject as Vehicle;
            if(vehicle.Selected)
                vehicle.SelectionRing.GetDrawer().Draw(projection,camera,gd);
                
            base.Draw(projection, camera, gd);
        }
    }
}
