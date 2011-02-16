using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ICGame
{
    public class GameObjectFactory
    {
        #region SINGLETON
        private GameObjectFactory()
        {

        }

        private static GameObjectFactory instance;

        public static GameObjectFactory Factory
        {
            get
            {
                if(instance==null)
                    instance = new GameObjectFactory();
                return instance;
            }
        }

        #endregion


        public enum ObjectClass
        {
            Vehicle, StaticObject, Unit, Building, GameObject, Infantry, Civilian
        }



        public GameObject CreateGameObject(GameObjectID gameObjectId)
        {

            GameObject CreatedObject = null;
            ObjectStats.GameObjectStats objectStats = GameObjectStatsReader.GetStatsReader().GetObjectStats(GameContentManager.GameObjectNames[gameObjectId]);

            foreach (ObjectStats.SubElement subElement in objectStats.SubElements)
            {
                //Translacja name <-> GameObjectID
                GameObjectID idOfSubElement = (from ids in GameContentManager.GameObjectNames
                                               where ids.Value == subElement.Name
                                               select ids.Key).First();
                subElement.GameObject = CreateGameObject(idOfSubElement);
            }

            Model loadedModel = GameContentManager.Content.GetGameObjectModel(gameObjectId);
            switch (objectStats.Type)
            {
                case ObjectClass.Vehicle:
                    CreatedObject = new Vehicle(loadedModel, objectStats as ObjectStats.VehicleStats);

                    //Hotffix
                    if (gameObjectId == GameObjectID.Chassy)
                        CreatedObject.Position = new Vector3(100, 0, 80);

                    break;
                case ObjectClass.StaticObject:
                    CreatedObject = new StaticObject(loadedModel, objectStats as ObjectStats.StaticObjectStats);
                    break;
                case ObjectClass.Building:
                    CreatedObject = new Building(loadedModel, objectStats as ObjectStats.BuildingStats);
                    break;
                case ObjectClass.Infantry:
                    CreatedObject = new Infantry(loadedModel, objectStats as ObjectStats.InfantryStats);
                    break;
                case ObjectClass.Civilian:
                    CreatedObject = new Civilian(loadedModel, objectStats as ObjectStats.CivilianStats);
                    break;
            }

            if (CreatedObject != null)
            {
                CreatedObject.Textures = GameContentManager.Content.GetModelTextures(gameObjectId).ToList();

                int i = 0;
                foreach (var model in CreatedObject.Model.Meshes)
                {
                    foreach (Effect effect in model.Effects)
                    {
                        if (CreatedObject.Textures[i++] != null)      //inaczej się kurwa nie dało
                        {
                            effect.CurrentTechnique = effect.Techniques["TexturedShaded"];
                        }
                        else
                        {
                            effect.CurrentTechnique = effect.Techniques["NotRlyTexturedShaded"];
                        }
                    }
                }

                //Wprowadzenie danych materialu - fix buga z Pipelinem
                MaterialReader materialReader = new MaterialReader(CreatedObject, GameContentManager.GameObjectNames[gameObjectId]);
                materialReader.PopulateObject();

                i = 0;
                foreach (var model in CreatedObject.Model.Meshes)
                {
                    foreach (Effect effect in model.Effects)
                    {
                        //Parametry materialu
                        effect.Parameters["xAmbient"].SetValue(CreatedObject.Ambient[i]);
                        effect.Parameters["xDiffuseColor"].SetValue(CreatedObject.DiffuseColor[i]);
                        effect.Parameters["xDiffuseFactor"].SetValue(CreatedObject.DiffuseFactor[i]);

                        effect.Parameters["xTransparency"].SetValue(CreatedObject.Transparency[i]);
                        effect.Parameters["xSpecularColor"].SetValue(CreatedObject.Specular[i]);
                        effect.Parameters["xSpecularFactor"].SetValue(CreatedObject.SpecularFactor[i]);

                        effect.Parameters["xHasTexture"].SetValue(CreatedObject.Textures[i] != null ? true : false);
                        effect.Parameters["xTexture"].SetValue(CreatedObject.Textures[i++]);
                    }
                }

                return CreatedObject;
            }
            else
            {
                throw new Exception("Unknown unit!");
            }

            return null;
        }

    }
}
