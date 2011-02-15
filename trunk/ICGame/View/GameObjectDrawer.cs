using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ICGame
{
    public class GameObjectDrawer : IDrawer
    {
        public GameObjectDrawer(GameObject gameObject)
        {
            GameObject = gameObject;
        }

        public GameObject GameObject
        {
            get; set;
        }

        public virtual void Draw(GraphicsDevice gd, GameTime gameTime, Vector4? clipPlane, float? alpha = null)
        {
            if(!GameObject.Visible)
            {
                return;
            }
            Matrix[] transforms = new Matrix[GameObject.Model.Bones.Count];
            Matrix modelMatrix = GameObject.AbsoluteModelMatrix;
            GameObject.Model.CopyAbsoluteBoneTransformsTo(transforms);
            gd.RasterizerState = RasterizerState.CullCounterClockwise;
            foreach (var model in GameObject.Model.Meshes)
            {
                foreach (Effect effect in model.Effects)
                {
                    if(clipPlane == null)
                    {
                        effect.Parameters["xClipPlanes"].SetValue(false);
                    }
                    else
                    {
                        effect.Parameters["xClipPlanes"].SetValue(true);
                        effect.Parameters["xClipPlane0"].SetValue((Vector4)clipPlane);
                    }

                    effect.Parameters["xEnableLighting"].SetValue(true);
                    effect.Parameters["xLightDirection"].SetValue(GameObject.Mission.Board.LightDirection);
                    effect.Parameters["xCameraPosition"].SetValue(DisplayController.Camera.CameraPosition);

                    //Macierze
                    effect.Parameters["xWorld"].SetValue(transforms[model.ParentBone.Index] * modelMatrix);
                    effect.Parameters["xView"].SetValue(DisplayController.Camera.CameraMatrix);
                    effect.Parameters["xWorldViewProjection"].SetValue(transforms[model.ParentBone.Index]*modelMatrix*
                                                                       DisplayController.Camera.CameraMatrix*
                                                                       DisplayController.Projection);
                    
                    if(alpha == null)
                    {
                        effect.Parameters["xSetAlpha"].SetValue(false);
                    }
                    else
                    {
                        effect.Parameters["xSetAlpha"].SetValue(true);
                        effect.Parameters["xAlpha"].SetValue((float)alpha);
                    }
                }

                model.Draw();

            }
            gd.RasterizerState = RasterizerState.CullClockwise;
        }
    }
}
