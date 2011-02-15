﻿using System;
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

        public EffectDrawer EffectDrawer
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        /// <summary>
        /// Rysowanie miniaturki modelu, na pozycji UI
        /// </summary>
        /// <param name="projection">macierz rzutowania</param>
        /// <param name="camera">Obiekt klasy Camera</param>
        /// <param name="gd">Graphics device</param>
        public virtual void DrawSmallModel(Matrix projection, Camera camera, GraphicsDevice gd, GameTime gameTime, float? alpha = null)
        {
            if (!GameObject.Visible)
            {
                return;
            }

            foreach (GameObject child in GameObject.GetChildren())
            {
                child.GetDrawer().DrawSmallModel(projection, camera, gd, gameTime, alpha);
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

            Camera temporaryCamera = new Camera(camera);
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
                    effect.Parameters["xCameraPosition"].SetValue(camera.CameraPosition);

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
                                                                   projection,
                                                                   temporaryCamera.CameraMatrix, Matrix.Identity);

                    effect.Parameters["xWorld"].SetValue(transforms[model.ParentBone.Index] *
                                                         GameObject.GetAbsoluteSmallModelMatrix(unproject, gd.Viewport.Width,
                                                                                        gameTime));
                    temporaryCamera.Reset();
                    effect.Parameters["xView"].SetValue(temporaryCamera.CameraMatrix);
                    effect.Parameters["xWorldViewProjection"].SetValue(transforms[model.ParentBone.Index] * 
                                                                        GameObject.GetAbsoluteSmallModelMatrix(unproject, gd.Viewport.Width, gameTime) * 
                                                                        temporaryCamera.CameraMatrix * projection);
                    
                }
                model.Draw();
                foreach (Effect effect in model.Effects)
                {
                    if(GameObject.Textures[j++] != null)      //inaczej się kurwa nie dało
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