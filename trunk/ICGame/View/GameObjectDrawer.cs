using System;
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

        public void DrawSelectionDisc(Matrix projection, Camera camera, GraphicsDevice gd)
        {

        }


        /// <summary>
        /// Rysowanie miniaturki modelu, na pozycji UI
        /// </summary>
        /// <param name="projection">macierz rzutowania</param>
        /// <param name="camera">Obiekt klasy Camera</param>
        /// <param name="gd">Graphics device</param>
        public virtual void DrawSmallModel(Matrix projection, Camera camera, GraphicsDevice gd, GameTime gameTime)
        {
            Matrix[] transforms = new Matrix[GameObject.Model.Bones.Count];
            if (GameObject is IAnimated)
            {
                transforms = ((IAnimated) GameObject).GetBasicTransforms();
            }
            else
            {
                GameObject.Model.CopyAbsoluteBoneTransformsTo(transforms);
            }
            //GameObject.Model.CopyAbsoluteBoneTransformsTo(transforms);
            Camera temporaryCamera = new Camera(camera);
            gd.RasterizerState = RasterizerState.CullClockwise;
            
            int i = 0;
            foreach (var model in GameObject.Model.Meshes)
            {
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
                    effect.Parameters["xCameraPosition"].SetValue(camera.CameraPosition);

                    effect.Parameters["xHasTexture"].SetValue(GameObject.Textures[i] != null ? true : false); 
                    effect.Parameters["xDiffuseFactor"].SetValue(GameObject.DiffuseFactor[i]);
                    effect.Parameters["xAmbient"].SetValue(2*GameObject.Ambient[i]);
                    effect.Parameters["xTransparency"].SetValue(GameObject.Transparency[i]);
                    effect.Parameters["xTexture"].SetValue(GameObject.Textures[i++]);
                    
                    //Macierze
                    Vector3 unproject = gd.Viewport.Unproject(new Vector3(gd.Viewport.Width-100, 80, 0.13f), projection, 
                        temporaryCamera.CameraMatrix, Matrix.Identity);

                    effect.Parameters["xWorld"].SetValue(transforms[model.ParentBone.Index] * 
                        GameObject.GetSmallModelMatrix(unproject, gd.Viewport.Width, gameTime));

                    temporaryCamera.Reset();        

                    effect.Parameters["xView"].SetValue(temporaryCamera.CameraMatrix);
                    effect.Parameters["xProjection"].SetValue(projection);
                  //  effect.GraphicsDevice.RenderState.AlphaBlendEnable = true;
                }
                model.Draw();
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
                    if(GameObject.Textures[i] != null)      //inaczej się kurwa nie dało
                    {
                        effect.CurrentTechnique = effect.Techniques["TexturedShaded"];
                    }
                    else
                    {
                        effect.CurrentTechnique = effect.Techniques["NotRlyTexturedShaded"];
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
                    effect.Parameters["xHasTexture"].SetValue(GameObject.Textures[i]!=null?true:false);
                    effect.Parameters["xTexture"].SetValue(GameObject.Textures[i++]);
                    


                    //Macierze
                    effect.Parameters["xWorld"].SetValue(transforms[model.ParentBone.Index] * GameObject.ModelMatrix);
                    effect.Parameters["xView"].SetValue(camera.CameraMatrix);
                    effect.Parameters["xProjection"].SetValue(projection);
                   // effect.GraphicsDevice.RenderState.AlphaBlendEnable = true;
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
