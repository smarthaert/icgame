using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public void UpdateMission()
        {
            Mission.Update();
            
        }
        
        public void StartMission()
        {
            Mission = new Mission();
            
        }

        public void LoadMissionData(GameObjectFactory gameObjectFactory)
        {
            Texture2D heightMap = MainGame.Content.Load<Texture2D>("Resources/heightmap");
            Mission.Board.LoadHeightData(heightMap);
            Mission.LoadObjects(gameObjectFactory);
        }

        public List<GameObject> GetMissionObjects()
        {
            return Mission.ObjectContainer.GameObjects;
        }

        public BoardDrawer GetBoardDrawer()
        {
            return Mission.Board.GetDrawer();
        }

        public GameObject GetActiveObject()
        {
            return GetMissionObjects()[0];
        }
    }
}
