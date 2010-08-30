using System;
using System.Collections.Generic;
using System.Text;

namespace ICGame
{
    public class Campaign
    {
    
        public Campaign()
        {
            GameState = GameState.Initialize;
            UnitContainer = new UnitContainer();
            GameObjectFactory = new GameObjectFactory();
        }


        public UnitContainer UnitContainer
        {
            get; set;
        }

        public Mission Mission
        {
            get; set;
        }

        public GameObjectFactory GameObjectFactory
        {
            get; set;
        }

        public GameState GameState
        {
            get; set;

        }

        public void LoadMission()
        {
            Mission = new Mission();
            Mission.LoadObjects(GameObjectFactory);
        }

        public void BuyUnit(GameObjectID gameObjectID)
        {
            GameObject gameObject = GameObjectFactory.CreateGameObject(gameObjectID);
            if(gameObject.GetType() == typeof(Vehicle))
            {
                UnitContainer.Units.Add(gameObject as Vehicle);
            }
        }

        public void SendToMission(Unit unit)
        {
            Mission.ObjectContainer.GameObjects.Add(unit);
        }
    }
}
