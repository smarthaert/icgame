using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ICGame
{
    public class DisplayController
    {
        private Display display;

        public static Camera Camera { get; set; }
        /// <summary>
        /// Macierz rzutowania. Pole statyczne inicjowane w konstruktorze DisplayController'a.
        /// </summary>
        public static Matrix Projection { get; set; }

        public DisplayController(GraphicsDeviceManager graphicsDeviceManager, UserInterface userInterface, Camera camera, CampaignController campaignController)
        {
            Camera = camera;
            display = new Display(graphicsDeviceManager, userInterface, campaignController);
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, graphicsDeviceManager.GraphicsDevice.Viewport.AspectRatio, 1.0f, 600.0f);

        }

        public void DrawScene(IEnumerable<IDrawer> drawers, GameTime gameTime)
        {
            display.Draw(drawers, Camera, gameTime);
        }
    }
}
