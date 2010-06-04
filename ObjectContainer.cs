using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace ICGame
{
    public class GameObjectContainer
    {
        private Board Board
        {
            get; set;
        }
        public GameObjectContainer(Board board)
        {
            Board = board;
            GameObjects=new List<GameObject>();
        }
        public List<GameObject> GameObjects
        {
            get; set;
        }
            

        public void UpdateGameObjects()
        {
            for (int i=0;i<GameObjects.Count;i++)
            {
                GameObjects[i].Position = new Vector3(GameObjects[i].Position.X,Board.GetHeight(GameObjects[i].Position.X,GameObjects[i].Position.Z),GameObjects[i].Position.Z);
                
            }
        }
    }
}
