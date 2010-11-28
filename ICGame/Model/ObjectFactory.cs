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
        
        public enum ObjectClass
        {
            Vehicle, StaticObject, Unit, Building, GameObject, Infantry, Civilian
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
        private Dictionary<string, LoadedModel> loadedModels = new Dictionary<string, LoadedModel>();

        public void LoadModels(Game game)
        {
            GameObjectStatsReader.Initialize();

            foreach (string name in GameObjectStatsReader.GetObjectsToLoad())
            {
                loadedModels.Add(name,new LoadedModel ());
                Model tempModel = loadedModels[name].model;
                tempModel = game.Content.Load<Model>("Model/" + name);
                foreach (ModelMesh mesh in tempModel.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        loadedModels[name].textures.Add(effect.Texture);
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
                loadedModels[name].model = tempModel;
                loadedModels[name].name = name;
            }
        }

        public GameObject CreateGameObject(string name)
        {
            
            LoadedModel loadedModel = loadedModels[name];
            GameObject newObject = null;
            ObjectStats.GameObjectStats objectStats = GameObjectStatsReader.GetObjectStats(name);
            
            switch (objectStats.Type) //To zdecydowanie da sie jakos zrefaktoryzowac... Refleksja, skomplikowane rzutowanie?
            {
                case ObjectClass.Vehicle:
                    newObject = new Vehicle(loadedModel.model, objectStats as ObjectStats.VehicleStats);
                    //if (loadedModel.name == "firetruck_2")
                    //    EffectController.AddEffectToAnObject(newObject,"water"); //tak, bezsens...

       
                    (newObject as Vehicle).SelectionRing = (CreateGameObject("selection_ring") as StaticObject); //creepy
                    //Hotffix
                    if(name == "chassy_1")
                        newObject.Position = new Vector3(100, 0, 80);

                    break;
                case ObjectClass.StaticObject:
                    newObject = new StaticObject(loadedModel.model, objectStats as ObjectStats.StaticObjectStats);
                    break;
                case ObjectClass.Building:
                    newObject = new Building(loadedModel.model, objectStats as ObjectStats.BuildingStats);
                    break;
                case ObjectClass.Infantry:
                    newObject = new Infantry(loadedModel.model, objectStats as ObjectStats.InfantryStats);
                    break;
                case ObjectClass.Civilian:
                    newObject = new Civilian(loadedModel.model, objectStats as ObjectStats.CivilianStats);
                    break;
            }

            if (newObject != null)
            {
                newObject.Textures = loadedModel.textures;

                int i = 0;
                foreach (var model in newObject.Model.Meshes)
                {
                    foreach (Effect effect in model.Effects)
                    {
                        if (newObject.Textures[i++] != null)      //inaczej się kurwa nie dało
                        {
                            effect.CurrentTechnique = effect.Techniques["TexturedShaded"];
                        }
                        else
                        {
                            effect.CurrentTechnique = effect.Techniques["NotRlyTexturedShaded"];
                        }
                    }
                }

                MaterialReader materialReader = new MaterialReader(newObject, loadedModel.name);
                materialReader.PopulateObject();

                i = 0;
                foreach (var model in newObject.Model.Meshes)
                {
                    foreach (Effect effect in model.Effects)
                    {
                        //Parametry materialu
                        effect.Parameters["xAmbient"].SetValue(newObject.Ambient[i]);
                        effect.Parameters["xDiffuseColor"].SetValue(newObject.DiffuseColor[i]);
                        effect.Parameters["xDiffuseFactor"].SetValue(newObject.DiffuseFactor[i]);

                        effect.Parameters["xTransparency"].SetValue(newObject.Transparency[i]);
                        effect.Parameters["xSpecularColor"].SetValue(newObject.Specular[i]);
                        effect.Parameters["xSpecularFactor"].SetValue(newObject.SpecularFactor[i]);

                        effect.Parameters["xHasTexture"].SetValue(newObject.Textures[i] != null ? true : false);
                        effect.Parameters["xTexture"].SetValue(newObject.Textures[i++]);
                    }
                }

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
