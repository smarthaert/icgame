using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ICGame
{
    public sealed class TechniqueProvider
    {
        private static Dictionary<string,Effect> effects;
        private static TechniqueProvider instance;

        protected TechniqueProvider(ContentManager contentManager)
        {
            LoadEffects(contentManager);
        }

        public static void StartTechniqueProvider(ContentManager contentManager)
        {
            if(instance == null)
            {
                instance = new TechniqueProvider(contentManager);
            }
        }

        public static TechniqueProvider GetTechniqueProvider()
        {
            if(instance == null)
            {
                throw new NullReferenceException("TechniqueProvider was not initialized");
            }
            return instance;
        }

        private void LoadEffects(ContentManager contentManager)
        {
            effects = new Dictionary<string, Effect>();

            DirectoryInfo directoryInfo = new DirectoryInfo(contentManager.RootDirectory + "\\ShaderEffects");

            if (!directoryInfo.Exists)
            {
                throw new IOException("Wrong directory");
            }

            FileInfo[] files = directoryInfo.GetFiles("*.xnb");
            foreach (FileInfo file in files)
            {
                string key = Path.GetFileNameWithoutExtension(file.Name);

                effects[key] = contentManager.Load<Effect>("ShaderEffects" + "/" + key);
            }
        }

        public static Effect GetEffect(string name)
        {
            if(effects == null)
            {
                throw new NullReferenceException("TechniqueProvider was not initialized");
            }
            if (!effects.ContainsKey(name))
            {
                throw new ArgumentOutOfRangeException("Invalid key");
            }
            return effects[name];
        }
    }
}
