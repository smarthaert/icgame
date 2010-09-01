using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ICGame
{
    public class Building : GameObject, IAnimated, IPhysical, IDestructible, IInteractive 
    {
        public Building(Model model)
            : base(model)
        {
            Position = new Vector3(10,0,16);
            PhysicalTransforms = Matrix.Identity + Matrix.CreateTranslation(Position);
        }

        #region IPhysical Members

        public BoundingBox BoundingBox
        {
            get;
            set;
        }

        #endregion

        #region IDestructible Members

        public int HP
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int Damage
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Destroy()
        {
            throw new NotImplementedException();
        }

        public void Fade()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IInteractive Members

        public bool Selected
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region IAnimated Members

        public void Animate(GameTime gameTime)
        {
            
        }

        #endregion

        #region IPhysical Members


        public void AdjustToGround(float front, float back, float left, float right, float length, float width)
        {
            return;
        }

        public Matrix PhysicalTransforms
        {
            get; set;
        }

        public float Width
        {
            get; set;
        }

        public float Height
        {
            get; set;
        }

        public float Length
        {
            get; set;
        }

        public float? CheckClicked(int x, int y, Camera camera, Matrix projection, Microsoft.Xna.Framework.Graphics.GraphicsDevice gd)
        {
            return null;
        }


        #endregion
    }
}
