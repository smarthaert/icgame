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

        public virtual void Draw(Matrix projection, Matrix view)
        {
            foreach (var model in GameObject.Model.Meshes)
            {
                foreach (var effect in model.Effects)
                {
                    effect.CurrentTechnique = effect.Techniques["Colored"];
                    effect.Parameters["xWorld"].SetValue(GameObject.ModelMatrix);
                    effect.Parameters["xView"].SetValue(view);
                    effect.Parameters["xProjection"].SetValue(projection);
                }
                model.Draw();
            }
        }
    }
}
