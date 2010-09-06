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
            GameObject obj = gameObjectFactory.CreateGameObject(GameObjectID.Home0);
            obj.Position=new Vector3(244.0f,Board.GetHeight(244.0f,164.0f),164.0f);
            ObjectContainer.GameObjects.Add(obj);

            obj = gameObjectFactory.CreateGameObject(GameObjectID.Home0);
            obj.Position = new Vector3(274.0f, Board.GetHeight(274.0f, 134.0f), 134.0f);
            ObjectContainer.GameObjects.Add(obj);
            
        }
    }
}
