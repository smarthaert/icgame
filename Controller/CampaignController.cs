using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICGame
{
    public class CampaignController
    {
        private Game MainGame;
        public CampaignController(Game game, MissionController missionController)
        {
            MainGame = game;
            MissionController = missionController;

        }
        public void StartCampaign()
        {
            Campaign = new Campaign();
            
            MissionController.StartMission();
            Campaign.Mission = MissionController.Mission;

            Campaign.GameObjectFactory.LoadModels(MainGame);
            Campaign.BuyUnit(GameObjectID.FireTruck);
            Campaign.SendToMission(Campaign.UnitContainer.Units[0]);
            MissionController.LoadMissionData(Campaign.GameObjectFactory);
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

        private Campaign Campaign
        {
            get; set;
        }

        private MissionController MissionController
        { 
            get; set;
        }

        public GameObject GetActiveObject()
        {
            return MissionController.GetActiveObject();
        }
    }
}
