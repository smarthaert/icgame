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
            ObjectContainer.UpdateGameObjects(gameTime);
            Board.SkyDome.GeneratePerlinNoise(time);
        }

        public void LoadObjects(GameObjectFactory gameObjectFactory)
        {
            GameObject obj = gameObjectFactory.CreateGameObject(GameObjectID.Home0);
            obj.Position=new Vector3(122.0f,Board.GetHeight(122.0f,82.0f),82.0f);
            obj.Mission = this;
            ObjectContainer.AddGameObject(obj, this);
            obj = gameObjectFactory.CreateGameObject(GameObjectID.Home0);
            obj.Position = new Vector3(137.0f, Board.GetHeight(137.0f, 67.0f), 67.0f);
            obj.Mission = this;
            ObjectContainer.AddGameObject(obj, this);

            ObjectContainer.InitializePathFinder();
        }
    }
}
