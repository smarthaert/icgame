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


                   // string ble = (string)meshPart.Tag;
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
                //unit.DiffuseColor = diffuse;
                //unit.DiffuseFactor = diffuseFactor;
                //unit.Transparency = transparency;
                //unit.Specular = specular;
                //unit.SpecularFactor = specularFactor;

                IDrawable i = unit;
                i.GetDrawer();

                MaterialReader mr = new MaterialReader(unit);
                mr.PopulateObject(); //Arise my minion!
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
