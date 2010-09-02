using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace ICGame
{
    public class Mission
    {
        public Mission(Game game)
        {
            Board = new Board(game);
            ObjectContainer=new GameObjectContainer(Board);
        }
       
        public GameObjectContainer ObjectContainer
        {
            get; set;
        }

        public Board Board
        {
            get; set;
        }

        public void Update(GameTime gameTime)
        {
            float time = (float) gameTime.TotalGameTime.TotalMilliseconds/100.0f;
            ObjectContainer.UpdateGameObjects();
            Board.SkyDome.GeneratePerlinNoise(time);
        }

        public void LoadObjects(GameObjectFactory gameObjectFactory)
        {
            ObjectContainer.GameObjects.Add(gameObjectFactory.CreateGameObject(GameObjectID.Home0));
        }
    }
}
