using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ICGame
{
    public class MiniModelDrawer : IDrawer
    {
        public MiniModelDrawer(GameObject gameObject)
        {
            GameObject = gameObject;
        }

        public GameObject GameObject
        {
            get; set;
        }

        /// <summary>
        /// Rysowanie miniaturki modelu, na pozycji UI
        /// </summary>
        /// <param name="projection">macierz rzutowania</param>
        /// <param name="camera">Obiekt klasy Camera</param>
        /// <param name="gd">Graphics device</param>
        public virtual void Draw(GraphicsDevice gd, GameTime gameTime, Vector4? clipPlane = null, float? alpha = null)
        {
            if (!GameObject.Visible)
            {
                return;
            }

            foreach (GameObject child in GameObject.GetChildren())
            {
                child.GetMiniModelDrawer().Draw(gd, gameTime, clipPlane, alpha);
            }

            gd.BlendState = BlendState.AlphaBlend;
            gd.RasterizerState = RasterizerState.CullCounterClockwise;
            Matrix[] transforms = new Matrix[GameObject.Model.Bones.Count];
            if (GameObject is IAnimated)
            {
                transforms = ((IAnimated)GameObject).GetBasicTransforms();
            }
            else
            {
                GameObject.Model.CopyAbsoluteBoneTransformsTo(transforms);
            }

            Camera temporaryCamera = new Camera(DisplayController.Camera);
            temporaryCamera.CalculateCamera();
            int i = 0;
            foreach (var model in GameObject.Model.Meshes)
            {
                int j = i;  //do przywracania poprzednich technik
                foreach (Effect effect in model.Effects)
                {
                    if (GameObject.Textures[i] != null)      //inaczej się kurwa nie dało
                    {
                        effect.CurrentTechnique = effect.Techniques["BlueHologram"];
                    }
                    else
                    {
                        effect.CurrentTechnique = effect.Techniques["NotRlyBlueHologram"];
                    }

                    effect.Parameters["xEnableLighting"].SetValue(true);
                    Vector3 lightDirection = new Vector3(0.5f, 0, -1.0f);
                    lightDirection.Normalize();
                    effect.Parameters["xLightDirection"].SetValue(lightDirection);
                    effect.Parameters["xCameraPosition"].SetValue(DisplayController.Camera.CameraPosition);

                    //Parametry materialu
                    effect.Parameters["xAmbient"].SetValue(GameObject.Ambient[i]);
                    effect.Parameters["xDiffuseColor"].SetValue(GameObject.DiffuseColor[i]);
                    effect.Parameters["xDiffuseFactor"].SetValue(GameObject.DiffuseFactor[i]);

                    effect.Parameters["xTransparency"].SetValue(GameObject.Transparency[i]);
                    effect.Parameters["xSpecularColor"].SetValue(GameObject.Specular[i]);
                    effect.Parameters["xSpecularFactor"].SetValue(GameObject.SpecularFactor[i]);

                    // Vector3 b = effect.Parameters["xDiffuseColor"].GetValueVector3();
                    effect.Parameters["xHasTexture"].SetValue(GameObject.Textures[i] != null ? true : false);
                    effect.Parameters["xTexture"].SetValue(GameObject.Textures[i++]);

                    if (alpha == null)
                    {
                        effect.Parameters["xSetAlpha"].SetValue(false);
                    }
                    else
                    {
                        effect.Parameters["xSetAlpha"].SetValue(true);
                        effect.Parameters["xAlpha"].SetValue((float)alpha);
                    }

                    //Macierze
                    Vector3 unproject = gd.Viewport.Unproject(new Vector3(gd.Viewport.Width - 100, 80, 0.13f),
                                                                   DisplayController.Projection,
                                                                   temporaryCamera.CameraMatrix, Matrix.Identity);

                    effect.Parameters["xWorld"].SetValue(transforms[model.ParentBone.Index] *
                                                         GameObject.GetAbsoluteSmallModelMatrix(unproject, gd.Viewport.Width,
                                                                                        gameTime));
                    temporaryCamera.Reset();
                    effect.Parameters["xView"].SetValue(temporaryCamera.CameraMatrix);
                    effect.Parameters["xWorldViewProjection"].SetValue(transforms[model.ParentBone.Index] *
                                                                        GameObject.GetAbsoluteSmallModelMatrix(unproject, gd.Viewport.Width, gameTime) *
                                                                        temporaryCamera.CameraMatrix * DisplayController.Projection);

                }
                model.Draw();
                foreach (Effect effect in model.Effects)
                {
                    if (GameObject.Textures[j++] != null)      //inaczej się kurwa nie dało
                    {
                        effect.CurrentTechnique = effect.Techniques["TexturedShaded"];
                    }
                    else
                    {
                        effect.CurrentTechnique = effect.Techniques["NotRlyTexturedShaded"];
                    }
                }
            }
        }
    }
}
