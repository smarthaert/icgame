using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ICGame
{
    public abstract class GameObject : IDrawable
    {
        
        #region IDrawable Members

        public GameObject(Model model)
        {
            Model = model;
        }
        public virtual GameObjectDrawer GetDrawer()
        {
            return new GameObjectDrawer(this);
        }


        public void Update()
        {
            
        }


        public Matrix ModelMatrix
        {
            get
            {
                return Matrix.CreateScale(0.0005f, 0.0005f, 0.0005f) * Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateTranslation(new Vector3(19, 12, -5));
            }
            

        }

        public Model Model
        {
            get; set;
        }

        #endregion
    }
}
