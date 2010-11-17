using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ICGame
{
    public class CampaignController
    {
        private Game MainGame;
        
        public CampaignController(Game game, MissionController missionController, EffectController effectController)
        {
            MainGame = game;
            MissionController = missionController;
            EffectController = effectController;

        }
        public void StartCampaign()
        {
            Campaign = new Campaign(EffectController);
            
            MissionController.StartMission();
            Campaign.Mission = MissionController.Mission;

            Campaign.GameObjectFactory.LoadModels(MainGame);
            Campaign.BuyUnit(GameObjectID.FireTruck);
            Campaign.BuyUnit(GameObjectID.Chassy);
            Campaign.SendToMission(Campaign.UnitContainer.Units[0]);
            Campaign.SendToMission(Campaign.UnitContainer.Units[1]);
            //Campaign.BuyUnit(GameObjectID.AnimFigure);
            //Campaign.SendToMission(Campaign.UnitContainer.Units[1]);
            //Campaign.BuyUnit(GameObjectID.FireTruck);
            //Campaign.SendToMission(Campaign.UnitContainer.Units[1]);
            MissionController.LoadMissionData(Campaign.GameObjectFactory, EffectController);
        }

        public List<GameObject> GetObjectsToDraw()
        {
            return MissionController.GetMissionObjects();
        }

        public GameState CampaignState
        {
            set
            {
                Campaign.GameState = value;
            }
            get
            {
                return Campaign.GameState;
            }
        }
     
        public BoardDrawer GetBackgroundDrawer()
        {
            //Weryfikacja Gamestate!
            return MissionController.GetBoardDrawer();
        }

        public TerrainWaterDrawer GetTerrainWaterDrawer()
        {
            return MissionController.GetTerrainWaterDrawer();
        }

        private Campaign Campaign
        {
            get; set;
        }

        public MissionController MissionController
        { 
            get; set;
        }

        public EffectController EffectController { get; set; }

        public GameObject GetActiveObject()
        {
            return MissionController.GetActiveObject();
        }

        public bool CheckSelection(int x, int y, Camera camera, Matrix projection, GraphicsDevice gd)
        {
            return MissionController.CheckSelection(x, y, camera, projection, gd);
        }
    }
}
