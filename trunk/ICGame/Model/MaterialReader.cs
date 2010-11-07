using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;

namespace ICGame
{
    /// <summary>
    /// To jest bardzo brzydka klasa i BĘDZIE zrefaktoryzowana. Ale są wakacje więc na razie MA DZIAŁAć
    /// </summary>
    class MaterialReader
    {
        private XDocument xml= new XDocument();
        private GameObject gameObject;

        /// <summary>
        /// Pobiera wektor z XMLa dla danej wartości materiału. Bug z parsowaniem stringów do floatow - zalatany dla polskiej wersji - wywali sie na USA
        /// </summary>
        /// <param name="materialName"></param>
        /// <param name="attributeName"></param>
        /// <returns>Wektor z wartością</returns>
        public Vector3 getVector(string materialName, string attributeName)
        {
            var result = from mat in xml.Descendants("material") select mat;
            
            foreach (XElement element in result)
            {
                if (element.Attribute("name").Value ==materialName)
                {
                   var subresult = from prop in element.Descendants(attributeName) select prop;
                    foreach (XElement xElement in subresult)
                    {
                       string data= xElement.Value;
                       data= data.Trim();
                       string[] elements= data.Split(' ');
                       return new Vector3(float.Parse(elements[0].Replace('.', ','), NumberStyles.Float), float.Parse(elements[1].Replace('.', ',')), float.Parse(elements[2].Replace('.', ',')));
                    }
                }
            }
            return new Vector3();
        }
        /// <summary>
        /// Pobiera floata z XMLa dla danej wartości materiału. Bug z parsowaniem stringów do floatow - zalatany dla polskiej wersji - wywali sie na USA
        /// </summary>
        /// <param name="materialName"></param>
        /// <param name="attributeName"></param>
        /// <returns>float albo -1 jak lipa</returns>
        public float getFloat(string materialName, string attributeName)
        {
            var result = from mat in xml.Descendants("material") select mat;

            foreach (XElement element in result)
            {
                if (element.Attribute("name").Value == materialName)
                {
                    var subresult = from prop in element.Descendants(attributeName) select prop;
                    foreach (XElement xElement in subresult)
                    {
                        string data = xElement.Value;
                        data = data.Trim();
                        string[] elements = data.Split(' ');
                        return float.Parse(elements[0].Replace('.', ','), NumberStyles.Float);
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// Idzcie parametry materialu i zaludniajcie obiekty stworzone przez programistow!
        /// </summary>
        public void PopulateObject()
        {
            foreach (string name in gameObject.MeshesNames)
            {
                gameObject.Ambient.Add(getFloat(name,"ambient"));
                gameObject.DiffuseColor.Add(getVector(name, "color"));
                gameObject.DiffuseFactor.Add(getFloat(name,"reflectivity"));
                gameObject.Specular.Add(getVector(name,"specular"));
                gameObject.SpecularFactor.Add(getFloat(name, "specularity"));
                gameObject.Transparency.Add(getFloat(name, "alpha"));
            }
        }


        public MaterialReader(GameObject gameObject, string name)
        {
            this.gameObject = gameObject;
            xml=XDocument.Load("Data\\materials\\"+name+".xml");
           // Vector3 test = getVector("BlueGlass", "specular");
            //Vector4 ble;
        }
    }
}
