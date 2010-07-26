using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ICGame
{
    public enum Direction
    {
        Forward,Backward,Left,Right,None
    }
    
    public abstract class GameObject : IDrawable
    {
        protected const float scale = 0.01f;

        private Vector3 position;
        private Model _model;

        private ArrayList meshesNames=new ArrayList();

        #region IDrawable Members

        public GameObject(Model model)
        {
            Model = model;
            Ambient = new List<float>();
            Opacity=new List<float>();
            Transparency=new List<float>();
            DiffuseColor=new List<Vector3>();
            DiffuseFactor=new List<float>();
            Specular=new List<Vector3>();
            SpecularFactor=new List<float>();
            Shininess = new List<Vector3>();
            Opacity = new List<float>();
            

            Position = new Vector3(0, 0, 0);
            
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
                Matrix result = Matrix.Identity;
                result *= Matrix.CreateScale(scale,scale,scale) /** Matrix.CreateRotationY(3*MathHelper.PiOver2)*/ * Matrix.CreateRotationZ(-MathHelper.PiOver2);// *result;
                if (this is IPhysical)
                {
                    result *= ((IPhysical)this).PhysicalTransforms;// *result;
                }
                result *= Matrix.CreateTranslation(Position);
                return result;
            }
            

        }

        public Model Model
        {
            get
            {
                return _model;
            }
               
            set
            {
                _model = value;
                foreach (ModelMesh mesh in value.Meshes)
                {
                    if (mesh.Tag!=null)
                        MeshesNames.Add(mesh.Tag.ToString().Split('_')[0]);

                }
            }
        }

        public List<Texture2D> Textures
        { 
            get; set;
        }

        #endregion

        public Vector3 Position
        {
            get
            {
                return position;
            } 
            set
            {
                position = new Vector3(value.X,value.Y,value.Z);
            }
        }

        public ArrayList MeshesNames
        {
            get
            {
                return meshesNames;
            }
        }
        public Vector3 Angle
        {
            get;
            set;
        }

        #region IDrawable Members

        public List<float> Opacity
        {
            get; set;
        }

        public List<float> Transparency
        {
            get; set;
        }


        public List<Vector3> DiffuseColor
        {
            get;
            set;
        }
        
        public List<float> DiffuseFactor
        {
            get;
            set;
        }

        public List<float> Ambient
        {
            get;
            set;
        }

        public List<Vector3> Specular
        {
            get;
            set;
        }

        public List<Vector3> Shininess
        {
            get;
            set;
        }

        public List<float> SpecularFactor
        {
            get;
            set;
        }

        #endregion

     
    }
}
