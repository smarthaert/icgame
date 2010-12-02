using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ICGame
{
    public class EffectController
    {
        private static Game MainGame;
        private EffectFactory effectFactory;

        public EffectController(Game mainGame)
        {
            MainGame = mainGame;
            effectFactory = new EffectFactory(mainGame);
        }

        public void Update(GameTime gameTime)
        {
            foreach (GameObject missionObject in MainGame.MissionController.GetMissionObjects())
            {
                foreach(IObjectEffect objectEffect in missionObject.EffectList)
                {
                 //   if(objectEffect.IsActive)
                     objectEffect.Update(gameTime);
                }
            }
        }

        //Dev
        public void SetEffects(bool state)
        {
            foreach (GameObject missionObject in MainGame.MissionController.GetMissionObjects())
            {
                foreach (IObjectEffect objectEffect in missionObject.EffectList)
                {
                    objectEffect.IsActive = state;
                }
            }
        }

        public TextureDrawer GetTextureDrawer()
        {
            return new TextureDrawer();
        }

        /// <summary>
        /// Deprecated
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="name"></param>
        private void AddEffectToAnObject(GameObject gameObject, string name) //TODO: ENUM!!!!!!!! (albo XML)
        {
            IObjectEffect gameEffect;
            if (name == "water")
                gameEffect = new WaterEffect(gameObject,MainGame);
            else if(name=="fire")
            {
                gameEffect = new FireEffect(gameObject, MainGame);
                AddEffectToAnObject(gameObject,"smoke");
                gameEffect.IsActive = true;
               
            }
            else
            {
                 gameEffect = new FireSmokeEffect(gameObject, MainGame);
                 gameEffect.IsActive = true;
            }
            
            gameObject.EffectList.Add(gameEffect);
        }

    }

}
