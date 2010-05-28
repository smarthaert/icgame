using System;
using System.Collections.Generic;
using System.Text;

namespace ICGame
{
    public class Mission
    {
        public Mission()
        {
            Board = new Board();
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
    }
}
