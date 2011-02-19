using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ICGame
{
    /// <summary>
    /// Interpretuje klikniecia myszą i wydaje rozkazy jednostkom
    /// </summary>
    public class UnitCommander
    {
        /// <summary>
        /// Obsluga lewego przycisku myszy
        /// </summary>
        /// <param name="clickedPoint">Kliknięty punkt</param>
        /// <param name="objectContainer">ObjectContainer aktualnej misji</param>
        /// <param name="device"></param>
        public void ToggleLeftClick(Point clickedPoint, GameObjectContainer objectContainer, GraphicsDevice device)
        {
            CheckSelection(clickedPoint.X, clickedPoint.Y, objectContainer, device);
        }

        /// <summary>
        /// Obsluga prawego przycisku myszy
        /// </summary>
        /// <param name="clickedPoint">Kliknięty punkt</param>
        /// <param name="objectContainer">ObjectContainer aktualnej misji</param>
        /// <param name="device"></param>
        public void ToggleRightClick(Point clickedPoint, GameObjectContainer objectContainer, GraphicsDevice device)
        {
            Vector3 pos3D = objectContainer.Board.GetPosition(clickedPoint.X, clickedPoint.Y);
            //Zakapsulkowac To!
            GameObject pointedObject = CheckClickedObject(clickedPoint.X, clickedPoint.Y, objectContainer, device);
            if (pointedObject != null)
            {
                if (objectContainer.GetSelectedObject() != null && (pointedObject is Building))
                {
                    (objectContainer.GetSelectedObject() as Vehicle).PointTurretToGameObject(pointedObject);
                    (objectContainer.GetSelectedObject() as Vehicle).ActivateSpecialAction();

                }
            }
            else
            {
                MoveToLocation(pos3D.X, pos3D.Z, objectContainer);
            }
        }

        /// <summary>
        /// Sprawdza, czy kliknieto obiekt, jesli tak zaznacza go
        /// </summary>
        /// <param name="x">mouse x</param>
        /// <param name="y">mouse y</param>
        /// <param name="gd"></param>
        /// <returns>Czy kliknieto obiekt</returns>
        private bool CheckSelection(int x, int y, GameObjectContainer objectContainer, GraphicsDevice gd)
        {
            float? selected = null;
            int i;
            for (i = 0; i < objectContainer.GameObjects.Count(); ++i)
            {
                if (objectContainer.GameObjects.ElementAt(i) is IPhysical)
                {
                    float? check = (objectContainer.GameObjects.ElementAt(i) as IPhysical).CheckClicked(x, y, gd);
                    if (check != null && (selected == null || check < selected))
                    {
                        selected = check;
                        objectContainer.SelectedObject = objectContainer.GameObjects.ToList().IndexOf(objectContainer.GameObjects.ElementAt(i).RootObject);
                    }
                }
            }
            if (selected == null)
            {
                objectContainer.SelectedObject = -1;
            }
            return selected != null;
        }

        /// <summary>
        /// Zwracamy kliknięty obiekt 
        /// </summary>
        /// <param name="x">mouse x</param>
        /// <param name="y">mouse y</param>
        /// <param name="gd"></param>
        /// <returns>Referencje na obiekt albo null jesli nic nie kliknięto</returns>
        private GameObject CheckClickedObject(int x, int y, GameObjectContainer objectContainer, GraphicsDevice gd)
        {
            foreach (GameObject gameObject in objectContainer.GameObjects)
            {
                if (gameObject is IPhysical)
                {
                    if ((gameObject as IPhysical).CheckClicked(x, y, gd) != null)
                        return gameObject.RootObject;
                }
            }
            return null;
        }

        private void MoveToLocation(float x, float z, GameObjectContainer objectContainer)
        {
            if (objectContainer.SelectedObject == -1)
            {
                return;
            }

            if (objectContainer.SelectedObject >= objectContainer.GameObjects.Count())
            {
                throw new ArgumentOutOfRangeException("dupa");
            }

            if (objectContainer.GameObjects.ElementAt(objectContainer.SelectedObject) is IControllable)
            {
                IControllable controllable = objectContainer.GameObjects.ElementAt(objectContainer.SelectedObject) as IControllable;

                int radius = 0;

                if (objectContainer.GameObjects.ElementAt(objectContainer.SelectedObject) is IPhysical)
                {
                    IPhysical physical = objectContainer.GameObjects.ElementAt(objectContainer.SelectedObject) as IPhysical;

                    radius =
                        Convert.ToInt32(
                            Math.Sqrt(Math.Pow(physical.Width / 2.0, 2) + Math.Pow(physical.Height / 2.0, 2) +
                                      Math.Pow(physical.Length / 2, 2.0)));
                }

                new PathFinder().FindPath(
                    new System.Drawing.Point(Convert.ToInt32(objectContainer.GameObjects.ElementAt(objectContainer.SelectedObject).Position.X),
                    Convert.ToInt32(objectContainer.GameObjects.ElementAt(objectContainer.SelectedObject).Position.Z)),
                    new System.Drawing.Point(Convert.ToInt32(x), Convert.ToInt32(z)), radius, controllable, objectContainer.GameObjects);

            }
        }
    }
}
