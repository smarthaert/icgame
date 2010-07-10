﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ICGame
{
    public class GameObjectDrawer
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

        public virtual void Draw(Matrix projection, Camera camera, GraphicsDevice gd)
        {
            Matrix[] transforms = new Matrix[GameObject.Model.Bones.Count];
            GameObject.Model.CopyAbsoluteBoneTransformsTo(transforms);
            
            int i = 0;
            foreach (var model in GameObject.Model.Meshes)
            {
                //if (model.Name == "Window0" || model.Name == "DoorLeft" || model.Name == "DoorRight" || model.Name == "AlertBarBlue")
                //{
                //    gd.RenderState.DepthBufferWriteEnable = false;
                //}
                foreach (Effect effect in model.Effects)
                {
                    effect.CurrentTechnique = effect.Techniques["TexturedShaded"];
                    effect.Parameters["xEnableLighting"].SetValue(true);
                    Vector3 lightDirection = new Vector3(0.5f, 0, -1.0f);
                    lightDirection.Normalize();
                    effect.Parameters["xLightDirection"].SetValue(lightDirection);
                    effect.Parameters["xAmbient"].SetValue(0.2f);
                    effect.Parameters["xCameraPosition"].SetValue(camera.CameraPosition);
                    effect.Parameters["xDiffuseColor"].SetValue(GameObject.DiffuseColor[i]);
                    effect.Parameters["xTransparency"].SetValue(GameObject.Transparency[i]);

                    effect.Parameters["xTexture"].SetValue(GameObject.Textures[i++]);
                    effect.Parameters["xWorld"].SetValue(transforms[model.ParentBone.Index] * GameObject.ModelMatrix);
                    effect.Parameters["xView"].SetValue(camera.CameraMatrix);
                    effect.Parameters["xProjection"].SetValue(projection);
                    //effect.GraphicsDevice.RenderState.AlphaBlendEnable = false;
                }
                model.Draw();
                //if (model.Name == "Window0" || model.Name == "DoorLeft" || model.Name == "DoorRight" || model.Name == "AlertBarBlue")
                //{
                //    gd.RenderState.DepthBufferWriteEnable = true;
                //}

            }
        }
    }
}