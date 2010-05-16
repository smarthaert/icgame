using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ICGame
{
    public class GameObjectFactory
    {

        private Model model1;

        public GameObjectFactory()
        {
            
        }

        public void LoadModels(Game game)
        {
            model1 = game.Content.Load<Model>("xwing");
        }

        public GameObject CreateGameObject(GameObjectID gameObjectID)
        {

            if (gameObjectID==GameObjectID.xwing)
            {
                return new Unit(model1);
            }
            return null;
        }

        public List<GameObject> GetAvaliableUnits()
        {
            throw new System.NotImplementedException();
        }
    }
}
