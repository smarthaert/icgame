using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ICGame
{
    public class Unit : GameObject, IAnimated, IPhysical, IInteractive, IDestructible, IControllable
    {
        protected Matrix[] BasicTransforms
        {
            get; set;
        }

        protected Matrix[] AnimationTransforms
        {
            get; set;
        }

        public Unit(Model model, float speed):
            base(model)
        {
            moving = Direction.None;
            turning = Direction.None;
            this.speed = speed;
            lastGameTime = new GameTime();
            moving = Direction.None;
            //TEMP
            moving = Direction.Forward;
            //\TEMP

            BasicTransforms = new Matrix[Model.Bones.Count];
            int j = 0;
            foreach (ModelBone bone in Model.Bones)
            {
                BasicTransforms[j++] = bone.Transform;
            }

            AnimationTransforms = new Matrix[Model.Bones.Count];
            for(int i = 0; i < AnimationTransforms.GetLength(0);++i)
            {
                AnimationTransforms[i] = Matrix.Identity;
            }
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

        #region IControllable Members

        public Microsoft.Xna.Framework.Vector2 Destination
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

        public Microsoft.Xna.Framework.Vector2 NextStep
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

        public void CalculateNextStep()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IAnimated Members

        public virtual void Animate(GameTime gameTime)
        {
            for(int i = 0; i < Model.Bones.Count; ++i)
            {
                Model.Bones[i].Transform = AnimationTransforms[i]*BasicTransforms[i];
            }
        }

        #endregion

        public Direction Moving
        {
            get { return moving; }
            set { moving = value; }
        }

        protected Direction moving;
        protected Direction turning;
        protected float speed;
        protected GameTime lastGameTime;
    }
}
