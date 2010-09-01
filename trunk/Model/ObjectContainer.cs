using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ICGame
{
    public class GameObjectContainer
    {
        //TEMP
        private GameInfo gi;
        //\TEMP

        /* -1 gdy nic nie jest zaznaczone
         * w przeciwnym przypadku indeks
         * TODO:(?)multiple selecton
         */
        private int selectedObject;

        private Board Board
        {
            get; set;
        }
        public GameObjectContainer(Board board)
        {
            Board = board;
            GameObjects=new List<GameObject>();
            selectedObject = -1;
            //TEMP
            gi = new GameInfo();
            //\TEMP
        }
        public List<GameObject> GameObjects
        {
            get; set;
        }

        public bool CheckSelection(int x, int y, Camera camera, Matrix projection, GraphicsDevice gd)
        {
            float? selected = null;
            int i;
            for (i = 0; i < GameObjects.Count; ++i)
            {
                if (GameObjects[i] is IPhysical)
                {
                    float? check = (GameObjects[i] as IPhysical).CheckClicked(x, y, camera, projection, gd);
                    if (check != null && (selected == null || check < selected))
                    {
                        selected = check;
                        selectedObject = i;
                    }
                }
            }
            if (selected == null)
            {
                selectedObject = -1;
            }
            return selected!=null;
        }

        public void UpdateGameObjects()
        {
            //TEMP
            string logText = "";
            //\TEMP
            for (int i=0;i<GameObjects.Count;i++)
            {
                if (GameObjects[i] is IPhysical)
                {
                    IPhysical go = GameObjects[i] as IPhysical; 

                    float sinl = Convert.ToSingle(Math.Sin(GameObjects[i].Angle.Y) * go.Length / 2);
                    float cosl = Convert.ToSingle(Math.Cos(GameObjects[i].Angle.Y) * go.Length / 2);
                    float sinw = Convert.ToSingle(Math.Sin(GameObjects[i].Angle.Y) * go.Width / 2);
                    float cosw = Convert.ToSingle(Math.Cos(GameObjects[i].Angle.Y) * go.Width / 2);

                    //TEMP
                    logText += sinl.ToString() + "\r\n" + cosl.ToString() + "\r\n" + sinw.ToString() + "\r\n" + cosw.ToString() + "\r\n";
                    //\TEMP

                    GameObjects[i].Position = new Vector3(GameObjects[i].Position.X,
                        (Board.GetHeight(GameObjects[i].Position.Z,GameObjects[i].Position.X)),
                        GameObjects[i].Position.Z);
                    go.AdjustToGround(
                        Board.GetHeight(
                            GameObjects[i].Position.Z + cosl,
                            GameObjects[i].Position.X + sinl
                            ),
                        Board.GetHeight(
                            GameObjects[i].Position.Z - cosl,
                            GameObjects[i].Position.X - sinl
                            ),
                        Board.GetHeight(
                            GameObjects[i].Position.Z - sinw,
                            GameObjects[i].Position.X + cosw
                            ),
                        Board.GetHeight(
                            GameObjects[i].Position.Z + sinw,
                            GameObjects[i].Position.X - cosw
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
                //TEMP
                logText += Convert.ToString(i) + ":\r\n";
                logText += "Position:\t" + GameObjects[i].Position.ToString() + "\r\n";
                logText += "Rotation:\t" + GameObjects[i].Angle.ToString() + "\r\n";
                if (GameObjects[i] is IPhysical)
                {
                    IPhysical ip = GameObjects[i] as IPhysical;
                    logText += "BoundingBox:\t" + ip.BoundingBox.ToString() + "\r\n";
                }
                //\TEMP
            }
            //TEMP
            logText += "Selected:\t" + selectedObject.ToString() + "\r\n";
            gi.ShowInfo(logText);
            //\TEMP
        }
    }
}
