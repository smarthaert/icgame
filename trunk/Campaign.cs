using System;
using System.Collections.Generic;
using System.Text;

namespace ICGame
{
    public class Campaign
    {
    
        public Campaign()
        {
            
        }
    

        public UnitContainer UnitContainer
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public Mission Mission
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public GameObjectFactory ObjectFactory
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public GameState GameState
        {
            get; set;

        }

        public void LoadMission()
        {
            throw new System.NotImplementedException();
        }

        public void BuyUnit(GameObjectID gameObjectID)
        {
            throw new System.NotImplementedException();
        }

        public void SendToMission(Unit unit)
        {
            throw new System.NotImplementedException();
        }
    }
}
