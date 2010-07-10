using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ICGame
{
    public class GameObjectFactory
    {

        private Model model1;
        private List<Texture2D> texture2Ds;
        private List<Vector3> diffuse;
        private List<float> diffuseFactor;
        private List<float> opacity;
        private List<float> transparency;
        private List<Vector3> specular;
        private List<float> specularFactor;

        public GameObjectFactory()
        {
            
        }

        public void LoadModels(Game game)
        {
            model1 = game.Content.Load<Model>("firetruck");
            texture2Ds = new List<Texture2D>();
            diffuse=new List<Vector3>();
            diffuseFactor=new List<float>();
            opacity = new List<float>();
            transparency = new List<float>();
            specular = new List<Vector3>(); 
            specularFactor = new List<float>();

       
            foreach (ModelMesh mesh in model1.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    texture2Ds.Add(effect.Texture);
                }
            }

            foreach (ModelMesh mesh in model1.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    diffuse.Add(meshPart.Effect.Parameters["DiffuseColor"].GetValueVector3());
                    if (meshPart.Effect.Parameters["DiffuseFactor"]!=null)
                    diffuseFactor.Add(meshPart.Effect.Parameters["DiffuseFactor"].GetValueSingle());
                    else 
                        diffuseFactor.Add(1.0f);
                    if (meshPart.Effect.Parameters["Alpha"] != null)
                        transparency.Add(meshPart.Effect.Parameters["Alpha"].GetValueSingle());
                    else
                    {
                        transparency.Add(1.0f);
                    }
                  //  if (meshPart.Effect.Parameters["SpecularPower"] != null)
                        //specularFactor.Add(meshPart.Effect.Parameters["SpecularPower"].GetValueSingle());
        //                meshPart.Effect.Parameters["SpecularPower"].GetValueString();
                  //  else
                   // {
                    //    specularFactor.Add(0.0f);
                   // }
                    if (meshPart.Effect.Parameters["SpecularColor"] != null)
                        specular.Add(meshPart.Effect.Parameters["SpecularColor"].GetValueVector3());
                    else
                    {
                        specular.Add(new Vector3(1.0f,1.0f,1.0f));
                    }
                    

                    meshPart.Effect = game.effect.Clone(game.GraphicsDevice);
                    
                }
                
            }
        }

        public GameObject CreateGameObject(GameObjectID gameObjectID)
        {

            if (gameObjectID==GameObjectID.FireTruck)
            {
                Vehicle unit = new Vehicle(model1,1.0f, 6);
                unit.Textures = texture2Ds;
                unit.DiffuseColor = diffuse;
                unit.DiffuseFactor = diffuseFactor;
                unit.Transparency = transparency;
                unit.Specular = specular;
                unit.SpecularFactor = specularFactor;

                IDrawable i = unit;
                i.GetDrawer();


                return unit;
            }
            return null;
        }

        public List<GameObject> GetAvaliableUnits()
        {
            throw new System.NotImplementedException();
        }
    }
}
