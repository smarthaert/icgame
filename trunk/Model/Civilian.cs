using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ICGame
{
    public class Civilian : GameObject, IAnimated, IPhysical, IDestructible, IInteractive
    {
        public Civilian(Model model)
            : base(model)
        {
            
        }
        #region IPhysical Members

        public BoundingBox BoundingBox
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

        public void Destroy()
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

        #region IDestructible Members


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

        public void Fade()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IAnimated Members

        public void Animate(GameTime gameTime, List<GameObject> gameObjects)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IPhysical Members


        public void AdjustToGround(float front, float back, float left, float right, float length, float width)
        {
            throw new NotImplementedException();
        }

        public Matrix PhysicalTransforms
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

        public float Width
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

        public float Height
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

        public float Length
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

        public float? CheckClicked(int x, int y, Camera camera, Matrix projection, Microsoft.Xna.Framework.Graphics.GraphicsDevice gd)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IInteractive Members


        public bool CheckMove(IPhysical physical, BoundingBox thisBB, GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public bool CheckMoveList(Direction directionFB, Direction directionLR, List<GameObject> gameObjects, GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
