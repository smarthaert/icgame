using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using ICGame.ObjectStats;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace ICGame
{
    /// <summary>
    /// Klasa odpowiedzialna za odczyt statystyk obiektów.
    /// </summary>
    public sealed class GameObjectStatsReader
    {
        private static GameObjectStatsReader instance;
        private XDocument xDocument;

        private GameObjectStatsReader()
        {
            xDocument = XDocument.Load("Content\\Resources\\GameObjectStats.xml");
        }

        public static GameObjectStatsReader GetStatsReader()
        {
            if(instance == null)
            {
                throw new NullReferenceException("GameObjectStatsReader was not initialized");
            }
            return instance;
        }

        public static void Initialize()
        {
            instance = new GameObjectStatsReader();
        }

        public List<string> GetObjectsToLoad()
        {
            if(xDocument == null)
            {
                throw new NullReferenceException("GameObjectStatsReader was not initialized");
            }

            List<string> objectsToLoad = new List<string>();

            foreach (XElement xElement in xDocument.Root.Elements())
            {
                objectsToLoad.Add(xElement.Attribute("Name").Value);
            }

            return objectsToLoad;
        }

        private void ParseVector3(string input, out Vector3 output)
        {
            string[] floats = input.Split(new char[] {' '});

            if(floats.Count() != 3)
            {
                throw new InvalidDataException("This is not Vector3");
            }

            output = new Vector3(float.Parse(floats[0]), float.Parse(floats[1]), float.Parse(floats[2]));
        }

        public GameObjectStats GetObjectStats(string name)
        {
            if (xDocument == null)
            {
                throw new NullReferenceException("GameObjectStatsReader was not initialized");
            }

            GameObjectStats gameObjectStats = null;
            foreach (XElement xElement in xDocument.Root.Elements())
            {
                if (xElement.Attribute("Name").Value == name)
                {
                    float fgetter;
                    int igetter;
                    bool bgetter;
                    switch (xElement.Attribute("Type").Value)
                    {
                        case "GameObject":
                            if (gameObjectStats == null)
                            {
                                gameObjectStats = new GameObjectStats();
                            }
                            gameObjectStats.Name = xElement.Attribute("Name").Value;
                            GameObjectFactory.ObjectClass objectClass;
                            GameObjectFactory.ObjectClass.TryParse(xElement.Attribute("Type").Value, out objectClass);
                            gameObjectStats.Type = objectClass;
                            foreach(XElement xel in xElement.Elements().Where(s => s.Attribute("AttributeName").Value == "Effect"))
                            {
                                gameObjectStats.Effects.Add(xel.Value);
                            }
                            foreach (XElement xel in xElement.Elements().Where(s => s.Attribute("AttributeName").Value == "SubModel"))
                            {
                                Vector3 pos, rot, sc;
                                
                                if(xel.Attribute("Position") != null)
                                {
                                    ParseVector3(xel.Attribute("Position").Value, out pos);
                                }
                                else
                                {
                                    pos = Vector3.Zero;
                                }
                                if (xel.Attribute("Rotation") != null)
                                {
                                    ParseVector3(xel.Attribute("Rotation").Value, out rot);
                                }
                                else
                                {
                                    rot = Vector3.Zero;
                                }
                                if (xel.Attribute("Scale") != null)
                                {
                                    ParseVector3(xel.Attribute("Scale").Value, out sc);
                                }
                                else
                                {
                                    sc = Vector3.One;
                                }

                                gameObjectStats.SubElements.Add(new SubElement(xel.Value, pos, rot, sc));
                            }
                            break;

                        case "Unit":
                            if (gameObjectStats == null)
                            {
                                gameObjectStats = new UnitStats();
                            }

                            float.TryParse(
                                xElement.Elements().Where(s => s.Attribute("AttributeName").Value == "Speed").First().Value, out fgetter);
                            ((UnitStats)gameObjectStats).Speed = fgetter;
                            float.TryParse(
                                xElement.Elements().Where(s => s.Attribute("AttributeName").Value == "TurnRadius").First().Value, out fgetter);
                            ((UnitStats)gameObjectStats).TurnRadius = fgetter;
                            goto case "GameObject";

                        case "Infantry":
                            if (gameObjectStats == null)
                            {
                                gameObjectStats = new InfantryStats();
                            }
                            goto case "Unit";

                        case "Vehicle":
                            if (gameObjectStats == null)
                            {
                                gameObjectStats = new VehicleStats();
                            }
                            Int32.TryParse(
                                xElement.Elements().Where(s => s.Attribute("AttributeName").Value == "FrontWheelsCount").First().Value, out igetter);
                            ((VehicleStats)gameObjectStats).FrontWheelCount = igetter;
                            Int32.TryParse(
                                xElement.Elements().Where(s => s.Attribute("AttributeName").Value == "RearWheelsCount").First().Value, out igetter);
                            ((VehicleStats)gameObjectStats).RearWheelCount = igetter;
                            Int32.TryParse(
                                xElement.Elements().Where(s => s.Attribute("AttributeName").Value == "DoorCount").First().Value, out igetter);
                            ((VehicleStats)gameObjectStats).DoorCount = igetter;
                            Boolean.TryParse(
                                xElement.Elements().Where(s => s.Attribute("AttributeName").Value == "HasTurret").First().Value, out bgetter);
                            ((VehicleStats)gameObjectStats).HasTurret = bgetter;
                            if (((VehicleStats)gameObjectStats).HasTurret)
                            {
                                Int32.TryParse(
                                    xElement.Elements().Where(s => s.Attribute("AttributeName").Value == "WaterSourceCount").First().Value, out igetter);
                                ((VehicleStats)gameObjectStats).WaterSourceCount = igetter;
                            }
                            else
                            {
                                ((VehicleStats)gameObjectStats).WaterSourceCount = 0;
                            }
                            goto case "Unit";

                        case "Building":
                            if (gameObjectStats == null)
                            {
                                gameObjectStats = new BuildingStats();
                            }
                            goto case "GameObject";

                        case "StaticObject":
                            if (gameObjectStats == null)
                            {
                                gameObjectStats = new StaticObjectStats();
                            }
                            goto case "GameObject";

                        case "Civilian":
                            if (gameObjectStats == null)
                            {
                                gameObjectStats = new CivilianStats();
                            }
                            goto case "GameObject";
                    }
                    break;
                }
            }

            if(gameObjectStats == null)
            {
                throw new ArgumentException("Invalid Object name");
            }

            return gameObjectStats;
        }
    }
}
