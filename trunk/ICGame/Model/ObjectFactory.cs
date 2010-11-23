using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ICGame
{
    public class GameObjectFactory
    {
        private EffectController EffectController;
        public GameObjectFactory(EffectController effectController)
        {
            EffectController = effectController;
        }
        
        private enum ObjectClass
        {
            Vehicle, StaticObject, Unit, Building, GameObject, Infantry
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
                                     {GameObjectID.FireTruck,"firetruck_2"},
                                     {GameObjectID.SelectionRing,"selection_ring"},
                                     {GameObjectID.Home0,"home0"},
                                     {GameObjectID.AnimFigure, "animfigure"},
                                     {GameObjectID.Chassy, "chassy_1"}
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
                        //meshPart.Effect = game.effect.Clone(game.GraphicsDevice);
                        //CONV
                        meshPart.Effect = TechniqueProvider.GetEffect("TexturedShaded").Clone();
                    }
                }
                loadedModels[pair.Key].model = tempModel;
                loadedModels[pair.Key].name = pair.Value;

                //Temp, temp i po trzykroć temp! Należy dodac jakis plik przechowujacy informację o typie ładowanego obiektu 
                switch (pair.Value)
                {
                    case "firetruck_2":
                        loadedModels[pair.Key].objectClass = ObjectClass.Vehicle;
                        break;
                    case "selection_ring":
                        loadedModels[pair.Key].objectClass = ObjectClass.StaticObject;
                        break;
                    case "home0":
                        loadedModels[pair.Key].objectClass = ObjectClass.Building;
                        break;
                    case "animfigure":
                        loadedModels[pair.Key].objectClass = ObjectClass.Infantry;
                        break;
                    case "chassy_1":
                        loadedModels[pair.Key].objectClass = ObjectClass.Vehicle;
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
                    if(gameObjectID == GameObjectID.FireTruck)
                    {
                        newObject = new Vehicle(loadedModel.model, 0.01f, 15.0f, 4, 2, 1, true);
                    }
                    else if(gameObjectID == GameObjectID.Chassy)
                    {
                        newObject = new Vehicle(loadedModel.model, 0.01f, 15.0f, 2, 2, 2, false);
                    }
                    if (loadedModel.name == "firetruck_2")
                        EffectController.AddEffectToAnObject(newObject,"water"); //tak, bezsens...

       
                    (newObject as Vehicle).SelectionRing = (CreateGameObject(GameObjectID.SelectionRing) as StaticObject); //creepy
                    //Hotffix
                    if(gameObjectID == GameObjectID.Chassy)
                        newObject.Position = new Vector3(200, 0, 160);

                    break;
                case ObjectClass.StaticObject:
                    newObject = new StaticObject(loadedModel.model);
                    break;
                case ObjectClass.Building:
                    newObject = new Building(loadedModel.model);
                    break;
                case ObjectClass.Infantry:
                    newObject = new Infantry(loadedModel.model,0.01f,1.0f);
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
