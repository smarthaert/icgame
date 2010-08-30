using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ICGame
{
    public class GameObjectFactory
    {
        private enum ObjectClass
        {
            Vehicle, StaticObject, Unit, Building, GameObject
        }

        private class LoadedModel
        {
            public Model model;
            public List<Texture2D> textures = new List<Texture2D>();
            public string name;
            public ObjectClass objectClass;
        } 

        private Dictionary<GameObjectID, string> objectAccessList = new Dictionary<GameObjectID, string>()
                                 {
                                     {GameObjectID.FireTruck,"firetruck"},
                                     {GameObjectID.SelectionRing,"selection_ring"}
                                 };
        private Dictionary<GameObjectID, LoadedModel> loadedModels = new Dictionary<GameObjectID, LoadedModel>();

        public void LoadModels(Game game)
        {
            foreach (KeyValuePair<GameObjectID, string> pair in objectAccessList)
            {
                loadedModels.Add(pair.Key,new LoadedModel ());
                Model tempModel = loadedModels[pair.Key].model;
                tempModel = game.Content.Load<Model>("Model/"+pair.Value);
                foreach (ModelMesh mesh in tempModel.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        loadedModels[pair.Key].textures.Add(effect.Texture);
                    }
                }
                foreach (ModelMesh mesh in tempModel.Meshes)
                {
                    foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    {
                        meshPart.Effect = game.effect.Clone(game.GraphicsDevice);
                    }
                }
                loadedModels[pair.Key].model = tempModel;
                loadedModels[pair.Key].name = pair.Value;

                //Temp, temp i po trzykroć temp! Należy dodac jakis plik przechowujacy informację o typie ładowanego obiektu
                switch (pair.Value)
                {
                    case "firetruck":
                        loadedModels[pair.Key].objectClass = ObjectClass.Vehicle;
                        break;
                    case "selection_ring":
                        loadedModels[pair.Key].objectClass = ObjectClass.StaticObject;
                        break;
                    default:
                        loadedModels[pair.Key].objectClass = ObjectClass.GameObject;
                        break;
                }
            }
        }

        public GameObject CreateGameObject(GameObjectID gameObjectID)
        {
            
            LoadedModel loadedModel = loadedModels[gameObjectID];
            GameObject newObject = null;
            switch (loadedModel.objectClass)
            {
                case ObjectClass.Vehicle:
                    newObject = new Vehicle(loadedModel.model, 1.0f, 6);
                    (newObject as Vehicle).SelectionRing = (CreateGameObject(GameObjectID.SelectionRing) as StaticObject); //creepy
                    break;
                case ObjectClass.StaticObject:
                    newObject = new StaticObject(loadedModel.model);
                    break;
            }

            if (newObject != null)
            {
                newObject.Textures = loadedModel.textures;
                MaterialReader materialReader = new MaterialReader(newObject, loadedModel.name);
                materialReader.PopulateObject();
                return newObject;
            }
            else
            {
                throw new Exception("Unknown unit!");
            }
          

          /*  if (gameObjectID==GameObjectID.FireTruck)
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

                MaterialReader mr = new MaterialReader(unit,"firetruck");
                mr.PopulateObject(); //Arise my minion!
                unit.SelectionRing = (StaticObject) this.CreateGameObject(GameObjectID.SelectionRing);

                return unit;
            }
            else if (gameObjectID==GameObjectID.SelectionRing)
            {
                StaticObject selectionRing = new StaticObject(model2);

                selectionRing.Textures = texture2Ds2;
                MaterialReader mr=new MaterialReader(selectionRing,"selection_ring");
                mr.PopulateObject(); //...just like dr Frankenstein...\
                return selectionRing;
            }*/
            return null;
        }

        public List<GameObject> GetAvaliableUnits()
        {
            throw new System.NotImplementedException();
        }
    }
}
