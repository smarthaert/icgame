using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ICGame
{
    /// <summary>
    /// Klasa zarzadzajaca contentem. Wydaje odpowiednie modele, tekstury
    /// </summary>
    public class GameContentManager
    {
        private ContentManager contentManager;

        /// <summary>
        /// Mapuje enumy na nazwy contentu
        /// </summary>
        public static Dictionary<GameObjectID, string> GameObjectNames = new Dictionary<GameObjectID, string>()
                                 {
                                     {GameObjectID.FireTruck,"firetruck_2"},
                                     {GameObjectID.SelectionRing,"selection_ring"},
                                     {GameObjectID.Home0,"home0"},
                                     {GameObjectID.AnimFigure, "animfigure"},
                                     {GameObjectID.Chassy, "chassy_1"}
                                 };


        /// <summary>
        /// Wewnetrzna klasa reprezetnujaca zaladowany model, nie uzywana nigdzie indziej
        /// </summary>
        private class LoadedModel
        {
            public Model model;
            public List<Texture2D> textures = new List<Texture2D>();
        }

        /// <summary>
        /// Lista zaladowanych modeli. 
        /// </summary>
        private Dictionary<GameObjectID, LoadedModel> loadedModels = new Dictionary<GameObjectID, LoadedModel>();

        /// <summary>
        /// Ladowanie wszystkich modeli. Inicjalizacja wywolywana z konstruktora
        /// </summary>
        private void LoadModels()
        {
            foreach (string name in GameObjectStatsReader.GetStatsReader().GetObjectsToLoad())
            {
                //odczytaj id obiektu na podstawie nazwy z xmla
                GameObjectID currentObjectId =
                    (from ids in GameObjectNames where ids.Value == name select ids.Key).First();

                //Utworz nowy model "ladowania"
                LoadedModel loadedModel = new LoadedModel();

                //Zaladuj odpowiedni model 3d
                loadedModel.model = contentManager.Load<Model>("Model/" + name);

                //Przepisz tesktury do modelu ladowania
                foreach (ModelMesh mesh in loadedModel.model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        loadedModel.textures.Add(effect.Texture);
                    }
                }

                //Przepisz shadery w modelu
                foreach (ModelMesh mesh in loadedModel.model.Meshes)
                {
                    foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    {
                        meshPart.Effect = TechniqueProvider.GetEffect("TexturedShaded").Clone();
                    }
                }
                
                loadedModels.Add(currentObjectId,loadedModel);
            }
        }
        
        /// <summary>
        /// NIE dla gameObjectow
        /// </summary>
        /// <param name="name">Pelna sciezka</param>
        /// <returns></returns>
        public Model GetModel(string name)
        {
            return contentManager.Load<Model>(name);    
        }

        public Model GetGameObjectModel(GameObjectID gameObjectId)
        {
            if (loadedModels.ContainsKey(gameObjectId))
                return loadedModels[gameObjectId].model;
            else
            {
                throw new Exception("Nieistniejacy model");
            }
        }

        public IEnumerable<Texture2D> GetModelTextures(GameObjectID gameObjectId)
        {
            if (loadedModels.ContainsKey(gameObjectId))
                return loadedModels[gameObjectId].textures;
            else
            {
                throw new Exception("Nieistniejacy model");
            }
        }


        public SpriteFont GetFont()
        {
            return contentManager.Load<SpriteFont>("Resources/font");
        }

        public Texture2D GetTexture(string name)
        {
            return contentManager.Load<Texture2D>(name);
        }


        #region SINGLETON
        private static GameContentManager instance;

        private GameContentManager(ContentManager contentManager)
        {
            this.contentManager = contentManager;
            LoadModels();
        }

        public static GameContentManager Content
        {
            get
            {
                if (instance == null)
                {
                    throw new Exception("Game content manager not initialized!");
                }

                return instance;
            }
        }

        public static void Initialize(ContentManager contentManager)
        {
            instance = new GameContentManager(contentManager);
        }

        #endregion




    }
}
