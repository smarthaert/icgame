using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using ICGame.ObjectStats;
using ICGame.Tools;
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
        protected const float scale = 0.005f;

        public Vector3 Scale { get; set; }
        private Vector3 position;
        private Model _model;
        //temp
        private float rot;
        private ArrayList meshesNames=new ArrayList();
        public GameObject Parent { get; set; }
        protected List<GameObject> children;

        public delegate void VectorEventHandler(object sender, VectorEventArgs e);

        public delegate void BoolEventHandler(object sender, BoolEventArgs e);

        #region IDrawable Members

        public GameObject(Model model, GameObjectStats objectStats)
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

            Scale = Vector3.One;

            position = new Vector3(105, 0, 80);

            EffectList = new List<IObjectEffect>();
            
            foreach (string effectName in objectStats.Effects)
            {
                IObjectEffect effect = EffectFactory.GetEffectByName(effectName);
                effect.GameObject = this;
                EffectList.Add(effect);
            }

            //tworzymy obiekty zależne od aktualnie ładowanego...

            if(children == null)
            {
                children = new List<GameObject>();
            }

            foreach (SubElement subElement in objectStats.SubElements)
            {
                GameObject gameObject = subElement.GameObject;
                gameObject.Position = subElement.Position;
                gameObject.Angle = subElement.Rotation;
                gameObject.Scale = subElement.Scale;
                gameObject.Parent = this;

                children.Add(gameObject);
            }

            visible = true;
            
        }

        private bool visible;
        
        public bool Visible
        {
            get
            {
                return visible;
            } 
            set
            {
                foreach (GameObject child in children)
                {
                    child.Visible = value;
                }
                visible = value;
            }
        }

        public virtual GameObjectDrawer GetDrawer()
        {
            return new GameObjectDrawer(this);
        }


        public void Update()
        {
            
        }

        public List<GameObject> GetChildren()
        {
            if(children == null)
            {
                return new List<GameObject>();
            }

            else
            {
                List<GameObject> result = new List<GameObject>();

                result.AddRange(children);
                foreach (GameObject gameObject in children)
                {
                    result.AddRange(gameObject.GetChildren());
                }
                return result;
            }
        }

        public GameObject RootObject
        {
            get
            {
                if(Parent == null)
                {
                    return this;
                }
                else
                {
                    return Parent.RootObject;
                }
            }
        }


        private Matrix ModelMatrix
        {
            get
            {
                Matrix result = Matrix.Identity;
                if(Parent == null)
                {
                    result *= Matrix.CreateScale(scale,scale,scale) * Matrix.CreateRotationZ(-MathHelper.PiOver2);
                }
                else
                {
                    result *= Matrix.CreateScale(Scale);
                }
                if (this is IPhysical)
                {
                    result *= ((IPhysical)this).PhysicalTransforms;// *result;
                }
				result *= Matrix.CreateRotationX(Angle.X) * Matrix.CreateRotationY(Angle.Y) * Matrix.CreateRotationZ(Angle.Z);
                result *= Matrix.CreateTranslation(Position);
                return result;
            }
        }

        public Matrix AbsoluteModelMatrix
        {
            get
            {
                if(Parent == null)
                {
                    return ModelMatrix;
                }
                else
                {
                    return ModelMatrix*Parent.AbsoluteModelMatrix;
                }
            }
        }

        public Matrix GetAbsoluteSmallModelMatrix(Vector3 newPosition, int screenWidth, GameTime gameTime)
        {
            if(Parent == null)
            {
                return GetSmallModelMatrix(newPosition, screenWidth, gameTime);
            }
            else
            {
                return ModelMatrix*Parent.GetAbsoluteSmallModelMatrix(newPosition, screenWidth, gameTime);
            }
        }

        //TODO: WYWALIC DO OSODNEGO MODELU ASAP! //coś za szybko to ASAP nie nadchodzi ;)
        private Matrix GetSmallModelMatrix(Vector3 newPosition, int screenWidth, GameTime gameTime)
        {
            Matrix result = Matrix.Identity;
            if(Parent == null)
            {
                result *= Matrix.CreateScale(scale / (screenWidth / 16), scale / (screenWidth / 16), scale / (screenWidth / 16)) * Matrix.CreateRotationZ(-MathHelper.PiOver2);
            }
            else
            {
                result *= Matrix.CreateScale(Scale);
            }

            rot += gameTime.ElapsedGameTime.Milliseconds*0.00004f;
            result *=Matrix.CreateRotationY(rot)*Matrix.CreateRotationX(-MathHelper.PiOver4);
            result *= Matrix.CreateTranslation(newPosition);

            return result;
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

        public event VectorEventHandler PositionChanged;

        public Vector3 Position
        {
            get
            {
                return position;
            } 
            set
            {
                position = new Vector3(value.X,value.Y,value.Z);
                PositionChanged.Invoke(this, new VectorEventArgs(value));
            }
        }

        public ArrayList MeshesNames
        {
            get
            {
                return meshesNames;
            }
        }

        private Vector3 angle;
        public event VectorEventHandler AngleChanged;

        public Vector3 Angle
        {
            get
            {
                return angle;
            }
            set
            {
                AngleChanged.Invoke(this, new VectorEventArgs(value));
                angle = value;
            }
        }

        public List<IObjectEffect> EffectList
        {
            get; set;
        }

        public Mission Mission       //jak jest lepszy pomysł na przekazanie Board'a to jestem otwarty na propozycje
        {                            //_LOL_ :D
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

        public List<IObjectEffect> GetEffectsToDraw()
        {
            List<IObjectEffect> result = new List<IObjectEffect>();
            foreach (IObjectEffect objectEffect in EffectList)
            {
                if(objectEffect.IsActive)
                    result.Add(objectEffect);
            }
            return result;
        }

        public IEnumerable<IParticleEffectDrawer> GetEffectDrawers()
        {
            List<IParticleEffectDrawer> drawers = new List<IParticleEffectDrawer>();
            foreach (IObjectEffect objectEffect in EffectList)
            {
                drawers.Add(objectEffect.GetDrawer());
            }
            return drawers;
        }
    }
}
