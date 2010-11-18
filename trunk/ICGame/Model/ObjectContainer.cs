using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Point = System.Drawing.Point;

namespace ICGame
{
    public class GameObjectContainer
    {
        //TEMP
        private GameInfo gi;
        //\TEMP

        /* -1 gdy nic nie jest zaznaczone
         * w przeciwnym przypadku indeks
         * TODO:(?)multiple selecton - Poprawki również w kontrolerach, Displayu (mały model), 
         * tam jest wszystko na jedna jednostke napisane
         */
        private int selectedObject;
        private PathFinder pathFinder;

        private int SelectedObject
        {
            get 
            { 
                return selectedObject;
            }
            set
            {
                if (selectedObject < GameObjects.Count && selectedObject != -1)
                {
                    if(GameObjects[selectedObject] is IInteractive)
                    {
                        IInteractive interactive = GameObjects[selectedObject] as IInteractive;
                        interactive.Selected = false;
                    }
                }
                selectedObject = value;
                if (selectedObject != -1 && GameObjects[selectedObject] is IInteractive)
                {
                    IInteractive interactive = GameObjects[selectedObject] as IInteractive;
                    interactive.Selected = true;
                }
            }
        }

        private Board Board
        {
            get; set;
        }
        public GameObjectContainer(Board board)
        {
            Board = board;
            GameObjects=new List<GameObject>();
            SelectedObject = -1;
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
                        SelectedObject = i;
                    }
                }
            }
            if (selected == null)
            {
                SelectedObject = -1;
            }
            return selected!=null;
        }

        /// <summary>
        /// Zwracamy kliknięty obiekt 
        /// </summary>
        /// <param name="x">mouse x</param>
        /// <param name="y">mouse y</param>
        /// <param name="camera"></param>
        /// <param name="projection"></param>
        /// <param name="gd"></param>
        /// <returns>Referencje na obiekt albo null jesli nic nie kliknięto</returns>
        public GameObject CheckClickedObject(int x, int y, Camera camera, Matrix projection, GraphicsDevice gd )
        {
            foreach (GameObject gameObject in GameObjects)
            {
                if ((gameObject as IPhysical).CheckClicked(x, y, camera, projection, gd) != null)
                    return gameObject;
            }
            return null;
        }

        public void InitializePathFinder()
        {
            pathFinder = new PathFinder(Board.GetDifficultyMap(), GameObjects);
        }

        public GameObject GetSelectedObject()
        {
            foreach (GameObject gameObject in GameObjects)
            {
                if (gameObject is Vehicle)
                {
                    if (((Vehicle)gameObject).Selected)
                    {
                        return gameObject;
                    }
                }
            }
            return null;
        }

        public void UpdateGameObjects(GameTime gameTime)
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
                        (Board.GetHeight(GameObjects[i].Position.X,GameObjects[i].Position.Z)),
                        GameObjects[i].Position.Z);
                    go.AdjustToGround(
                        Board.GetHeight(
                            GameObjects[i].Position.X + sinl,
                            GameObjects[i].Position.Z + cosl
                            ),
                        Board.GetHeight(
                            GameObjects[i].Position.X - sinl,
                            GameObjects[i].Position.Z - cosl
                            ),
                        Board.GetHeight(
                            GameObjects[i].Position.X + cosw,
                            GameObjects[i].Position.Z - sinw
                            ),
                        Board.GetHeight(
                            GameObjects[i].Position.X - cosw,
                            GameObjects[i].Position.Z + sinw
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
            logText += "Selected:\t" + SelectedObject.ToString() + "\r\n";
            gi.ShowInfo(logText);
            //\TEMP

            UpdateEffects(gameTime);
        }

        public void MoveToLocation(float x, float z)
        {
            if(SelectedObject == -1)
            {
                return;
            }

            if(SelectedObject >= GameObjects.Count)
            {
                throw new ArgumentOutOfRangeException("dupa");
            }

            if(GameObjects[SelectedObject] is IControllable)
            {
                IControllable controllable = GameObjects[SelectedObject] as IControllable;

                int radius = 0;

                if(GameObjects[SelectedObject] is IPhysical)
                {
                    IPhysical physical = GameObjects[SelectedObject] as IPhysical;

                    radius =
                        Convert.ToInt32(
                            Math.Sqrt(Math.Pow(physical.Width/2.0, 2) + Math.Pow(physical.Height/2.0, 2) +
                                      Math.Pow(physical.Length/2, 2.0)));
                }

                new PathFinder().FindPath(new Point(Convert.ToInt32(GameObjects[SelectedObject].Position.X),
                                                    Convert.ToInt32(GameObjects[SelectedObject].Position.Z)),
                                          new Point(Convert.ToInt32(x),Convert.ToInt32(z)), radius, controllable);

            }
        }
        
        public void UpdateEffects(GameTime gameTime)
        {
            foreach (GameObject gameObject in GameObjects)
            {
                foreach (IObjectEffect effect in gameObject.EffectList)
                {
                    if(effect.IsActive)
                        effect.Update(gameTime);
                }
            }
        }
    }
}
