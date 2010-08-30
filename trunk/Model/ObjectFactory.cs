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
                                     {GameObjectID.SelectionRing,"selection_ring"},
                                     {GameObjectID.Home0,"home0"}
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
                    case "home0":
                        loadedModels[pair.Key].objectClass = ObjectClass.Building;
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
            
            switch (loadedModel.objectClass) //To zdecydowanie da sie jakos zrefaktoryzowac... Refleksja, skomplikowane rzutowanie?
            {
                case ObjectClass.Vehicle:
                    newObject = new Vehicle(loadedModel.model, 1.0f, 6);
                    (newObject as Vehicle).SelectionRing = (CreateGameObject(GameObjectID.SelectionRing) as StaticObject); //creepy
                    break;
                case ObjectClass.StaticObject:
                    newObject = new StaticObject(loadedModel.model);
                    break;
                case ObjectClass.Building:
                    newObject = new Building(loadedModel.model);
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
          
            return null;
        }

        public List<GameObject> GetAvaliableUnits()
        {
            throw new System.NotImplementedException();
        }
    }
}
