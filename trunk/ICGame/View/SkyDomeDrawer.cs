using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ICGame
{
    class SkyDomeDrawer
    {
        public SkyDomeDrawer(Board board)
        {
            Board = board;
        }

        private Board Board { get; set; }

        /// <summary>
        /// Renderuje SkyDome'a
        /// </summary>
        /// <param name="alpha">Przezroczystość</param>
        public void Draw(float? alpha = null)
        {
            Matrix[] modelTransforms = new Matrix[Board.SkyDomeModel.Bones.Count];
            Board.SkyDomeModel.CopyAbsoluteBoneTransformsTo(modelTransforms);
            Vector3 modifiedCameraPosition = DisplayController.Camera.CameraPosition;
            modifiedCameraPosition.Y = -45.0f;

            foreach (ModelMesh mesh in Board.SkyDomeModel.Meshes)
            {
                foreach (Effect currentEffect in mesh.Effects)
                {
                    Matrix worldMatrix = modelTransforms[mesh.ParentBone.Index] * Matrix.CreateScale(400) * Matrix.CreateTranslation(modifiedCameraPosition);
                    currentEffect.CurrentTechnique = currentEffect.Techniques["SkyDome"];
                    currentEffect.Parameters["xWorldViewProjection"].SetValue(worldMatrix * DisplayController.Camera.CameraMatrix * DisplayController.Projection);
                    currentEffect.Parameters["xTexture"].SetValue(Board.CloudMap);
                    currentEffect.Parameters["xEnableLighting"].SetValue(false);

                    if (alpha == null)
                    {
                        currentEffect.Parameters["xSetAlpha"].SetValue(false);
                    }
                    else
                    {
                        currentEffect.Parameters["xSetAlpha"].SetValue(true);
                        currentEffect.Parameters["xAlpha"].SetValue((float)alpha);
                    }
                }
                mesh.Draw();
            }
        }
    }
}
