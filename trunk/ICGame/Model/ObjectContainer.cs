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

        public int SelectedObject
        {
            get 
            { 
                return selectedObject;
            }
            set
            {
                if (selectedObject < gameObjects.Count && selectedObject != -1)
                {
                    if(gameObjects[selectedObject] is IInteractive)
                    {
                        IInteractive interactive = gameObjects[selectedObject] as IInteractive;
                        interactive.Selected = false;
                    }
                }
                selectedObject = value;
                if (selectedObject != -1 && gameObjects[selectedObject] is IInteractive)
                {
                    IInteractive interactive = gameObjects[selectedObject] as IInteractive;
                    interactive.Selected = true;
                }
            }
        }

        public Board Board
        {
            get; private set;
        }

        public GameObjectContainer(Board board)
        {
            Board = board;
            gameObjects=new List<GameObject>();
            SelectedObject = -1;
            //TEMP
            //gi = new GameInfo();
            //\TEMP
        }

        private List<GameObject> gameObjects
        {
            get; set;
        }

        public IEnumerable<GameObject> GameObjects
        {
            get
            {
                return gameObjects;
            }
        }

        /// <summary>
        /// Dodaje obiekt i jego potomków do listy.
        /// </summary>
        /// <param name="gameObject">Obiekt do dodania</param>
        /// <param name="mission">Misja, do której obiekt jest wysyłany</param>
        public void AddGameObject(GameObject gameObject, Mission mission)
        {
            gameObject.Mission = mission;
            foreach (GameObject child in gameObject.GetChildren())
            {
                child.Mission = mission;
            }

            gameObjects.Add(gameObject);
            gameObjects.AddRange(gameObject.GetChildren());
        }

        public void InitializePathFinder()
        {
            pathFinder = new PathFinder(Board.GetDifficultyMap(), gameObjects);
        }

        public GameObject GetSelectedObject()
        {
            /*foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject is Vehicle)
                {
                    if (((Vehicle)gameObject).Selected)
                    {
                        return gameObject;
                    }
                }
            }*/
            if(SelectedObject >= 0)
            {
                return gameObjects[SelectedObject];
            }
            return null;
        }

        public void UpdateGameObjects(GameTime gameTime)
        {
            //TEMP
            //string logText = "";
            //\TEMP
            for (int i=0;i<gameObjects.Count;i++)
            {
                if (gameObjects[i] is IPhysical)
                {
                    IPhysical go = gameObjects[i] as IPhysical; 

                    float sinl = Convert.ToSingle(Math.Sin(gameObjects[i].Angle.Y) * go.Length / 2);
                    float cosl = Convert.ToSingle(Math.Cos(gameObjects[i].Angle.Y) * go.Length / 2);
                    float sinw = Convert.ToSingle(Math.Sin(gameObjects[i].Angle.Y) * go.Width / 2);
                    float cosw = Convert.ToSingle(Math.Cos(gameObjects[i].Angle.Y) * go.Width / 2);

                    //TEMP
                    //logText += sinl.ToString() + "\r\n" + cosl.ToString() + "\r\n" + sinw.ToString() + "\r\n" + cosw.ToString() + "\r\n";
                    //\TEMP

                    gameObjects[i].Position = new Vector3(gameObjects[i].Position.X,
                        (Board.GetHeight(gameObjects[i].Position.X,gameObjects[i].Position.Z)),
                        gameObjects[i].Position.Z);
                    go.AdjustToGround(
                        Board.GetHeight(
                            gameObjects[i].Position.X + sinl,
                            gameObjects[i].Position.Z + cosl
                            ),
                        Board.GetHeight(
                            gameObjects[i].Position.X - sinl,
                            gameObjects[i].Position.Z - cosl
                            ),
                        Board.GetHeight(
                            gameObjects[i].Position.X + cosw,
                            gameObjects[i].Position.Z - sinw
                            ),
                        Board.GetHeight(
                            gameObjects[i].Position.X - cosw,
                            gameObjects[i].Position.Z + sinw
                            ),
                        go.Length,
                        go.Width
                        );
                }
                else
                {
                    gameObjects[i].Position = new Vector3(gameObjects[i].Position.X,
                        (Board.GetHeight(gameObjects[i].Position.X, gameObjects[i].Position.Z)),
                        gameObjects[i].Position.Z);
                }
                //TEMP
                //logText += Convert.ToString(i) + ":\r\n";
                //logText += "Position:\t" + gameObjects[i].Position.ToString() + "\r\n";
                //logText += "Rotation:\t" + gameObjects[i].Angle.ToString() + "\r\n";
                if (gameObjects[i] is IPhysical)
                {
                    IPhysical ip = gameObjects[i] as IPhysical;
                    //logText += "BoundingBox:\t" + ip.BoundingBox.ToString() + "\r\n";
                }
                //\TEMP
            }
            //TEMP
            //logText += "Selected:\t" + SelectedObject.ToString() + "\r\n";
            //gi.ShowInfo(logText);
            //\TEMP

            UpdateEffects(gameTime);
        }
        
        public void UpdateEffects(GameTime gameTime)
        {
            foreach (GameObject gameObject in gameObjects)
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
