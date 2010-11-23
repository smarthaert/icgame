using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ICGame
{
    public class MissionController
    {
        private Game MainGame;
        
        public MissionController(Game game)
        {
            MainGame = game;
        }

        public Mission Mission
        {
            get; set;
        }

        public void UpdateMission(GameTime gameTime)
        {
            Mission.Update(gameTime);
           
            
        }
        
        public void StartMission()
        {
            Mission = new Mission(MainGame);
            
        }

        public void LoadMissionData(GameObjectFactory gameObjectFactory, EffectController effectController)
        {
            Microsoft.Xna.Framework.Content.ContentManager contentManager = MainGame.Content;
            Texture2D heightMap = contentManager.Load<Texture2D>("Resources/heightmap");

            Mission.Board.LoadTextures(contentManager.Load<Texture2D>("Texture2D/Board/grass"),
                contentManager.Load<Texture2D>("Texture2D/Board/rock"),
                contentManager.Load<Texture2D>("Texture2D/Board/sand"),
                contentManager.Load<Texture2D>("Texture2D/Board/snow")
                );
            Mission.Board.PrepareSkyDome(MainGame.Content.Load<Model>("Resources/dome"), MainGame.GraphicsDevice);
            Mission.Board.LoadHeightData(heightMap);
            Mission.Board.PrepareTerrainWater(MainGame.Display.Camera, MainGame.Display.Projection);
            Mission.Board.TerrainWater.LoadTextures(contentManager.Load<Texture2D>("Texture2D/Board/waterbump"));
            Mission.LoadObjects(gameObjectFactory, effectController);
        }
        /// <summary>
        /// Do wywalenia w przyszłości
        /// </summary>
        /// <returns></returns>
        public List<GameObject> GetMissionObjects()
        {
            return Mission.ObjectContainer.GameObjects;
        }

        public GameObject GetSeletedObject()
        {
            return Mission.ObjectContainer.GetSelectedObject();
        }

        public BoardDrawer GetBoardDrawer()
        {
            return Mission.Board.GetDrawer();
        }

        public TerrainWaterDrawer GetTerrainWaterDrawer()
        {
            return Mission.Board.TerrainWater.GetDrawer();
        }

        /// <summary>
        /// Zwraca aktualnie aktywną jednostkę. UWAGA: TEMP, zwraca pierwszą jednostkę z listy aktualnej 
        /// misji, powinno być zastąpione przez GetSelectedObject i zwracać również inne zaznaczone 
        /// jednostki
        /// </summary>
        /// <returns>Zaznaczona jednostka</returns>

        public GameObject GetActiveObject()
        {
            return GetMissionObjects()[0];
        }

        public bool CheckSelection(int x, int y, Camera camera, Matrix projection, GraphicsDevice gd)
        {
            bool selected = Mission.ObjectContainer.CheckSelection(x, y, camera, projection, gd);
            return selected;
        }
    }
}
