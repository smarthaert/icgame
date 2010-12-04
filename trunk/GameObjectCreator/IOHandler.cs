using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace GameObjectCreator
{
    class IOHandler
    {
        private string path;
        public XDocument XDocument { get; set; }

        public IOHandler(string path)
        {
            this.path = path;
            ReadXMLFile();
        }

        private void ReadXMLFile()
        {
            XDocument = XDocument.Load(path);
        }

        public void Reset()
        {
            ReadXMLFile();
        }

        public void WriteXMLFile()
        {
            XDocument.Save(path);
        }
    }
}
