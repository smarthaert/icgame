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
        private static float angle = 0;
        public GameObjectDrawer(GameObject gameObject)
        {
            GameObject = gameObject;
        }

        public GameObject GameObject
        {
            get; set;
        }

        public virtual void Draw(Matrix projection, Matrix view)
        {
            Matrix[] transforms = new Matrix[GameObject.Model.Bones.Count];
            GameObject.Model.CopyAbsoluteBoneTransformsTo(transforms);
            transforms[GameObject.Model.Bones["rueda_1"].Index] = Matrix.CreateRotationX(angle)*
                                                            transforms[GameObject.Model.Bones["rueda_1"].Index];
            angle += 0.1f;
            int i = 0;
            foreach (var model in GameObject.Model.Meshes)
            {
                foreach (Effect effect in model.Effects)
                {
                    effect.CurrentTechnique = effect.Techniques["Textured"];
                    effect.Parameters["xTexture"].SetValue(GameObject.Textures[i++]);
                    effect.Parameters["xWorld"].SetValue(transforms[model.ParentBone.Index] * GameObject.ModelMatrix);
                    effect.Parameters["xView"].SetValue(view);
                    effect.Parameters["xProjection"].SetValue(projection);
                }
                model.Draw();
            }
        }
    }
}
