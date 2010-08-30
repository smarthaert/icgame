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
                if (GameObjects[i] is IPhysical)
                {
                    IPhysical go = GameObjects[i] as IPhysical; 
                    /*float tl = Board.GetHeight(GameObjects[i].Position.X + (go.BoundingBox.Max.X - go.BoundingBox.Min.X) / 2, 
                        GameObjects[i].Position.Z + (go.BoundingBox.Max.Z - go.BoundingBox.Min.Z) / 2);
                    float dl = Board.GetHeight(GameObjects[i].Position.X + (go.BoundingBox.Max.X - go.BoundingBox.Min.X) / 2,
                        GameObjects[i].Position.Z - (go.BoundingBox.Max.Z - go.BoundingBox.Min.Z) / 2);
                    float tr = Board.GetHeight(GameObjects[i].Position.X - (go.BoundingBox.Max.X - go.BoundingBox.Min.X) / 2,
                        GameObjects[i].Position.Z + (go.BoundingBox.Max.Z - go.BoundingBox.Min.Z) / 2);
                    GameObjects[i].Position = new Vector3(GameObjects[i].Position.X,
                        (tl +
                        dl +
                        tr +
                        Board.GetHeight(GameObjects[i].Position.X - (go.BoundingBox.Max.X - go.BoundingBox.Min.X) / 2,
                        GameObjects[i].Position.Z - (go.BoundingBox.Max.Z - go.BoundingBox.Min.Z) / 2)
                        )/4,
                        GameObjects[i].Position.Z);
                    go.AdjustToGround(tl, tr, dl);*/
                    GameObjects[i].Position = new Vector3(GameObjects[i].Position.X,
                        (Board.GetHeight(GameObjects[i].Position.Z,GameObjects[i].Position.X)),
                        GameObjects[i].Position.Z);
                    go.AdjustToGround(
                        Board.GetHeight(
                            GameObjects[i].Position.Z + go.Length / 2,
                            GameObjects[i].Position.X
                            ),
                        Board.GetHeight(
                            GameObjects[i].Position.Z - go.Length / 2,
                            GameObjects[i].Position.X
                            ),
                        Board.GetHeight(
                            GameObjects[i].Position.Z,
                            GameObjects[i].Position.X + go.Width / 2
                            ),
                        Board.GetHeight(
                            GameObjects[i].Position.Z,
                            GameObjects[i].Position.X - go.Width / 2
                            ),
                        go.Length,
                        go.Width
                        );
                }
                else
                {
                    GameObjects[i].Position = new Vector3(GameObjects[i].Position.X,
                        (Board.GetHeight(GameObjects[i].Position.X, GameObjects[i].Position.Z)),
                        GameObjects[i].Position.Z);
                }
                
            }
        }
    }
}
