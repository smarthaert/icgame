using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace ICGame
{
    public class VehicleDrawer : GameObjectDrawer
    {
        public VehicleDrawer(GameObject gameObject)
            : base(gameObject)

        {
           
        }

        public override void Draw(Matrix projection, Matrix viewMatrix, Vector3 cameraPosition, 
            Microsoft.Xna.Framework.Graphics.GraphicsDevice gd, Vector4? clipPlane)
        {
            Vehicle vehicle = GameObject as Vehicle;
            if (vehicle.Selected)
            {
                vehicle.SelectionRing.GetDrawer().Draw(projection, viewMatrix, cameraPosition, gd, clipPlane);
            }

            base.Draw(projection, viewMatrix, cameraPosition, gd, clipPlane);
        }
    }
}
