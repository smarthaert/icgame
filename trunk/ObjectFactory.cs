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
        private List<double> ambient;
        private List<Vector3> specular;
        private List<double> shininess;
        private List<double> alpha;
        private List<float> specularFactor;

        public GameObjectFactory()
        {
            
        }

        public void LoadModels(Game game)
        {
            model1 = game.Content.Load<Model>("_models/firetruck");
            texture2Ds = new List<Texture2D>();
            diffuse = new List<Vector3>();
            specular = new List<Vector3>();
            ambient = new List<double>();
            alpha = new List<double>();
            specularFactor = new List<float>();
            foreach (ModelMesh mesh in model1.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    texture2Ds.Add(effect.Texture);
                    diffuse.Add(effect.Parameters["DiffuseColor"].GetValueVector3());
                    //ambient.Add(effect.Parameters["AmbientFactor"].GetValueSingle());
                    specular.Add(effect.SpecularColor);
                    //shininess.Add(effect.);
                    alpha.Add(effect.Alpha);
                    specularFactor.Add(effect.SpecularPower);
                }
            }

            foreach (ModelMesh mesh in model1.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
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
